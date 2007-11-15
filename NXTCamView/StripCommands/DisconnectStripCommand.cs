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
using System.Threading;
using System.Windows.Forms;
using NXTCamView.Commands;

namespace NXTCamView.StripCommands
{
    public class DisconnectStripCommand : StripCommand
    {
        private ISerialProvider _serialProvider;

        public DisconnectStripCommand( ISerialProvider serialProvider )
        {
            _serialProvider = serialProvider;
        }

        public override bool CanExecute()
        {
            return AppState.Inst.State != State.NotConnected;
        }

        public override bool Execute()
        {
            bool wasBusy = false;
            foreach( Form form in MainForm.Instance.MdiChildren )
            {
                ITaskRunner taskRunner = form as ITaskRunner;
                if( taskRunner != null )
                {
                    //if we're someone is busy (like capturing) we will ask them to abort,
                    //then disconnect once the abort is complete
                    if (taskRunner.IsBusy())
                    {
                        taskRunner.AbortCompleted += delegate { disconnect( ); };
                        taskRunner.Abort( );
                        wasBusy = true;
                    }
                }
            }
            //if nothing was busy, then we can just "disconnect" now
            if( !wasBusy )
            {
                disconnect( );
            }
            return true;
        }

        private void disconnect( )
        {
            //this is not so pretty, but we want to clear away any junk
            //that would cause a connect to fail later
            do
            {
                _serialProvider.ReadExisting( );
                Thread.Sleep( 100 );
            } while( _serialProvider.BytesToRead > 0 );

            AppState.Inst.State = State.NotConnected;
            OnCompeted();
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}
