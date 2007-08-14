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
using System.Configuration;
using System.Drawing;

namespace NXTCamView.Properties 
{
    internal sealed partial class Settings 
    {
        private void forceRead()
        {
            string dummy = ColorsString;
        }

        private Color[] _minColors = new Color[8];
        public Color[] MinColors { get
        {
            forceRead();
            return _minColors;
        } set { _minColors = value; } }

        private Color[] _maxColors = new Color[8];
        public Color[] MaxColors { 
        get
        {
            forceRead();
            return _maxColors;
        } 
            set { _maxColors = value; } 
        }

        private Color[] _uploadedMinColors = new Color[8];
        public Color[] UploadedMinColors
        {
            get
            {
                forceRead();
                return _uploadedMinColors;
            }
            set { _uploadedMinColors = value; }
        }

        private Color[] _uploadedMaxColors = new Color[8];
        public Color[] UploadedMaxColors
        {
            get
            {
                forceRead();
                return _uploadedMaxColors;
            }
            set { _uploadedMaxColors = value; }
        }

        public Color GetAverageColor(int index)
        {
            return getAverageColor(MaxColors[index], MinColors[index]);
        }

        public Color GetAverageUploadedColor(int index)
        {
            return getAverageColor(UploadedMaxColors[index], UploadedMinColors[index]);
        }

        private static Color getAverageColor(Color maxColor, Color minColor)
        {
            return Color.FromArgb((minColor.R + maxColor.R) / 2,
                                  (minColor.G + maxColor.G) / 2,
                                  (minColor.B + maxColor.B) / 2);
        }

        public Settings() 
        {
            SettingsSaving += settingsSavingEventHandler;
            SettingsLoaded +=new SettingsLoadedEventHandler(settingsLoadedEventHandler);
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0")]
        private string ColorsString { get { return (string)this["Colors"]; } set { this["Colors"] = value; } }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0")]
        private string UploadedColorsString { get { return (string)this["UploadedColors"]; } set { this["UploadedColors"] = value; } }

        private void settingsSavingEventHandler(object sender, CancelEventArgs e)
        {
            List<string> list = new List<string>(_minColors.Length*2);
            foreach (Color color in _minColors) list.Add(color.ToArgb().ToString());
            foreach (Color color in _maxColors) list.Add(color.ToArgb().ToString());
            ColorsString = String.Join(",", list.ToArray());

            list = new List<string>(_uploadedMinColors.Length * 2);
            foreach (Color color in _uploadedMinColors) list.Add(color.ToArgb().ToString());
            foreach (Color color in _uploadedMaxColors) list.Add(color.ToArgb().ToString());
            UploadedColorsString = String.Join(",", list.ToArray());
        }

        private void settingsLoadedEventHandler(object sender, SettingsLoadedEventArgs e)
        {
            string[] colorInts = ColorsString.Split(',');
            for( int i=0; i<colorInts.Length; i++ )
            {
                if( i < colorInts.Length/2 )
                {
                    _minColors[i] = Color.FromArgb(Int32.Parse(colorInts[i]));
                }
                else
                {
                    _maxColors[i - _minColors.Length] = Color.FromArgb(Int32.Parse(colorInts[i]));
                }
            }

            for (int i = 0; i < colorInts.Length; i++)
            {
                if (i < colorInts.Length / 2)
                {
                    _uploadedMinColors[i] = Color.FromArgb(Int32.Parse(colorInts[i]));
                }
                else
                {
                    _uploadedMaxColors[i - _uploadedMinColors.Length] = Color.FromArgb(Int32.Parse(colorInts[i]));
                }
            }
        }
    }
}