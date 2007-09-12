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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Blue.Windows;
using NXTCamView.Commands;
using NXTCamView.Properties;
using NXTCamView.Resources;

namespace NXTCamView
{
    public partial class ColorForm : Form
    {
        private static ColorForm _instance;
        public static ColorForm Instance { get { return _instance; } }
        private int COLOR_DETAIL_HEIGHT = 241;
        private const int COLOR_PANEL_SPACING = 30;
        private const int TRACKED_COLORS = 8;
        private ColorDetail _colorDetail = new ColorDetail();
        private Panel[] _colorPanels = new Panel[TRACKED_COLORS];
        private OverlapInfo[] _overlapInfos = new OverlapInfo[TRACKED_COLORS];
        private Image _errorImage;
        private int _selectedColorIndex;
        private bool _isColorDetailVisible = false;
        private Color _highlightColorLow;
        private Color _highlightColorHigh;
        private ColorFunction _colorFunction;

        [Category("Custom")]
        public event EventHandler HighlightColorsChanged;
        [Category("Custom")]
        public Color HighlightColorLow { get { return _highlightColorLow; } }
        [Category("Custom")]
        public Color HighlightColorHigh { get { return _highlightColorHigh; } }

        [Category("Custom")]
        public event EventHandler ColorFunctionChanged;

        //DO NOT CALL THIS DIRECTLY - use the Inst property
        public ColorForm()
        {
            InitializeComponent();
            Icon = AppImages.GetIcon(AppImages.Colors);

            setColorFunction(NXTCamView.ColorFunction.Setting);

            initErrorImage();
        }

        public static void Init()
        {
            //doing here as causing issues from static constructor
            _instance = new ColorForm();
            StickyWindowsUtil.MakeStickyMDIChild(_instance);
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

        private void initErrorImage()
        {
            _errorImage = AppImages.Error;
            for (int i = 0; i < TRACKED_COLORS; i++)
            {
                _overlapInfos[i] = new OverlapInfo();
            }
        }

        public int SelectedColorIndex { get { return _selectedColorIndex; } set { _selectedColorIndex = value; } }

        private void setColorFunction(ColorFunction newFunction)
        {
            if (_colorFunction != newFunction)
            {
                _colorFunction = newFunction;
                _colorDetail.SetColorFunction(_colorFunction);
                if (ColorFunctionChanged != null)
                {
                    ColorFunctionChanged(this, new EventArgs());
                }
            }
        }

        public ColorFunction ColorFunction { get { return _colorFunction; } set { setColorFunction(value);} }

        private void ColorForm_Load(object sender, EventArgs e)
        {
            Controls.Add(_colorDetail);
            _colorDetail.Dock = DockStyle.Fill;
            _colorDetail.BringToFront();
            _colorDetail.Height = 0;
            _colorDetail.ColorChanged += new EventHandler(_colorDetail_ColorChanged);
            _colorDetail.Refresh();

            Settings settings = Settings.Default;
            _selectedColorIndex = settings.SelectedColorIndex;

            for (int index = 0; index < TRACKED_COLORS; index++)
            {
                DoubleBufferedPanel panel = new DoubleBufferedPanel();
                pnlColorPanels.Controls.Add(panel);
                panel.Size = pnlSample.Size;
                panel.BorderStyle = pnlSample.BorderStyle;
                panel.Anchor = pnlSample.Anchor;
                panel.Top = pnlSample.Top;
                panel.Left = pnlSample.Left + (COLOR_PANEL_SPACING * index) - ((TRACKED_COLORS - 1) * COLOR_PANEL_SPACING);
                panel.BackColor = settings.GetAverageColor(index);
                panel.Tag = index;
                panel.Click += new EventHandler(panel_Click);
                panel.MyPaint += new PaintEventHandler(panel_MyPaint);
                panel.MouseHover += new EventHandler(panel_MouseHover);
                _colorPanels[index] = panel;
                updateOverlapStatus(index);
            }
            pnlColorPanels.Paint += pnlColorPanels_Paint;
            _colorDetail.SetColorMinMax(settings.MinColors[_selectedColorIndex], settings.MaxColors[_selectedColorIndex]);

            //ensure the tooltip is ok
            toolTipPanel.Show("", this);
            toolTipPanel.Hide(this);

            pnlSample.Visible = false;

            //hack to try and get the tooltip appearing the Right way around
            llColorDetail.Focus();

            toggleColorDetail();

            positionTopRight();
        }

        private void positionTopRight()
        {
            MdiClient mdiClient = StickyWindow.GetMdiClient(MainForm.Instance);
            if( mdiClient != null )
            {
                Top = 0;
                Left = mdiClient.ClientRectangle.Width - Width;
            }
        }

        void panel_MouseHover(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                int index = (int)panel.Tag;
                OverlapInfo overlapInfo = _overlapInfos[index];
                if (overlapInfo.IsOverlapped)
                {
                    string message = overlapInfo.Message + ".\nSelect colors that are less similar or make the tolerance smaller.";
                    toolTipPanel.Show(message, panel, panel.Width - 5, panel.Height, 4000);
                }
                else
                {
                    if (toolTipPanel.Active) toolTipPanel.Hide(this);
                }
            }
        }

