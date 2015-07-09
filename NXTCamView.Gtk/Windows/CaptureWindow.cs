//
//  CaptureWindow.cs
//
//  Author:
//       David Lechner <david@lechnology.com>
//
//  Copyright (c) 2015 David Lechner
//
//  Based on CaptureForm.cs
//  Copyright 2007 Paul Tingey
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Gdk;
using Gtk;

using NXTCamView.Core;
using NXTCamView.Core.Comms;
using NXTCamView.Core.Commands;

namespace NXTCamView.Gtk.Windows
{
    public partial class CaptureWindow : global::Gtk.Window
    {
        const uint mainStatusBarContext = 0;
        const uint captureStatusBarContext = 1;

        readonly IAppState appState;
        readonly ICommsPort commsPort;

        Pixbuf rawPixbuf;
        Pixbuf interpolatedPixbuf;
        BackgroundWorker worker;

        public CaptureWindow (IAppState appState, ICommsPort commsPort) :
            base (global::Gtk.WindowType.Toplevel)
        {
            this.appState = appState;
            this.commsPort = commsPort;
            this.worker = new BackgroundWorker () { WorkerReportsProgress = true };
            this.Build ();
            var width = FetchFrameCommand.ImageWidth;
            var height = FetchFrameCommand.ImageHeight;
            rawPixbuf = new Pixbuf (Colorspace.Rgb, false, 8, width, height);
            rawPixbuf.Fill (0);
            stretchyImage.Pixbuf = rawPixbuf;
            statusbar.Push (mainStatusBarContext, "Idle");
        }

        public void StartCapture ()
        {
            Title = string.Format(string.Format("NXTCapture{0:HHmmss}", DateTime.Now));
            //pbBayer.Visible = true;
            //pbInterpolated.Visible = false;
            appState.State = NXTCamView.Core.State.ConnectedBusy;
            progressbar.Fraction = 0;
            progressbar.Visible = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
            statusbar.Push (captureStatusBarContext, "Capturing");
        }

        /// <summary>
        /// Called on a worker thread to get the images
        /// </summary>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            FetchFrameCommand cmd = new DumpFrameCommand (appState, worker, commsPort);
            cmd.Execute();
            if (!cmd.IsSuccessful || cmd.Aborted) {
                e.Cancel = cmd.Aborted;
                e.Result = cmd.ErrorDescription;
                return;
            }
            e.Result = cmd.Interpolated;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Application.Invoke (delegate {
                if (e.ProgressPercentage >= 100) {
                    progressbar.Fraction = 1.0;
                    return;
                }
                //Copy in the next line
                LinePair linePair = (LinePair)e.UserState;
                var width = linePair.Colors.GetUpperBound (0);
                for (int x = 0; x < width; x++) {
                    var offset = 3 * x + rawPixbuf.Rowstride * linePair.Y;
                    Marshal.WriteByte (rawPixbuf.Pixels, offset + 0, linePair.Colors [x, 0].R);
                    Marshal.WriteByte (rawPixbuf.Pixels, offset + 1, linePair.Colors [x, 0].G);
                    Marshal.WriteByte (rawPixbuf.Pixels, offset + 2, linePair.Colors [x, 0].B);
                    offset += rawPixbuf.Rowstride;
                    Marshal.WriteByte (rawPixbuf.Pixels, offset + 0, linePair.Colors [x, 1].R);
                    Marshal.WriteByte (rawPixbuf.Pixels, offset + 1, linePair.Colors [x, 1].G);
                    Marshal.WriteByte (rawPixbuf.Pixels, offset + 2, linePair.Colors [x, 1].B);
                }
                stretchyImage.NotifyPixbufChanged ();
                progressbar.Fraction = e.ProgressPercentage / 100d;
            });
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Application.Invoke (delegate {
                worker.DoWork -= worker_DoWork;
                worker.ProgressChanged -= worker_ProgressChanged;
                worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
                statusbar.Pop (captureStatusBarContext);
                progressbar.Visible = false;
                completeFetch (e);
                appState.State = NXTCamView.Core.State.Connected;
                //was any one interested if we aborted?
//            if(e.Cancelled && AbortCompleted != null) {
//                AbortCompleted (this, new EventArgs ());
//            }
            });
        }

        void completeFetch(RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled) {
                statusbar.Push (mainStatusBarContext, "Aborted");
                statusbar.ModifyText (StateType.Normal, new Gdk.Color () { Red = 255 });
                //btnAbort.Enabled = false;
                return;
            }
            var errorMessage = e.Result as string;
            if (errorMessage != null) {
                statusbar.Push (mainStatusBarContext, errorMessage);
                statusbar.ModifyText (StateType.Normal, new Gdk.Color () { Red = 255 });
                //btnAbort.Enabled = false;
                return;
            }

            var bitmap = (Bitmap)e.Result;
            var rect = new System.Drawing.Rectangle (0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits (rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            interpolatedPixbuf = new Pixbuf (Colorspace.Rgb, false, 8, bitmap.Width, bitmap.Height);
            var pixelCount = data.Stride * data.Height / 3;
            for (int i = 0; i < pixelCount; i++) {
                var offset = i * 3;
                // System.Drawing.Bitmap is BRG order, Gtk.Pixmap is RGB order.
                var b = Marshal.ReadByte (data.Scan0, offset + 0);
                var g = Marshal.ReadByte (data.Scan0, offset + 1);
                var r = Marshal.ReadByte (data.Scan0, offset + 2);
                Marshal.WriteByte (interpolatedPixbuf.Pixels, offset + 0, r);
                Marshal.WriteByte (interpolatedPixbuf.Pixels, offset + 1, g);
                Marshal.WriteByte (interpolatedPixbuf.Pixels, offset + 2, b);
            }
            bitmap.UnlockBits (data);

            showIterpolatedCheckbutton.Active = true;
            //_isCaptured = true;
            //_colorForm.SetVisibility(true);
        }

        protected void OnShowIterpolatedCheckbuttonToggled (object sender, EventArgs e)
        {
            if (showIterpolatedCheckbutton.Active) {
                stretchyImage.Pixbuf = interpolatedPixbuf;
            } else {
                stretchyImage.Pixbuf = rawPixbuf;
            }
        }
    }
}
