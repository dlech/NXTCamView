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
using System.IO.Ports;

namespace NXTCamView.StripCommands
{
    public class OpenOptionsStripCommand : StripCommand
    {
        private SerialPort _serialPort;

        public OpenOptionsStripCommand(SerialPort serialPort)
        {
            _serialPort = serialPort;
        }

        public override bool CanExecute()
        {
            //don't all options to be changed if tracking or serial comms are outstanding
            return AppState.Instance.State == State.Connected || AppState.Instance.State == State.NotConnected;
        }

        public override bool Execute()
        {
            OptionsForm form = new OptionsForm(_serialPort);
            form.ShowDialog();
            return true;
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}
