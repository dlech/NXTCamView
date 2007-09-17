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
using System.Drawing;
using System.Windows.Forms;

namespace NXTCamView
{
    static class ColorUtils
    {
        public static bool IsNotSet(Color color)
        {
            return IsNotSet(color, color);
        }

        public static bool IsNotSet(Color minColor, Color maxColor)
        {
            return (minColor.ToArgb() == Color.Black.ToArgb() && maxColor.ToArgb() == Color.Black.ToArgb()) ||
                   (minColor.ToArgb() == 0 && maxColor.ToArgb() == 0);
        }

        public static Color GetAverage(Color min, Color max)
        {
            return Color.FromArgb((min.R + max.R) / 2,
                (min.G + max.G) / 2,
                (min.B + max.B) / 2);
        }

        public static ColorFunction GetColorFunction(Keys keys)
        {
            ColorFunction function = ColorFunction.NotSet;            
            if (keys == Keys.Control)
            {
                function = ColorFunction.AddToColor;
            }
            else if (keys == (Keys.Shift | Keys.Control))
            {
                function = ColorFunction.RemoveFromColor;
            }
            return function;
        }
    }
}
