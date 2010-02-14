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
using System.Windows.Forms;

namespace NXTCamView.Controls
{
    //thank you reflector
    class TransparentPictureBox : PictureBox
    {
        private Color _transarentColorLow = Color.Empty;
        private Color _transarentColorHigh = Color.Empty;
        private ImageAttributes _imageAttributes = new ImageAttributes();

        [Category("Custom")]
        [DefaultValue(0)]
        public Color TransarentColorLow
        {
            get { return _transarentColorLow; }
            set
            {
                _transarentColorLow = value;
                setColorKeys();
            }
        }

        [Category("Custom")]
        [DefaultValue(0)]
        public Color TransarentColorHigh
        {
            get { return _transarentColorHigh; } 
            set
            {
                _transarentColorHigh = value;
                setColorKeys();
            }
        }

        private Brush _highlightBrush = Brushes.Yellow;
        private Color _highlightColor = Color.Yellow;
        [Category("Custom")]
        [DefaultValue("Color.Yellow")]
        public Color HighlightColor
        {
            get { return _highlightColor; }
            set
            {
                _highlightColor = value;
                if( _highlightBrush != null ) _highlightBrush.Dispose();
                _highlightBrush = _highlightColor == Color.Empty ? null : new SolidBrush(_highlightColor);
                Refresh();
            }
        }

        private void setColorKeys()
        {
            if (_transarentColorLow != Color.Empty &&
                _transarentColorHigh != Color.Empty &&
                _transarentColorLow.R <= _transarentColorHigh.R &&
                _transarentColorLow.G <= _transarentColorHigh.G &&
                _transarentColorLow.B <= _transarentColorHigh.B &&
                _transarentColorLow.A <= _transarentColorHigh.A)
            {
                _imageAttributes.SetColorKey(_transarentColorLow, _transarentColorHigh, ColorAdjustType.Default);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Image image = Image;
            if (image != null)
            {
                Rectangle rect = ImageRectangleFromSizeMode();
                if ( _highlightBrush == null || _transarentColorLow == Color.Empty || _transarentColorHigh == Color.Empty)
                {
                    pe.Graphics.DrawImage(image, rect);                    
                }
                else
                {
                    //draw a rectange with transperancy
                    pe.Graphics.FillRectangle(_highlightBrush,rect);                    
                    pe.Graphics.DrawImage(image, rect, 0,0, image.Width, image.Height, GraphicsUnit.Pixel, _imageAttributes);                    
                }
            }
        }

        public Rectangle ImageRectangleFromSizeMode()
        {
            return ImageRectangleFromSizeMode(SizeMode);
        }

        public Rectangle ImageRectangleFromSizeMode(PictureBoxSizeMode mode)
        {
            Image image = Image;
            Rectangle rectangle = DeflateRect(ClientRectangle, Padding);
            if (Image != null)
            {
                switch (mode)
                {
                    case PictureBoxSizeMode.Normal:
                    case PictureBoxSizeMode.AutoSize:
                        rectangle.Size = Image.Size;
                        return rectangle;

                    case PictureBoxSizeMode.StretchImage:
                        return rectangle;

                    case PictureBoxSizeMode.CenterImage:
                        rectangle.X += (rectangle.Width - image.Width) / 2;
                        rectangle.Y += (rectangle.Height - image.Height) / 2;
                        rectangle.Size = image.Size;
                        return rectangle;

                    case PictureBoxSizeMode.Zoom:
                        {
                            Size size = image.Size;
                            float num = Math.Min((((float)ClientRectangle.Width) / ((float)size.Width)), (((float)ClientRectangle.Height) / ((float)size.Height)));
                            rectangle.Width = (int)(size.Width * num);
                            rectangle.Height = (int)(size.Height * num);
                            rectangle.X = (ClientRectangle.Width - rectangle.Width) / 2;
                            rectangle.Y = (ClientRectangle.Height - rectangle.Height) / 2;
                            return rectangle;
                        }
                }
            }
            return rectangle;
        }

        public static Rectangle DeflateRect(Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }
    }
}


