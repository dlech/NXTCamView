//
//  PreferencesDialog.cs
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
using System.IO.Ports;
using Gtk;
using NXTCamView.Core;
using NXTCamView.Core.Comms;

namespace NXTCamView.Gtk.Windows
{
    public partial class PreferencesDialog : Dialog
    {
        static readonly int[] baudRates = { 19200, 38400, 115200, 230400, 460800 };
        static readonly int[] dataBits = { 8, 7 };

        readonly ISettings settings;

        public PreferencesDialog (ISettings settings)
        {
            this.settings = settings;

            this.Build ();

            int i = 0;
            foreach (var port in SerialPort.GetPortNames()) {
                portNameCombobox.AppendText (port);
                if (port == settings.COMPort) {
                    portNameCombobox.Active = i;
                }
                i++;
            }
            i = 0;
            foreach (var baud in baudRates) {
                baudRateCombobox.AppendText (baud.ToString ());
                if (baud == settings.BaudRate) {
                    baudRateCombobox.Active = i;
                }
                i++;
            }
            i = 0;
            foreach (var size in dataBits) {
                dataBitsCombobox.AppendText (size.ToString ());
                if (size == settings.DataBits) {
                    dataBitsCombobox.Active = i;
                }
                i++;
            }
            i = 0;
            foreach (Parity parity in Enum.GetValues(typeof(Parity))) {
                parityCombobox.AppendText (parity.ToString ());
                if (parity == settings.Parity) {
                    parityCombobox.Active = i;
                }
                i++;
            }
            i = 0;
            foreach (StopBits stopBits in Enum.GetValues(typeof(StopBits))) {
                stopBitsCombobox.AppendText (stopBits.ToString ());
                if (stopBits == settings.StopBits) {
                    stopBitsCombobox.Active = i;
                }
                i++;
            }
            i = 0;
            foreach (Handshake handshake in Enum.GetValues(typeof(Handshake))) {
                handshakeCombobox.AppendText (handshake.ToString ());
                if (handshake ==  settings.Handshake) {
                    handshakeCombobox.Active = i;
                }
                i++;
            }
        }

        protected void OnButtonOkClicked (object sender, EventArgs e)
        {
            settings.COMPort = portNameCombobox.ActiveText;
            settings.BaudRate = int.Parse (baudRateCombobox.ActiveText);
            settings.DataBits = int.Parse (dataBitsCombobox.ActiveText);
            settings.Parity = (Parity)parityCombobox.Active;
            settings.StopBits = (StopBits)stopBitsCombobox.Active;
            settings.Handshake = (Handshake)handshakeCombobox.Active;
            Destroy ();
        }

        protected void OnButtonCancelClicked (object sender, EventArgs e)
        {
            Destroy ();
        }
    }
}

