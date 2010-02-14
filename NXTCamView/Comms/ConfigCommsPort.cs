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
using System.Diagnostics;
using NXTCamView.Properties;

namespace NXTCamView.Comms
{
    public class ConfigCommsPort : CommsPort, IConfigCommsPort
    {
        public ConfigCommsPort( ITracer tracer ) : base( GetSettingsFromConfig(), tracer )
        {
            Properties.Settings.Default.PropertyChanged += settingsPropertyChanged;
        }

        private void settingsPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            var settings = (Settings) sender;
            if ( settings != Properties.Settings.Default )
                throw new ApplicationException( "Only allow changing of Settings.Default" );

            try
            {
                switch ( e.PropertyName )
                {
                    case "COMPort":
                    case "BaudRate":
                    case "Parity":
                    case "Handshake":
                    case "DataBits":
                    case "StopBits":
                        Settings = GetSettingsFromConfig();
                        //only open if we were already open
                        if( SerialPort != null && 
                            SerialPort.IsOpen )
                        {
                            EnsureClosed();
                            EnsureOpen();
                        }
                        return;
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine( string.Format( "Error changing propertied {0}", ex ) );
            }
        }

        private static CommsPortSettings GetSettingsFromConfig()
        {
            Settings settings = Properties.Settings.Default;
            return new CommsPortSettings( settings.COMPort, settings.BaudRate, settings.Parity, settings.DataBits,
                                          settings.StopBits, settings.Handshake );
        }
    }
}