        void panel_MyPaint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            Brush brush = null;
            try
            {
                bool isNotSet = ColorUtils.IsNotSet(panel.BackColor);
                brush = isNotSet ?
                    (Brush)new HatchBrush(HatchStyle.Percent30, Color.DarkGray, Color.White) :
                    new SolidBrush(panel.BackColor);
                using (Pen outsidePen = new Pen(pnlColorPanels.BackColor))
                {
                    //draw the fill and rectange by hand to allow the icon to overlay it
                    e.Graphics.DrawRectangle(outsidePen, 0, 0, panel.Width - 1, panel.Height - 1);
                    e.Graphics.FillRectangle(brush, 1, 1, panel.Width - 3, panel.Height - 3);
                    e.Graphics.DrawRectangle(Pens.Black, 0, 0, panel.Width - 2, panel.Height - 2);
                    int index = (int)panel.Tag;
                    Brush textBrush = panel.BackColor.GetBrightness() > 0.5 || isNotSet ? Brushes.Black : Brushes.White;
                    e.Graphics.DrawString((index + 1).ToString(), Font, textBrush, 4, 3);
                    if (_overlapInfos[index].IsOverlapped)
                    {
                        e.Graphics.DrawImage(_errorImage, new Rectangle(6, 5, 16, 16));
                    }
                }
            }
            finally
            {
                if (brush != null)
                {
                    brush.Dispose();
                }
            }
        }

        void pnlColorPanels_Paint(object sender, PaintEventArgs e)
        {
            //draw the selected panel
            Rectangle rect = pnlSample.Bounds;
            rect.X = pnlSample.Left + (COLOR_PANEL_SPACING * _selectedColorIndex) - ((TRACKED_COLORS - 1) * COLOR_PANEL_SPACING) - 2;
            rect.Y -= 2;
            rect.Width += 3;
            rect.Height += 3;
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        void panel_Click(object sender, EventArgs e)
        {
            if (toolTipPanel.Active) toolTipPanel.Hide(this);
            setSelectedColor((int)((Panel)sender).Tag);
            if (cbHighLight.Checked)
            {
                setHighlightColors();
            }
        }

        void _colorDetail_ColorChanged(object sender, EventArgs e)
        {
            Settings.Default.MinColors[_selectedColorIndex] = _colorDetail.MinColor;
            Settings.Default.MaxColors[_selectedColorIndex] = _colorDetail.MaxColor;
            _colorPanels[_selectedColorIndex].BackColor = Settings.Default.GetAverageColor(_selectedColorIndex);
            SetActiveColor( Settings.Default.GetAverageColor(_selectedColorIndex) );
            updateOverlapStatusAndMessage(_selectedColorIndex);
            updateUploadButton();
            setHighlightColors();
        }

        private void updateUploadButton()
        {
            foreach( OverlapInfo info in _overlapInfos )
            {
                if( info.IsOverlapped)
                {
                    btnUpload.Enabled = false;
                    return;
                }
            }
            btnUpload.Enabled = true;
        }

        private void updateOverlapStatusAndMessage(int index)
        {
            updateOverlapStatus(index);
            bool isOverlapped = _overlapInfos[index].IsOverlapped;
            _colorDetail.lblWarning.Text = isOverlapped ? _overlapInfos[index].Message : "";
            if (toolTipPanel.Active) toolTipPanel.Hide(this);
            pnlColorPanels.Refresh();
        }

        private void setSelectedColor(int index)
        {
            _selectedColorIndex = index;
            pnlColorPanels.Invalidate();
            _colorDetail.SetColorMinMax(Settings.Default.MinColors[_selectedColorIndex], Settings.Default.MaxColors[_selectedColorIndex]);
        }

        public void SetActiveColor(Color color )
        {
            _colorDetail.SetActiveColor(color);
            if( color == Color.Empty ) return;
            pnlActiveColor.BackColor = color;
            lbActiveColor.Text = string.Format("r:{0} g:{1} b:{2}", color.R, color.G, color.B);
        }

        public void SetSelectedColor()
        {
            if (toolTipPanel.Active) toolTipPanel.Hide(this);
            if (_colorFunction == ColorFunction.Setting)
            {
                _colorDetail.BaseColor = pnlActiveColor.BackColor;
            }
            else
            {
                _colorDetail.ApplyModifiedRange();
            }
            //is this needed?
            pnlColorPanels.Refresh();
        }

        private void llColorDetail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            toggleColorDetail();
        }

