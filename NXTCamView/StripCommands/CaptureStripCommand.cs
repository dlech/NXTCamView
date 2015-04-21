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
using NXTCamView.Core;
using NXTCamView.Core.Comms;
using NXTCamView.Forms;

namespace NXTCamView.StripCommands
{
    public class CaptureStripCommand : StripCommand
    {
        private readonly MainForm _mainForm;
        private readonly ColorForm _colorForm;
        private readonly ICommsPort _commsPort;

        public CaptureStripCommand( IAppState appState, MainForm mainForm, ColorForm colorForm, ICommsPort commsPort) : base(appState)
        {
            _mainForm = mainForm;
            _colorForm = colorForm;
            _commsPort = commsPort;
        }

        public override bool CanExecute()
        {
            return _appState.State == State.Connected;
        }

        public override bool Execute()
        {
            SetState(State.ConnectedBusy);
            var form = new CaptureForm( _appState, _colorForm, _commsPort ) {MdiParent = _mainForm, Visible = true};
            form.StartCapture();
            return true;
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}
