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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NXTCamView
{
    public partial class ColorDetail : UserControl
    {
        public event EventHandler ColorChanged;
        private Color _minColor = Color.Blue;
        private Color _maxColor = Color.Blue;
        private bool _isNotSet;
        private ColorFunction _colorFunction;

        public ColorDetail()
        {
            InitializeComponent();
        }

        public Color MinColor
        {
            get { return _minColor; }
        }
        
        public Color MaxColor
        {
            get { return _maxColor; } 
        }

        public void SetColorMinMax(Color minColor, Color maxColor)
        {
            _minColor = minColor;
            _maxColor = maxColor;
            applyChanges();
        }

        private void notifyChanges()
        {
            if( ColorChanged != null )
            {
                ColorChanged(this, new EventArgs());
            }
        }

        public Color BaseColor
        {
            get
            {
                //return the average color
                return ColorUtils.GetAverage(_minColor,_maxColor);
            }
            set
            {
                int scaledRange = (int) (16*nudRange.Value/2);
                Color baseColor = value;

                _minColor = Color.FromArgb(
                    Math.Max(0, baseColor.R - scaledRange),
                    Math.Max(0, baseColor.G - scaledRange),
                    Math.Max(0, baseColor.B - scaledRange));

                _maxColor = Color.FromArgb(
                    Math.Min(255, baseColor.R + scaledRange),
                    Math.Min(255, baseColor.G + scaledRange),
                    Math.Min(255, baseColor.B + scaledRange));
                applyChanges();
                notifyChanges();
            }
        }

        private void applyChanges()
        {
            rbRed.SetRangeMinMax(_minColor.R/16, _maxColor.R/16);
            rbGreen.SetRangeMinMax(_minColor.G/16, _maxColor.G/16);
            rbBlue.SetRangeMinMax(_minColor.B/16, _maxColor.B/16);

            Debug.WriteLine(string.Format("R:{0}-{1},G:{2}-{3},B:{4}-{5}",
                                          rbRed.RangeMinimum,
                                          rbRed.RangeMaximum,
                                          rbGreen.RangeMinimum,
                                          rbGreen.RangeMaximum,
                                          rbBlue.RangeMinimum,
                                          rbBlue.RangeMaximum)); 
            Debug.WriteLine(string.Format("ColorMin:{0},ColorMax{1}",_minColor,_maxColor));
            Color color = Color.FromArgb(_maxColor.R - _minColor.R, _maxColor.G - _minColor.G, _maxColor.B - _minColor.B);
            Debug.WriteLine(string.Format("ColorDif:{0}", color));

            updateAndNotify();
        }


        private void panel_Paint(object sender, PaintEventArgs e)
        {
            if( _isNotSet )
            {
                return;
            }
            int colorCount = getColorCount();
            int pixelsAcross = (int) Math.Ceiling(Math.Sqrt(colorCount));            

            int pixelSideLen = Math.Min(pnlMatchingColors.Width/pixelsAcross,pnlMatchingColors.Height/pixelsAcross);

            Debug.WriteLine("colorCount: " + colorCount);
            Debug.WriteLine("pixelsAcross: " + pixelsAcross);
            Debug.WriteLine("pixelSideLen: " + pixelSideLen);
            
            Graphics gr = e.Graphics;
            int x = 0;
            for( int r = 0; r <= 15; r++ )
            {
                for( int g = 0; g <= 15; g++ )
                {
                    for (int b = 0; b <= 15; b++)
                    {
                        if (r >= rbRed.RangeMinimum && r <= rbRed.RangeMaximum &&
                            g >= rbGreen.RangeMinimum && g <= rbGreen.RangeMaximum &&
                            b >= rbBlue.RangeMinimum && b <= rbBlue.RangeMaximum)
                        {
                            //0-255 = 0-15 * 17
                            using (SolidBrush sb = new SolidBrush(Color.FromArgb(r * 17, g * 17, b * 17)))
                            {
                                gr.FillRectangle(sb, (x % (pixelsAcross)) * pixelSideLen, (x / (pixelsAcross)) * pixelSideLen, pixelSideLen, pixelSideLen);
                            }
                            x++;
                        }
                    }
                }
            }
        }

        private int getColorCount()
        {
            return Math.Max(1 + rbRed.RangeMaximum - rbRed.RangeMinimum, 1) * 
                   Math.Max(1 + rbGreen.RangeMaximum - rbGreen.RangeMinimum, 1) * 
                   Math.Max(1 + rbBlue.RangeMaximum - rbBlue.RangeMinimum, 1);
        }

        private void updateAndNotify()
        {
            _isNotSet = ColorUtils.IsNotSet(_minColor, _maxColor);
            lblNoSet.Visible = _isNotSet;
            lbMatches.Text = string.Format("Colors in range: {0}",getColorCount());
            pnlMatchingColors.Refresh();
            notifyChanges();
        }

        private void ColorDetail_Load(object sender, EventArgs e)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            updateAndNotify();
        }

        private void RangeChanging(object sender, EventArgs e)
        {
            //0-255 = 0-15 * 17
            _minColor = Color.FromArgb(rbRed.RangeMinimum * 17, rbGreen.RangeMinimum * 17, rbBlue.RangeMinimum * 17);
            _maxColor = Color.FromArgb(rbRed.RangeMaximum * 17, rbGreen.RangeMaximum * 17, rbBlue.RangeMaximum * 17);
            updateAndNotify();
        }

        private void nudRange_ValueChanged(object sender, EventArgs e)
        {
            //recreate the mix/max given the new range
            BaseColor = ColorUtils.GetAverage(_minColor, _maxColor);
        }

        public void SetActiveColor(Color color)
        {
            bool isActive = color != Color.Empty;
            //0-15
            rbRed.ValueActive = isActive;
            rbGreen.ValueActive = isActive;
            rbBlue.ValueActive = isActive;
            rbRed.Value = color.R/16;
            rbGreen.Value = color.G/16;
            rbBlue.Value = color.B/16;
        }

        public void SetColorFunction(ColorFunction function)
        {
            _colorFunction = function;
            rbRed.ColorFunction = function;
            rbGreen.ColorFunction = function;
            rbBlue.ColorFunction = function;
        }

        public void ApplyModifiedRange()
        {
            if (_colorFunction == ColorFunction.AddToColor)
            {
                _minColor = Color.FromArgb(
                    (int) Math.Max(0, Math.Min(rbRed.RangeMinimum, rbRed.Value))*17,
                    (int)Math.Max(0, Math.Min(rbGreen.RangeMinimum, rbGreen.Value)) * 17,
                    (int)Math.Max(0, Math.Min(rbBlue.RangeMinimum, rbBlue.Value)) * 17);

                _maxColor = Color.FromArgb(
                    (int)Math.Min(255, Math.Max(rbRed.RangeMaximum, rbRed.Value)) * 17,
                    (int)Math.Min(255, Math.Max(rbGreen.RangeMaximum, rbGreen.Value)) * 17,
                    (int)Math.Min(255, Math.Max(rbBlue.RangeMaximum, rbBlue.Value)) * 17);
            } 
            else if (_colorFunction == ColorFunction.RemoveFromColor)
            {
                _minColor = Color.FromArgb(
                    getRemoved(rbRed, _minColor.R/17, true)*17,
                    getRemoved(rbGreen, _minColor.G/17, true)*17,
                    getRemoved(rbBlue, _minColor.B/17, true)*17);

                _maxColor = Color.FromArgb(
                    getRemoved(rbRed, _maxColor.R / 17, false) * 17,
                    getRemoved(rbGreen, _maxColor.G / 17, false) * 17,
                    getRemoved(rbBlue, _maxColor.B / 17, false) * 17);
            }
            applyChanges();
            notifyChanges();
        }

        private int getRemoved(RangeBar bar, int current, bool isMin)
        {
            if( bar.Value < bar.RangeMinimum || bar.Value > bar.RangeMaximum) return current;
            if( bar.RangeMaximum - bar.Value > bar.Value - bar.RangeMinimum)
            {
                return (int)(isMin ? bar.Value : bar.RangeMaximum); 
            }
            else 
            {
                return (int)(isMin ? bar.RangeMinimum : bar.Value); 
            }
        }
        
    }
}