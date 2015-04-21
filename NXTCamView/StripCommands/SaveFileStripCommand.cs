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
using System.Windows.Forms;
using NXTCamView.Core;
using NXTCamView.Core.Comms;
using NXTCamView.Forms;

namespace NXTCamView.StripCommands
{
    public class OpenFileStripCommand : StripCommand
    {
        private readonly MainForm _mainForm;
        private readonly OpenFileDialog _openFileDialog;
        private readonly ICommsPort _commsPort;
        private readonly ColorForm _colorForm;

        public OpenFileStripCommand(IAppState appState, MainForm mainForm, ColorForm colorForm, OpenFileDialog openFileDialog, ICommsPort commsPort) : base(appState)
        {
            _mainForm = mainForm;
            _colorForm = colorForm;
            _commsPort = commsPort;
            _openFileDialog = openFileDialog;
        }

        public override bool CanExecute()
        {
            return true;
        }

        public override bool Execute()
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var form = new CaptureForm( _appState, _colorForm, _commsPort ) {MdiParent = _mainForm};
                form.Show();
                form.LoadFile(_openFileDialog.FileName);
                return true;
            }
            return false;
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}