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
using System.Windows.Forms;

namespace NXTCamView.Forms
{
    public partial class CustomRegisterSettingControl : UserControl
    {
        private int _row;
        public CustomRegisterSettingControl(int row,string configSettings, EventHandler valueChangedHandler)
        {
            InitializeComponent();
            _row = row;
            SetConfigSettings(configSettings);
            cbRegisterEnabled.CheckedChanged += valueChangedHandler;
            nudReg.ValueChanged += valueChangedHandler;
            nudValue.ValueChanged += valueChangedHandler;
        }

        public void SetConfigSettings(string configSettings)
        {
            try
            {
                string[] settings = configSettings.Split(',');
                cbRegisterEnabled.Checked = bool.Parse(settings[0]);
                nudReg.Value = int.Parse(settings[1]);
                nudValue.Value = int.Parse(settings[2]);                
            }
            catch( Exception ex )
            {
                Debug.WriteLine("Error parsing custom registers settings config. " + ex);
            }
        }

        public int Row { get { return _row; } }

        public bool RegisterEnabled { get { return cbRegisterEnabled.Checked; } }
        public byte Register { get { return (byte) nudReg.Value; } }
        public byte Value { get { return (byte) nudValue.Value; } }

        public string GetConfigSetting()
        {
            return string.Format("{0},{1},{2}", cbRegisterEnabled.Checked, nudReg.Value, nudValue.Value);
        }
        
    }
}


