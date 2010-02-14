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
using NXTCamView.Commands;
using NXTCamView.Comms;
using NXTCamView.Forms;

namespace NXTCamView.StripCommands
{
    public class OpenOptionsStripCommand : StripCommand
    {
        private readonly ICommsPort _commsPort;
        private readonly ICommsPortFactory _commsPortFactory;

        public OpenOptionsStripCommand(IAppState appState, ICommsPort commsPort, ICommsPortFactory commsPortFactory ) : base(appState)
        {
            _commsPort = commsPort;
            _commsPortFactory = commsPortFactory;
        }

        public override bool CanExecute()
        {
            //don't all options to be changed if tracking or serial comms are outstanding
            return _appState.State == State.Connected || _appState.State == State.NotConnected;
        }

        public override bool Execute()
        {
            var form = new OptionsForm( _appState, _commsPort, _commsPortFactory );
            form.ShowDialog();
            return true;
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}
