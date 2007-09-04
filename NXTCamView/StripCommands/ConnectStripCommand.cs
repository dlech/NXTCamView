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
using NXTCamView.Commands;
using NXTCamView.Properties;

namespace NXTCamView.StripCommands
{
    public class ConnectStripCommand : StripCommand
    {
        private string _version;

        public string GetVersion()
        {
            return _version;
        }

        public override bool CanExecute()
        {
            return AppState.Instance.State == State.NotConnected;            
        }

        public override bool Execute()
        {
            bool isOk = false;
            try
            {
                setState(State.ConnectedBusy);
                PingCommand pingCmd = new PingCommand(MainForm.Instance.SerialPort);
                pingCmd.Execute();
                isOk = pingCmd.IsSuccessful;
                if( isOk )
                {
                    GetVersionCommand versionCmd = new GetVersionCommand(MainForm.Instance.SerialPort);
                    versionCmd.Execute();
                    isOk = versionCmd.IsSuccessful;
                    _version = isOk ? versionCmd.Version : "";
                }
            }
            finally
            {
                setState(isOk ? State.Connected : State.NotConnected);
                OnCompeted();
                if( !isOk )
                {
                    if (MessageBox.Show(MainForm.Instance, 
                        string.Format("Connection failed on port {0}.  \nWould you like to change the settings?",Settings.Default.COMPort), 
                        Application.ProductName, 
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information, 
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        //show options
                        OpenOptionsStripCommand cmd = new OpenOptionsStripCommand(MainForm.Instance.SerialPort);
                        cmd.Execute();
                    }
                }
            }
            return isOk;
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}
