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
using System.IO.Ports;
using NXTCamView;
using NXTCamView.Commands;

public class DumpFrameCommand : FetchFrameCommand
{
    public DumpFrameCommand( BackgroundWorker worker, SerialPort serialPort )
        : base("Capture", serialPort, worker)
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
            if( _isLogging ) Debug.WriteLine("Capture start:");
            for( int count = 0; count < PACKETS_IN_DUMP; count++ )
            {
                byte[] buffer = new byte[BYTES_PER_PACKET];
                int totalRead = 0;
                while( totalRead < BYTES_PER_PACKET )
                {
                    totalRead += _serialPort.Read(buffer, totalRead, BYTES_PER_PACKET - totalRead);
                }

                if( _isLogging ) Debug.WriteLine(string.Format("Pkt:{0} :{1}", count, DumpBytes(buffer)));
                LinePair linePair = getLinePair(buffer);
                _worker.ReportProgress(100*count/PACKETS_IN_DUMP, linePair);
                if (_worker.CancellationPending)
                {
                    setAborted();
                    return;
                }
            }
            updateInterpolateImage();
        }
        catch (Exception ex)
        {
            setError(ex);
        }
        finally
        {            
            completeCommand();
        }
    }
}