        private void toggleColorDetail()
        {
            SuspendLayout();
            _isColorDetailVisible = !_isColorDetailVisible;
            int heightChange = _isColorDetailVisible ? COLOR_DETAIL_HEIGHT : -COLOR_DETAIL_HEIGHT;
            Height += heightChange;
            llColorDetail.Text = _isColorDetailVisible ? "Hide" : "Show";
            ResumeLayout();
        }

        private void ColorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //don't close it, just hide it
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            Settings.Default.SelectedColorIndex = _selectedColorIndex;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            int red = 0;
            int green = 16;
            int blue = 32;
            try
            {
                //prepare the color map
                byte[] colorMap = new byte[3*16];
                for( int objectNum = 0; objectNum < TRACKED_COLORS; objectNum++ )
                {
                    Color minColor = Settings.Default.MinColors[objectNum];
                    Color maxColor = Settings.Default.MaxColors[objectNum];
                    if( ColorUtils.IsNotSet(minColor, maxColor) ) continue; //skip as black == not set
                    for( int offset = 0; offset < 16; offset++ )
                    {
                        //this is weird, but the gets us to the layout we need
                        //div by 17 to get it back to 0-15
                        byte redMask =
                            (byte) ((offset >= minColor.R/17 && offset <= maxColor.R/17) ? (1 << objectNum) : 0);
                        colorMap[red + offset] |= redMask;

                        byte greenMask =
                            (byte) ((offset >= minColor.G/17 && offset <= maxColor.G/17) ? (1 << objectNum) : 0);
                        colorMap[green + offset] |= greenMask;

                        byte blueMask =
                            (byte) ((offset >= minColor.B/17 && offset <= maxColor.B/17) ? (1 << objectNum) : 0);
                        colorMap[blue + offset] |= blueMask;
                    }
                }

                Debug.Write("colorMap: ");
                for( int i = 0; i < 16*3; i++ )
                {
                    if( i%16 == 0 ) Debug.Write("- ");
                    Debug.Write(String.Format("{0:x} ", colorMap[i]));
                }
                Debug.WriteLine("");
                SetColorMapCommand cmd = new SetColorMapCommand(MainForm.Instance.SerialPort);
                cmd.ColorMap = new List< byte >(colorMap);
                cmd.Execute();
                if( cmd.IsSuccessful )
                {
                    Settings.Default.UploadedMinColors = (Color[]) Settings.Default.MinColors.Clone();
                    Settings.Default.UploadedMaxColors = (Color[]) Settings.Default.MaxColors.Clone();
                    Settings.Default.LastColorUpload = DateTime.Now;
                    TrackingForm.Instance.SetupColors();

                    MessageBox.Show(this, "Color upload was successful!", Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show(this, cmd.ErrorDescription, Application.ProductName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch( Exception ex )
            {
                Debug.WriteLine(string.Format("Error uploading colors {0}", ex));
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            pnlColorPanels.Refresh();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearColor(_selectedColorIndex);
            //refresh the sliders
            setSelectedColor(_selectedColorIndex);
            pnlColorPanels.Refresh();
        }

        private void clearAll()
        {
            for (int i = 0; i < _colorPanels.Length; i++) clearColor(i);
            pnlColorPanels.Refresh();
            //refresh the sliders
            setSelectedColor(0);
        }

        private void clearColor(int index)
        {
            Settings.Default.MinColors[index] = Color.Black;
            Settings.Default.MaxColors[index] = Color.Black;
            _colorPanels[index].BackColor = Settings.Default.GetAverageColor(index);
            updateOverlapStatusAndMessage(index);
        }

        private void updateOverlapStatus(int index)
        {
            updateOverlapStatus(index, TRACKED_COLORS, Settings.Default.MinColors[index], Settings.Default.MaxColors[index]);
        }

        private void updateOverlapStatus(int index, int colorCount, Color minColor1, Color maxColor1)
        {
            bool isNotSet = ColorUtils.IsNotSet(minColor1, maxColor1);
            _overlapInfos[index].ClearOverlaps();
            for (int i = 0; i < colorCount; i++)
            {
                if (i == index) continue;  //skip the color we are setting

                if (isNotSet)
                {
                    //ensure its clear on color2
                    _overlapInfos[i].SetOverlapStatus(index,false);
                }
                else
                {
                    Color minColor2 = Settings.Default.MinColors[i];
                    Color maxColor2 = Settings.Default.MaxColors[i];

                    if( ColorUtils.IsNotSet(minColor2, maxColor2) )
                    {
                        //tidy up
                        _overlapInfos[i].ClearOverlaps();
                        continue;
                    }

                    bool isRedOverlapped = IsBetween(minColor1.R, minColor2.R, maxColor2.R) ||
                                               IsBetween(maxColor1.R, minColor2.R, maxColor2.R);
                    bool isGreenOverlapped = IsBetween(minColor1.G, minColor2.G, maxColor2.G) ||
                                             IsBetween(maxColor1.G, minColor2.G, maxColor2.G);
                    bool isBlueOverlapped = IsBetween(minColor1.B, minColor2.B, maxColor2.B) ||
                                            IsBetween(maxColor1.B, minColor2.B, maxColor2.B);
                    bool isOverlapped = isRedOverlapped && isBlueOverlapped && isGreenOverlapped;

                    //set/clear the new overlap status on color2
                    _overlapInfos[i].SetOverlapStatus(index, isOverlapped);
                    //only set the new overlap staus on color1 is set
                    if( isOverlapped ) _overlapInfos[index].SetOverlapStatus(i, true);
                }
            }
        }

        private bool IsBetween(Byte val, Byte min, Byte max)
        {
            return min < max ? (val >= min) && (val <= max) : (val <= min) && (val >= max);
        }

        class OverlapInfo
        {
            private List<Int32> _overlappedIndices = new List<Int32>();
            public bool IsOverlapped { get { return _overlappedIndices.Count > 0; } }
            public string Message
            {
                get
                {
                    if (_overlappedIndices.Count == 0) return "No overlaps";
                    _overlappedIndices.Sort();
                    StringBuilder sb = new StringBuilder("This color overlaps with color(s) ");
                    for (int i = 0; i < _overlappedIndices.Count - 1; i++)
                    {
                        sb.Append((_overlappedIndices[i] + 1) + ",");
                    }
                    sb.Append(_overlappedIndices[_overlappedIndices.Count - 1] + 1);
                    return sb.ToString();
                }
            }

            public void ClearOverlaps() { _overlappedIndices = new List<int>(); }

            public void SetOverlapStatus(int index, bool isOverlapped)
            {
                if (isOverlapped)
                {
                    if (!_overlappedIndices.Contains(index)) _overlappedIndices.Add(index);
                }
                else
                {
                    if (_overlappedIndices.Contains(index)) _overlappedIndices.Remove(index);
                }
            }
        }

        private void raiseHighlightColorChanged()
        {
            if( HighlightColorsChanged != null )
            {
                HighlightColorsChanged(this,new EventArgs());
            }
        }

        private void cbHighLight_CheckedChanged(object sender, EventArgs e)
        {
            setHighlightColors();
        }

        private void setHighlightColors()
        {
            _highlightColorLow = cbHighLight.Checked ? getHightlightColor(Settings.Default.MinColors[_selectedColorIndex]) : Color.Empty;
            _highlightColorHigh = cbHighLight.Checked ? getHightlightColor(Settings.Default.MaxColors[_selectedColorIndex]) : Color.Empty;
            raiseHighlightColorChanged();
        }

        private Color getHightlightColor(Color color)
        {
            if( ColorUtils.IsNotSet(color) )
            {
                return Color.Empty;
            }
            return color;
        }

        private void ColorForm_SizeChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(Bounds);
        }

        private void ColorForm_Move(object sender, EventArgs e)
        {
            Debug.WriteLine(Bounds);
        }

        private void ColorForm_KeyUpDown(object sender, KeyEventArgs e)
        {
            ColorFunction = ColorUtils.GetColorFunction(ModifierKeys);
        }
    }

    public enum ColorFunction
    {
        Setting,
        Adding,
        Removing
    }
}