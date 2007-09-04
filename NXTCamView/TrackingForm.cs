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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using NXTCamView.Commands;
using NXTCamView.Properties;

namespace NXTCamView
{
    public partial class TrackingForm : Form
    {
        public static TrackingForm Instance = new TrackingForm();
        private bool _isShowingDetail = true;

        private SerialPort _serialPort;
        private TrackingCommand _trackingCmd;
        private List<TrackedColor> _trackedToPaint;
        private Pen _penBlack = new Pen(Color.Red);
        private List<TrackingData> _trackingData = new List<TrackingData>(8);
        private int _scaling = 3;
        private int _matchesTotal;
        private int _matchesFiltered;
        private int _frameCount;
        private bool _isPausing;
        private int _lastSequence = int.MinValue;
        private int _sequence = 0;
        private Pen _detailPen;
        private Brush _detailBrush;

        //DO NOT CALL THIS DIRECTLY - use the Instance property
        public TrackingForm()
        {
            InitializeComponent();
            _serialPort = MainForm.Instance.SerialPort;
            pnlTrackedColors.MyPaint += new PaintEventHandler(pnlTracking_Paint);
            _trackingCmd = new TrackingCommand(_serialPort, objectsDetected);
            SetupColors();
            MainForm.Instance.SerialPortChanged += mainForm_SerialPortChanged;

            StickyWindowsUtil.MakeStickyMDIChild(this);
        }

        private void mainForm_SerialPortChanged(object sender, EventArgs e)
        {
            _serialPort = MainForm.Instance.SerialPort;
            _trackingCmd = new TrackingCommand(_serialPort, objectsDetected);
        }

        public void SetupColors()
        {
            _trackedToPaint = null;
            lvColors.Items.Clear();
            _trackingData.Clear();
            for (int i = 0; i < 8; i++)
            {
                Color color = Settings.Default.GetAverageUploadedColor(i);
                if (ColorUtils.IsNotSet(color))
                {
                    //skip as black == not set}
                    _trackingData.Add(new TrackingData(Color.Black,null));
                    continue; 
                }
                ListViewItem item = new ListViewItem(new string[] { (i + 1).ToString(), "", "0", "0" });
                item.UseItemStyleForSubItems = false;
                item.SubItems[1].BackColor = color;
                lvColors.Items.Add(item);
                _trackingData.Add(new TrackingData(color, item));
            }
            //show timestamp if there is one
            DateTime upload = Settings.Default.LastColorUpload;
            lblUploadTimestamp.Text = upload > new DateTime(2000, 1, 1) ? upload.ToString() : "";
            lvColors.Refresh();
        }

        public void SetVisibility(bool makeVisible)
        {
            if (!Visible && makeVisible)
            {
                MdiParent = MainForm.Instance;
                Show();
            }
            else if (Visible && !makeVisible)
            {
                Hide();
            }
        }

        private void TrackingForm_Load(object sender, EventArgs e)
        {
            Size formBorders = Size - ClientSize;
            int fixedWidth = ClientRectangle.Width - pnlTrackedColors.Width;
            int fixedHeight = ClientRectangle.Height - pnlTrackedColors.Height;
            lblMessage.Text = "";
            Size = formBorders + new Size((_scaling * FetchFrameCommand.IMAGE_WIDTH) + fixedWidth, (_scaling * FetchFrameCommand.IMAGE_HEIGHT) + fixedHeight);
            setDetailColor(pnlDetailColor.BackColor);
        }

        private void updateRow(TrackingData data)
        {
            if (data.LVItem != null)
            {
                data.LVItem.SubItems[2].Text = data.MatchesTotal.ToString();
                data.LVItem.SubItems[3].Text = data.MatchesFiltered.ToString();
            }
        }

        private int areaCompare(TrackedColor x, TrackedColor y)
        {
            //compare areas
            return (y.Bounds.Width * y.Bounds.Height).CompareTo(x.Bounds.Width * x.Bounds.Height);
        }

