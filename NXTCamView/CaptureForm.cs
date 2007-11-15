//
//    Copyright 2007 Paul Tingey
//
//    This file is part of NXTCamView.
//
//    NXTCamView is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 3 of the License, or
//    (at your option) any later version.
//
//    Foobar is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using NXTCamView.Commands;
using NXTCamView.Resources;

namespace NXTCamView
{
    public partial class CaptureForm : Form, ITaskRunner
    {
        private bool _isCaptured = false;
        private Bitmap _resizeInterpolated;
        private ISerialProvider _serialProvider;
        private string _filename = "";
        private bool _isHighlighting = true;
        private readonly Cursor _addingColor;
        private readonly Cursor _removingColor;


        public CaptureForm() : this(null)
        {            
        }

        public CaptureForm( ISerialProvider serialProvider )
        {
            InitializeComponent();
            Icon = AppImages.GetIcon(AppImages.Capture);
            _serialProvider = serialProvider;
            ColorForm.Instance.HighlightColorsChanged += colorDetail_HighlightColorsChanged;
            ColorForm.Instance.ColorFunctionChanged += colorDetail_ColorFunctionChanged;
            StickyWindowsUtil.MakeStickyMDIChild(this);

            _addingColor = AppCursors.AddingColor;
            _removingColor = AppCursors.RemovingColor;
        }

        public string Filename { get { return _filename; } set { _filename = value; } }

        private void CaptureForm_Load(object sender, EventArgs e)
        {
            pbBayer.Image = new Bitmap(FetchFrameCommand.IMAGE_WIDTH, FetchFrameCommand.IMAGE_HEIGHT);
            pbBayer.Visible = true;
            pbInterpolated.Visible = false;
            pnlBottom.Visible = false;
            Size = new Size(3 * FetchFrameCommand.IMAGE_WIDTH + 10, 3 * FetchFrameCommand.IMAGE_HEIGHT + pnlProgress.Height);
        }


        private void captureForm_Resize(object sender, EventArgs e)
        {
            resizeInterpolated();
        }


        private void resizeInterpolated()
        {
            _resizeInterpolated = new Bitmap(pbInterpolated.Size.Width,pbInterpolated.Size.Height);
            pbInterpolated.DrawToBitmap(_resizeInterpolated,pbInterpolated.ClientRectangle);
        }


        public void StartCapture()
        {
            Text = string.Format(string.Format("NXTCapture{0:HHmmss}", DateTime.Now));
            pbBayer.Visible = true;
            pbInterpolated.Visible = false;
            AppState.Inst.State = State.ConnectedBusy;
            pbProgress.Value = 0;
            worker.DoWork += workerCapture_DoWork;
            worker.ProgressChanged += workerCapture_ProgressChanged;
            worker.RunWorkerCompleted += workerCapture_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Called on a worker thread to get the images
        /// </summary>
        private void workerCapture_DoWork(object sender, DoWorkEventArgs e)
        {            
            FetchFrameCommand cmd = new DumpFrameCommand(worker, _serialProvider );
            cmd.Execute();
            if( !cmd.IsSuccessful || cmd.Aborted )
            {
                e.Cancel = cmd.Aborted;
                e.Result = cmd.ErrorDescription;
                return;
            }
            e.Result = cmd.Interpolated;
        }
        
        private void workerCapture_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            if (e.ProgressPercentage < 100)
            {
                lbStatus.Text = "Capturing";
                //Copy in the next line
                LinePair linePair = (LinePair) e.UserState;
                Bitmap bitmap = (Bitmap) pbBayer.Image;
                for (int x=0; x < linePair.Colors.GetUpperBound(0); x++ )
                {
                    bitmap.SetPixel(x, linePair.Y, linePair.Colors[x,0]);
                    bitmap.SetPixel(x, linePair.Y+1, linePair.Colors[x,1]);
                }
                pbBayer.Refresh();
                pbProgress.Value = e.ProgressPercentage;
            }
            else
            {
                lbStatus.Text = "Interpolating";
                //Updating interpolated image
                pbProgress.Value = e.ProgressPercentage - 100;
            }
        }

        private void workerCapture_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= workerCapture_DoWork;
            worker.ProgressChanged -= workerCapture_ProgressChanged;
            worker.RunWorkerCompleted -= workerCapture_RunWorkerCompleted;
            completeFetch(e);
            AppState.Inst.State = State.Connected;
            //was any one interested if we aborted?
            if( e.Cancelled && AbortCompleted!= null )
            {
                AbortCompleted( this, new EventArgs( ) );
            }
        }

        private void completeFetch(RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lbStatus.Text = "Aborted";
                lbStatus.ForeColor = Color.Red;
                btnAbort.Enabled = false;
                pbProgress.Visible = false;
                return;
            }
            if (e.Result is string)
            {
                lbStatus.Text = (string) e.Result;
                lbStatus.ForeColor = Color.Red;
                btnAbort.Enabled = false;
                pbProgress.Visible = false;
                return;
            }
            lbStatus.Visible = false;
            pbBayer.Visible = false;
            pnlProgress.Visible = false;
            pnlBottom.Visible = true;

            pbInterpolated.Image = (Bitmap)e.Result;
            pbInterpolated.Visible = true;
            pbInterpolated.Refresh();

