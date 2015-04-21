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

#region using

using System;
using System.ComponentModel;
using System.Diagnostics;
using NXTCamView.Core.Comms;

#endregion

namespace NXTCamView.Core.Commands
{
    public class DumpFrameCommand : FetchFrameCommand
    {
        public DumpFrameCommand( IAppState appState, BackgroundWorker worker, ICommsPort commsPort )
            : base( appState, "Capture", commsPort, worker )
        {
        }

        public override void Execute()
        {
            ////DEBUG
            //_isSuccessful = true;
            //_isCompleted = true;
            //return;

            try
            {
                _request = "DF";
                SendAndReceive();
                if ( _isLogging ) Debug.WriteLine( "Capture start:" );
                for ( int count = 0; count < PacketsInDump; count++ )
                {
                    var buffer = new byte[BytesPerPacket];
                    int totalRead = 0;
                    while ( totalRead < BytesPerPacket )
                    {
                        totalRead += _commsPort.Read( buffer, totalRead, BytesPerPacket - totalRead );
                    }

                    if ( _isLogging ) Debug.WriteLine( string.Format( "Pkt:{0} :{1}", count, DumpBytes( buffer ) ) );
                    LinePair linePair = GetLinePair( buffer );
                    Worker.ReportProgress( 100*count/PacketsInDump, linePair );
                    if ( Worker.CancellationPending )
                    {
                        SetAborted();
                        return;
                    }
                }
                UpdateInterpolateImage();
            }
            catch ( Exception ex )
            {
                setError( ex );
            }
            finally
            {
                completeCommand();
            }
        }
    }
}