        private void startTracking()
        {
            if (!_trackingCmd.CanExecute() ) return;
            AppState.Instance.State = State.ConnectedTracking;
            lblMessage.Text = "Starting Tracking";
            lblMessage.ForeColor = Color.Black;
            lblMessage.Refresh();
            _trackingCmd.Execute();
            lblMessage.Text = _trackingCmd.IsSuccessful ? "Tracking Running" : _trackingCmd.ErrorDescription;
            lblMessage.ForeColor = _trackingCmd.IsSuccessful ? Color.Black : Color.Red;
            AppState.Instance.State = _trackingCmd.IsSuccessful ? State.ConnectedTracking : State.Connected;
            paintTimer.Start();
        }

        private void stopTracking()
        {
            lblMessage.Text = "Stopping Tracking";
            lblMessage.ForeColor = Color.Black;
            lblMessage.Refresh();
            _trackingCmd.StopTracking();
            lblMessage.Text = _trackingCmd.IsSuccessful ? "Tracking Stopped" : _trackingCmd.ErrorDescription;
            lblMessage.ForeColor = _trackingCmd.IsSuccessful ? Color.Black : Color.Red;
            if( _trackingCmd.IsSuccessful ) AppState.Instance.State = State.Connected;
            paintTimer.Stop();
        }

        private void objectsDetected(object sender, ObjectsDetectedEventArgs args)
        {
            //update the tracking data.  Actual painting is done off a timer to avoid killing CPU
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler<ObjectsDetectedEventArgs>(objectsDetected), new object[] { sender, args });
                return;
            }
            _sequence++;
            List<TrackedColor> trackedColors = new List<TrackedColor>(args.TrackedColors);
            if (!_isPausing) _trackedToPaint = new List<TrackedColor>();

            int lastMatchesTotal = _matchesTotal;

            foreach (TrackedColor trackedColor in trackedColors)
            {
                Debug.WriteLine("ColorIndex: " + trackedColor.ColorIndex);
                if (trackedColor.ColorIndex <= 0 || trackedColor.ColorIndex > _trackingData.Count)
                {
                    Debug.WriteLine(string.Format("Bad tracking index: {0}", trackedColor.ColorIndex));
                    continue;
                }
                TrackingData data = _trackingData[trackedColor.ColorIndex - 1];
                _matchesTotal++;
                data.MatchesTotal++;
                if ((trackedColor.Bounds.Width * trackedColor.Bounds.Height) < nudAreaFilter.Value)
                {
                    //skip it - too small
                    continue;
                }
                data.MatchesFiltered++;
                _matchesFiltered++;
                if (!_isPausing) _trackedToPaint.Add(trackedColor);
            }

