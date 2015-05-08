//
//  Settings.cs
//
//  Author:
//       David Lechner <david@lechnology.com>
//
//  Copyright (c) 2015 David Lechner
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using NXTCamView.Core;

namespace NXTCamView.Gtk
{
    public class Settings : ISettings
    {
        GLib.Settings gsettings;

        public Settings ()
        {
#if DEBUG
            var xdg_data_dirs = Environment.GetEnvironmentVariable ("XDG_DATA_DIRS");
            var localSchemasDir = System.IO.Path.GetFullPath ("../../");
            System.Diagnostics.Debug.Assert (System.IO.Directory.Exists (
                System.IO.Path.Combine(localSchemasDir, "glib-2.0/schemas")));
            xdg_data_dirs = localSchemasDir + ":" + xdg_data_dirs;
            Environment.SetEnvironmentVariable ("XDG_DATA_DIRS", xdg_data_dirs);
#endif
            gsettings = new GLib.Settings ("org.ev3dev.NxtCamView");
        }

        #region INotifyPropertyChanged implementation

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region ISettings implementation

        public string TrackingMode {
            get {
                return gsettings.GetString ("tracking-mode");
            }
            set {
                gsettings.SetString ("tracking-mode", value);
            }
        }

        public string COMPort {
            get {
                return gsettings.GetString ("serial-port");
            }
            set {
                gsettings.SetString ("serial-port", value);
            }
        }

        public int BaudRate {
            get {
                return gsettings.GetInt ("baud-rate");
            }
            set {
                gsettings.SetInt ("baud-rate", value);
            }
        }

        public System.IO.Ports.Parity Parity {
            get {
                return (System.IO.Ports.Parity)gsettings.GetEnum ("parity");
            }
            set {
                gsettings.SetEnum ("parity", (int)value);
            }
        }

        public int DataBits {
            get {
                return gsettings.GetInt ("data-bits");
            }
            set {
                gsettings.SetInt ("data-bits", value);
            }
        }

        public System.IO.Ports.StopBits StopBits {
            get {
                return (System.IO.Ports.StopBits)gsettings.GetEnum ("stop-bits");
            }
            set {
                gsettings.SetEnum ("stop-bits", (int)value);
            }
        }

        public System.IO.Ports.Handshake Handshake {
            get {
                return (System.IO.Ports.Handshake)gsettings.GetEnum ("handshake");
            }
            set {
                gsettings.SetEnum ("handshake", (int)value);
            }
        }

        #endregion
    }
}
