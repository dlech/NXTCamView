//
//  StretchyImageWidget.cs
//
//  Author:
//       David Lechner <david@lechnology.com>
//
//  Copyright (c) 2015 David Lechner
//
//  Based on http://stackoverflow.com/a/20469852/1976323
//  and http://stackoverflow.com/a/4941301/1976323
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

using Gdk;
using Gtk;

namespace NXTCamView.Gtk.Widgets
{
    [System.ComponentModel.ToolboxItem (true)]
    public partial class StretchyImage : global::Gtk.Bin
    {
        Pixbuf original;
        bool pixbufChanged;

        public Pixbuf Pixbuf {
            get {
                return original;
            }
            set {
                original = value;
                image.Pixbuf = value;
                pixbufChanged = true;
            }
        }

        public StretchyImage ()
        {
            this.Build ();
        }

        public void NotifyPixbufChanged ()
        {
            pixbufChanged = true;
            QueueDraw ();
        }

        protected void OnImageExposeEvent (object o, ExposeEventArgs args)
        {
            if (image.Pixbuf != null) {
                var widthError = Math.Abs (image.Pixbuf.Width - Allocation.Width);
                var heightError = Math.Abs (image.Pixbuf.Height - Allocation.Height);
                // There seems to be an off-by-one rounding error for the allocated width/height
                // that causes this to be called in an infinte loop if we try for an exact match.
                if (pixbufChanged || (widthError > 1 && heightError > 1)) {
                    int resultWidth, resultHeight;
                    ScaleRatio (original.Width, original.Height, Allocation.Width,
                        Allocation.Height, out resultWidth, out resultHeight);
                    image.Pixbuf = original.ScaleSimple (resultWidth, resultHeight,
                        InterpType.Tiles);
                    pixbufChanged = false;
                }
            }
        }

        static void ScaleRatio(int srcWidth, int srcHeight, int destWidth,
            int destHeight, out int resultWidth, out int resultHeight)
        {
            var widthRatio = (float)destWidth / srcWidth;
            var heigthRatio = (float)destHeight / srcHeight;

            var ratio = Math.Min(widthRatio, heigthRatio);
            resultHeight = (int)(srcHeight * ratio);
            resultWidth = (int)(srcWidth * ratio);
        }
    }
}