            if (lastMatchesTotal != _matchesTotal)
            {
                _frameCount++;
            }
        }


        private void paintTimer_Tick(object sender, EventArgs e)
        {
            if (_trackedToPaint == null) return;
            if (_sequence == _lastSequence) return;
            _lastSequence = _sequence;

            foreach (TrackedColor trackedColor in _trackedToPaint)
            {
                TrackingData data = _trackingData[trackedColor.ColorIndex - 1];
                updateRow(data);
            }
            ////update stats
            lblTotalTracked.Text = _matchesTotal.ToString();
            lblOutOfRangeTracked.Text = (_matchesTotal - _matchesFiltered).ToString();
            lblInRangeTracked.Text = _matchesFiltered.ToString();
            lblFrameCount.Text = _frameCount.ToString();
            if (_frameCount > 0) lblMatchesPerFrame.Text = (_matchesTotal / _frameCount).ToString();

            //force actual tracking to repaint
            pnlTrackedColors.Invalidate();
        }


        void pnlTracking_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, 0, 0, pnlTrackedColors.Size.Width, pnlTrackedColors.Size.Height);
            if (_trackedToPaint == null) return;
            //sort to put little before big
            _trackedToPaint.Sort(new Comparison<TrackedColor>(areaCompare));
            foreach (TrackedColor trackedColor in _trackedToPaint)
            {
                TrackingData data = _trackingData[trackedColor.ColorIndex - 1];
                updateRow(data);

                Rectangle bounds = new Rectangle(
                    trackedColor.Bounds.X * _scaling,
                    trackedColor.Bounds.Y * _scaling,
                    trackedColor.Bounds.Width * _scaling,
                    trackedColor.Bounds.Height * _scaling);

                if (cbSolid.Checked)
                {
                    Brush blobBrush = cbUseColor.Checked ? data.Brush : Brushes.Gray;
                    e.Graphics.FillRectangle(blobBrush, bounds);
                    e.Graphics.DrawRectangle(Pens.Black, bounds);
                }
                else
                {
                    Pen pen = cbUseColor.Checked ? data.Pen : Pens.Black;
                    e.Graphics.DrawRectangle(pen, bounds);
                }

                //This determines a good color for writting inside the blob if needbe
                //Brush brush = null;
                //BlobStyle penType = (BlobStyle) ((cbUseColor.Checked ? 0x2 : 0x0) + (cbSolid.Checked ? 0x1 : 0x0));
                //switch (penType)
                //{
                //    case BlobStyle.NoColorOutline: brush = Brushes.Black; break;
                //    case BlobStyle.NoColorSolid: brush = Brushes.White; break;
                //    case BlobStyle.ColorOutline: brush = data.Brush; break;
                //    case BlobStyle.ColorSolid: brush = data.Color.GetBrightness() > 0.5 ? Brushes.Black : Brushes.White; break;
                //}

                List<string> lines = new List<string>(8);
                //put the number in the middle
                if (cbColorNumber.Checked)
                {
                    lines.Add(string.Format("C={0}", trackedColor.ColorIndex));
                }

                if (cbLocation.Checked)
                {
                    lines.Add(string.Format("X={0} Y={1}", trackedColor.Bounds.X, trackedColor.Bounds.Y));
                }
                if (cbSize.Checked)
                {
                    lines.Add(string.Format("W={0} H={1}", trackedColor.Bounds.Width, trackedColor.Bounds.Height));
                }
                if (cbArea.Checked)
                {
                    lines.Add(string.Format("A={0}", trackedColor.Bounds.X * trackedColor.Bounds.Y));
                }

                if (lines.Count > 0)
                {
                    //assume most characters = longest line
                    string longest = "";
                    foreach (string line in lines)
                    {
                        if (line.Length >= longest.Length) longest = line;
                    }
                    //measuresize gives short results sometimes so try this method
                    int textWidth = 10+MeasureDisplayStringWidth(e.Graphics, longest, lblDummyDetail.Font);
                    int textHeight = lblDummyDetail.Font.Height*lines.Count;
                    int offset = 5;
                    //assume the text will fit Bottom Right
                    Rectangle rect = new Rectangle(bounds.Right + offset, bounds.Bottom + offset, textWidth, textHeight);
                    //make it on screen
                    Point joinStart;
                    Point joinEnd;
                    if (pnlTrackedColors.Bounds.Contains(rect))
                    {
                        joinStart = new Point(bounds.X + bounds.Width/2, bounds.Bottom);
                        joinEnd = new Point(rect.X, rect.Y + rect.Height/2);
                    }
                    else
                    {
                        if( bounds.X + (bounds.Width/2) < pnlTrackedColors.Bounds.Width/2 )
                        {
                            //on Bottom Left so put the text Top Right
                            rect = new Rectangle(bounds.Right + offset, bounds.Top - (offset + textHeight), textWidth, textHeight);
                            joinStart = new Point(bounds.X + bounds.Width / 2, bounds.Y);
                            joinEnd = new Point(rect.X, rect.Y + rect.Height / 2);
                        }
                        else
                        {
                            if( bounds.Y + (bounds.Height/2) < pnlTrackedColors.Bounds.Height/2 )
                            {
                                //on the Top Right so put the text Bottom Left
                                rect = new Rectangle(bounds.X - (offset + textWidth), bounds.Bottom + offset, textWidth, textHeight);
                                joinStart = new Point(bounds.X + bounds.Width / 2, bounds.Bottom);
                                joinEnd = new Point(rect.Right, rect.Y + rect.Height / 2);
                            }
                            else
                            {
                                //on the Bottom Right so put the text Top Left
                                rect = new Rectangle(bounds.X - (offset + textWidth), bounds.Top - (offset + textHeight), textWidth, textHeight);
                                joinStart = new Point(bounds.X + bounds.Width / 2, bounds.Y);
                                joinEnd = new Point(rect.Right, rect.Y + rect.Height / 2);
                            }
                        }
                    }
                    e.Graphics.DrawRectangle(_detailPen, rect);
                    e.Graphics.DrawString(string.Join("\n", lines.ToArray()), lblDummyDetail.Font, _detailBrush, rect);
                    e.Graphics.DrawLine(_detailPen, joinStart, joinEnd );
                }
            }
        }

        static public int MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
        {
            StringFormat format = new StringFormat();
            RectangleF rect = new RectangleF(0, 0, 1000, 1000);
            CharacterRange[] ranges = { new CharacterRange(0, text.Length) };
            Region[] regions;

            format.SetMeasurableCharacterRanges(ranges);

            regions = graphics.MeasureCharacterRanges(text, font, rect, format);
            rect = regions[0].GetBounds(graphics);

            return (int)(rect.Right + 1.0f);
        }

        [Flags]
        enum BlobStyle
        {
            NoColorOutline = 0x0,
            NoColorSolid = 0x01,
            ColorOutline = 0x2,
            ColorSolid = 0x03,
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopTracking();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startTracking();
        }

        private void cb_Click(object sender, EventArgs e)
        {
            pnlTrackedColors.Refresh();
        }

        private void TrackingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //don't close it, just hide it
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            if (AppState.Instance.State == State.ConnectedTracking)
            {
                stopTracking();
            }
        }

        private void btnPause_MouseDown(object sender, MouseEventArgs e)
        {
            startPause();
        }

        private void startPause()
        {
            btnPause.Text = "PAUSING";
            btnPause.Refresh();
            _isPausing = true;
        }

        private void btnPause_MouseUp(object sender, MouseEventArgs e)
        {
            stopPause();
        }

        private void stopPause()
        {
            btnPause.Text = "Pause";
            btnPause.Refresh();
            _isPausing = false;
        }

        private void btnPause_MouseLeave(object sender, EventArgs e)
        {
            stopPause();
        }

        private void btnSetDetailColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = pnlDetailColor.BackColor;
            if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                setDetailColor(colorDialog.Color);
            }
        }

        private void setDetailColor(Color newColor)
        {
            if (_detailPen != null) _detailPen.Dispose();
            if (_detailBrush != null) _detailBrush.Dispose();
            pnlDetailColor.BackColor = newColor;
            _detailPen = new Pen(newColor);
            _detailBrush = new SolidBrush(newColor);
        }

        private void TrackingForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch( e.KeyCode )
            {
                case Keys.Space:
                    startPause();
                    e.Handled = true;
                    break;
                case Keys.T:
                    startTracking();
                    e.Handled = true;
                    break;
                case Keys.S:
                    stopTracking();
                    e.Handled = true;
                    break;
            }
        }

        private void TrackingForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    stopPause();
                    e.Handled = true;
                    break;
            }
        }

        private void llShowHideDetail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _isShowingDetail = !_isShowingDetail;
            llShowHideDetail.Text = _isShowingDetail ? "Hide" : "Detail";
            Height += (_isShowingDetail ? 1 : -1) * 200;
        }
    }

    public class TrackingData : IDisposable
    {
        public readonly Color Color;
        public readonly Brush Brush;
        public readonly Pen Pen;
        public readonly ListViewItem LVItem;
        private int _matchesTotal;
        private int _matchesFiltered;

        public int MatchesTotal { get { return _matchesTotal; } set { _matchesTotal = value; } }
        public int MatchesFiltered { get { return _matchesFiltered; } set { _matchesFiltered = value; } }

        public void AddFrame(int matchesFiltered, int matchesTotal)
        {
            _matchesTotal += matchesTotal;
            _matchesFiltered += matchesFiltered;            
        }

        public TrackingData(Color color, ListViewItem lvItem)
        {
            Color = color;
            Brush = new SolidBrush(color);
            Pen = new Pen(color);
            LVItem = lvItem;
        }

        public void Dispose()
        {
            Brush.Dispose();
            Pen.Dispose();
        }
    }
}