            resizeInterpolated();
            _isCaptured = true;
            ColorForm.Instance.SetVisibility(true);
        }

        private void cbHighlightColors_CheckedChanged(object sender, EventArgs e)
        {
            hightlightTimer.Enabled = cbHighlightColors.Checked;
            updateHighlighting();
        }

        void colorDetail_HighlightColorsChanged(object sender, EventArgs e)
        {
            if (cbHighlightColors.Checked && !hightlightTimer.Enabled) hightlightTimer.Enabled = true;
            updateHighlighting();
        }

        private void updateHighlighting()
        {
            if (pbInterpolated.Visible)
            {
                if( cbHighlightColors.Checked )
                {
                    Color low = Color.FromArgb( 254, ColorForm.Instance.HighlightColorLow);
                    pbInterpolated.TransarentColorLow = low;
                    pbInterpolated.TransarentColorHigh = ColorForm.Instance.HighlightColorHigh;
                }
                else
                {
                    pbInterpolated.TransarentColorLow = Color.Empty;
                    pbInterpolated.TransarentColorHigh = Color.Empty;
                }
                pbInterpolated.Invalidate();
            }
        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            if( !_isCaptured ) return;            
            if( e.X < 0 || 
                e.X >= _resizeInterpolated.Width || 
                e.Y < 0 || 
                e.Y >= _resizeInterpolated.Height)
            {
                ColorForm.Instance.SetActiveColor( Color.Empty );
                return;
            }

            Color color = _resizeInterpolated.GetPixel(e.X, e.Y);
            ColorForm.Instance.SetActiveColor(color );
        }

        private void colorDetail_ColorFunctionChanged(object sender, EventArgs e)
        {
            switch( ColorForm.Instance.GetColorFunction() )
            {
                case ColorFunction.AddToColor:
                    pbInterpolated.Cursor = _addingColor;
                    pbBayer.Cursor = _addingColor;
                    break;
                case ColorFunction.RemoveFromColor:
                    pbInterpolated.Cursor = _removingColor;
                    pbBayer.Cursor = _removingColor;
                    break;
                default:
                    pbInterpolated.Cursor = Cursors.Default;
                    pbBayer.Cursor = Cursors.Default;
                    break;
            }
        }

        private void pbInterpolated_Click(object sender, EventArgs e)
        {            
            ColorForm.Instance.SetSelectedColor();
        }


        private void btnAbort_Click(object sender, EventArgs e)
        {
            Abort( );
        }

        public bool IsBusy()
        {
            return worker.IsBusy;
        }

        public void Abort( )
        {
            worker.CancelAsync();
        }

        public event EventHandler< EventArgs > AbortCompleted;

        public void AbortJob( )
        {
            worker.CancelAsync();
        }

        private void cbInterpolated_CheckedChanged(object sender, EventArgs e)
        {
            pbInterpolated.Visible = !pbInterpolated.Visible;
            pbBayer.Visible = !pbBayer.Visible;
        }

        public void SaveFile(string filename)
        {
            using (FileStream stream = new FileStream(filename,FileMode.OpenOrCreate))
            {
                //bayerimage
                pbBayer.Image.Save(stream, ImageFormat.Png);
            }
        }

        public void LoadFile(string filename)
        {
            pbBayer.Visible = true;
            pbInterpolated.Visible = false;
            Image image;
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                //bayerimage
                image = Image.FromStream(stream);
            }
            Text = Path.GetFileNameWithoutExtension(filename);
            worker.DoWork += workerOpen_DoWork;
            worker.ProgressChanged += workerOpen_ProgressChanged;
            worker.RunWorkerCompleted += workerOpen_RunWorkerCompleted;
            worker.RunWorkerAsync(new Bitmap(image));
            pbBayer.Image = new Bitmap(image);
        }

        private void workerOpen_DoWork(object sender, DoWorkEventArgs e)
        {
            Image image = (Image) e.Argument;
            InterpolateFrameCommand cmd = new InterpolateFrameCommand(worker, null, image);
            cmd.Execute();
            if (!cmd.IsSuccessful || cmd.Aborted)
            {
                e.Cancel = cmd.Aborted;
                e.Result = cmd.ErrorDescription;
                return;
            }
            e.Result = cmd.Interpolated;
        }

        private void workerOpen_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lbStatus.Text = "Interpolating";
            //Updating interpolated image
            pbProgress.Value = e.ProgressPercentage - 100;
        }

        private void workerOpen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= workerOpen_DoWork;
            worker.ProgressChanged -= workerOpen_ProgressChanged;
            worker.RunWorkerCompleted -= workerOpen_RunWorkerCompleted;
            completeFetch(e);
            setTransparentColor();
        }

        private void hightlightTimer_Tick(object sender, EventArgs e)
        {
            if( pbInterpolated.Visible )
            {
                setTransparentColor();
            }
        }

        private void setTransparentColor()
        {
            Color color = ColorUtils.GetAverage(ColorForm.Instance.HighlightColorLow, ColorForm.Instance.HighlightColorHigh);
            Color highlightColor = color.GetBrightness() > 0.5 ? Color.Blue : Color.Yellow;

            _isHighlighting = !_isHighlighting;
            pbInterpolated.HighlightColor = _isHighlighting ? highlightColor : Color.Empty;
        }

        private void pb_MouseLeave(object sender, EventArgs e)
        {
            ColorForm.Instance.ColorFunctionTemp = ColorFunction.NotSet;
        }

        private void captureForm_KeyUpDown(object sender, KeyEventArgs e)
        {
            ColorForm.Instance.ColorFunctionTemp = ColorUtils.GetColorFunction(ModifierKeys);
        }
    }
}