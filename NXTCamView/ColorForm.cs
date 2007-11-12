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
using NXTCamView.Properties;
using NXTCamView.Resources;
using NXTCamView.StripCommands;

namespace NXTCamView
{
    public partial class ColorForm : Form
    {
        private static ColorForm _instance;
        public static ColorForm Instance { get { return _instance; } }
        private int COLOR_DETAIL_HEIGHT = 241;
        private const int COLOR_PANEL_SPACING = 30;
        public const int TRACKED_COLORS = 8;
        private ColorDetail _colorDetail = new ColorDetail();
        private Panel[] _colorPanels = new Panel[TRACKED_COLORS];
        private OverlapInfo[] _overlapInfos = new OverlapInfo[TRACKED_COLORS];
        private static Image _errorImage;
        private int _selectedColorIndex;
        private bool _isColorDetailVisible = false;
        private Color _highlightColorLow;
        private Color _highlightColorHigh;
        private ColorFunction _colorFunctionTemp;
        private ColorFunction _colorFunctionMode;
        private HighlightColorStripCommand _highlightColorCmd;

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
            setupColorFunction();
            setupToolbar();
            initErrorImage();
            MainForm.Instance.ConnectionStateChanged += mainForm_ConnectionStateChanged;
            AppState.Inst.StateChanged += AppStateChanged;
        }

        private void AppStateChanged(object sender, EventArgs e)
        {
            updateAllEnablement();
        }

        private void mainForm_ConnectionStateChanged(object sender, EventArgs e)
        {
            updateAllEnablement();
        }

        private void setupColorFunction()
        {
            _colorFunctionTemp = ColorFunction.NotSet;
            _colorFunctionMode = ColorFunction.SetColor;
            updateColorFunction();
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

        private void setTempColorFunction(ColorFunction newFunction)
        {
            if (_colorFunctionTemp != newFunction)
            {
                _colorFunctionTemp = newFunction;
                updateColorFunction();
            }
        }

        private void setColorFunctionMode(ColorFunction newFunction)
        {
            if (_colorFunctionMode != newFunction)
            {
                _colorFunctionMode = newFunction;
                updateColorFunction();
            }
        }

        private void updateColorFunction()
        {
            _colorDetail.SetColorFunction(GetColorFunction());
            if (ColorFunctionChanged != null)
            {
                ColorFunctionChanged(this, new EventArgs());
            }
            updateAllEnablement();
        }

        public ColorFunction GetColorFunction()
        {
            return _colorFunctionTemp == ColorFunction.NotSet ? _colorFunctionMode : _colorFunctionTemp;
        }

        public ColorFunction ColorFunctionTemp { get { return _colorFunctionTemp; } set { setTempColorFunction(value); } }
        public ColorFunction ColorFunctionMode { get { return _colorFunctionMode; } set { setColorFunctionMode(value); } }

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

            toggleDetailVisible();

            positionTopRight();
        }

        private void setupToolbar()
        {
            setupItem(tsbSetColor, "Set color", "Use color setting mode", AppImages.SetColor, new SetColorFunctionModeStripCommand(this, ColorFunction.SetColor));
            setupItem(tsbAddToColor, "Add to color", "Use color adding mode (or press CTRL while clicking)", AppImages.AddToColor, new SetColorFunctionModeStripCommand(this, ColorFunction.AddToColor));
            setupItem(tsbRemoveFromColor, "Remove from color", "Use color removing mode (or press CTRL+SHIFT while clicking)", AppImages.RemoveFromColor, new SetColorFunctionModeStripCommand(this, ColorFunction.RemoveFromColor));

            setupItem(tsbUpload, "Upload", "Upload tracking colors to NXTCam", AppImages.UploadColors, new UploadColorsStripCommand( this,  SerialProvider.Instance ));
            setupItem(tsbClear, "Clear", "Clear color", AppImages.ClearColor, new ClearColorsStripCommand(false, this));
            setupItem(tsbClearAll, "Clear All", "Clear all colors", AppImages.ClearAllColors, new ClearColorsStripCommand(true, this));
            _highlightColorCmd = new HighlightColorStripCommand(true, this);
            setupItem(tsbHighlight, "Highlight", "Highlight the color in the capture", AppImages.HighlightColor, _highlightColorCmd);
            setupItem(tslShowHide, "Hide", "Show/Hide colors", null, new ToggleShowHideStripCommand(this));
        }

        private void positionTopRight()
        {
            MdiClient mdiClient = StickyWindow.GetMdiClient(MainForm.Instance);
            if (mdiClient != null)
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
            if (_highlightColorCmd.CanExecute())
            {
                setHighlightColors();
            }
        }

        void _colorDetail_ColorChanged(object sender, EventArgs e)
        {
            Settings.Default.MinColors[_selectedColorIndex] = _colorDetail.MinColor;
            Settings.Default.MaxColors[_selectedColorIndex] = _colorDetail.MaxColor;
            _colorPanels[_selectedColorIndex].BackColor = Settings.Default.GetAverageColor(_selectedColorIndex);
            SetActiveColor(Settings.Default.GetAverageColor(_selectedColorIndex));
            updateOverlapStatusAndMessage(_selectedColorIndex);
            updateAllEnablement();
            setHighlightColors();
        }

        private void updateAllEnablement()
        {
            updateEnablement(tsToolBar.Items);
        }

        private void updateEnablement(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item != null)
                {
                    StripCommand cmd = item.Tag as StripCommand;
                    if (cmd != null)
                    {
                        item.Enabled = cmd.CanExecute();

                        ToolStripButton button = item as ToolStripButton;
                        if (button != null) button.Checked = cmd.HasExecuted();

                        ToolStripMenuItem menu = item as ToolStripMenuItem;
                        if (menu != null) menu.Checked = cmd.HasExecuted();
                    }
                }
            }
        }

        public bool IsAnyOverlapped()
        {
            bool isOverlapped = false;
            foreach (OverlapInfo info in _overlapInfos)
            {
                if (info.IsOverlapped)
                {
                    isOverlapped = true;
                }
            }
            return isOverlapped;
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

        public void SetActiveColor(Color color)
        {
            _colorDetail.SetActiveColor(color);
            if (color == Color.Empty) return;
            pnlActiveColor.BackColor = color;
            lbActiveColor.Text = string.Format("r:{0} g:{1} b:{2}", color.R, color.G, color.B);
        }

        public void SetSelectedColor()
        {
            if (toolTipPanel.Active) toolTipPanel.Hide(this);
            if ( GetColorFunction() == ColorFunction.SetColor )
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

        private void toggleDetailVisible()
        {
            SuspendLayout();
            _isColorDetailVisible = !_isColorDetailVisible;
            int heightChange = _isColorDetailVisible ? COLOR_DETAIL_HEIGHT : -COLOR_DETAIL_HEIGHT;
            tslShowHide.Text = _isColorDetailVisible ? "Hide" : "Show";
            Height += heightChange;
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

        private void clearColor()
        {
            clearColor(_selectedColorIndex);
            //refresh the sliders
            setSelectedColor(_selectedColorIndex);
            pnlColorPanels.Refresh();
        }

        private void clearAllColors()
        {
            for (int i = 0; i < _colorPanels.Length; i++) clearColor(i);
            pnlColorPanels.Refresh();
            //refresh the sliders
            setSelectedColor(0);
            pnlColorPanels.Refresh();
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
                    _overlapInfos[i].SetOverlapStatus(index, false);
                }
                else
                {
                    Color minColor2 = Settings.Default.MinColors[i];
                    Color maxColor2 = Settings.Default.MaxColors[i];

                    if (ColorUtils.IsNotSet(minColor2, maxColor2))
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
                    if (isOverlapped) _overlapInfos[index].SetOverlapStatus(i, true);
                }
            }
        }

        private bool IsBetween(Byte val, Byte min, Byte max)
        {
            return min < max ? (val >= min) && (val <= max) : (val <= min) && (val >= max);
        }

        private class OverlapInfo
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
            if (HighlightColorsChanged != null)
            {
                HighlightColorsChanged(this, new EventArgs());
            }
        }

        private void setHighlightColors()
        {
            bool isOn = _highlightColorCmd.HasExecuted();
            _highlightColorLow = isOn ? getHightlightColor(Settings.Default.MinColors[_selectedColorIndex]) : Color.Empty;
            _highlightColorHigh = isOn ? getHightlightColor(Settings.Default.MaxColors[_selectedColorIndex]) : Color.Empty;
            raiseHighlightColorChanged();
        }

        private Color getHightlightColor(Color color)
        {
            if (ColorUtils.IsNotSet(color))
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

        private void ColorForm_MouseLeave(object sender, EventArgs e)
        {
            ColorFunctionTemp = ColorFunction.NotSet;
        }

        private void ColorForm_KeyUpDown(object sender, KeyEventArgs e)
        {
            ColorFunctionTemp = ColorUtils.GetColorFunction(ModifierKeys);            
        }

        #region StripCommand Handling

        //TODO: refactor out as this is the same as for mainform
        private void setupItem(ToolStripItem tsItem, string caption, string tip, Image image, StripCommand command)
        {
            tsItem.Text = caption;
            tsItem.Image = image;
            tsItem.Tag = command;
            tsItem.ToolTipText = tip;
            tsItem.Click += ToolItem_Click;
        }

        private void ToolItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item == null) throw new ApplicationException(string.Format("sender is not a toolstrip: {0}", sender));

            StripCommand cmd = item.Tag as StripCommand;
            if (cmd == null) throw new ApplicationException(string.Format("toolstrip has not command: {0}", item.Text));

            if (cmd.CanExecute()) cmd.Execute();

            updateAllEnablement();
        }
        #endregion

        public class ClearColorsStripCommand : StripCommand
        {
            private ColorForm _form;
            private bool _isAll;


            public ClearColorsStripCommand(bool isAll, ColorForm form)
            {
                _form = form;
                _isAll = isAll;
            }

            public override bool CanExecute()
            {
                return true;
            }

            public override bool Execute()
            {
                if (_isAll)
                {
                    _form.clearAllColors();
                }
                else
                {
                    _form.clearColor();
                }
                return true;
            }

            public override bool HasExecuted()
            {
                return false;
            }
        }

        public class HighlightColorStripCommand : StripCommand
        {
            private ColorForm _form;
            private bool _isHighlighting;

            public HighlightColorStripCommand(bool isHightlighting, ColorForm form)
            {
                _form = form;
                _isHighlighting = isHightlighting;
            }

            public override bool CanExecute()
            {
                return true;
            }

            public override bool Execute()
            {
                _isHighlighting = !_isHighlighting;
                _form.setHighlightColors();
                return true;
            }

            public override bool HasExecuted()
            {
                return _isHighlighting;
            }
        }

        public class ToggleShowHideStripCommand : StripCommand
        {
            private ColorForm _form;

            public ToggleShowHideStripCommand( ColorForm form )
            {
                _form = form;
            }

            public override bool CanExecute()
            {
                return true;
            }

            public override bool Execute()
            {
                _form.toggleDetailVisible();
                return true;
            }

            public override bool HasExecuted()
            {
                return false;
            }
        }

        public class SetColorFunctionModeStripCommand : StripCommand
        {
            private ColorForm _form;
            private ColorFunction _colorFunction;

            public SetColorFunctionModeStripCommand( ColorForm form, ColorFunction colorFunction )
            {
                _form = form;
                _colorFunction = colorFunction;
            }

            public override bool CanExecute()
            {
                return true;
            }

            public override bool Execute()
            {
                _form.ColorFunctionMode = _colorFunction;
                return true;
            }

            public override bool HasExecuted()
            {
                //we want to show the combined colorfunction temp & mode
                return _form.GetColorFunction() == _colorFunction;
            }
        }       
    }

    public enum ColorFunction
    {
        SetColor,
        AddToColor,
        RemoveFromColor,

        NotSet
    }
}