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

namespace NXTCamView.Comms
{
    public interface IConfigCommsPort : ICommsPort
    {
    }

    public interface ICommsPort
    {
        void WriteLine( string request );
        string ReadLine( );
        int Read( byte[] buffer, int read, int i );
        string ReadExisting( );
        int ReadByte( );
        int BytesToRead { get; }
        event SerialDataReceivedEventHandler DataReceived;
        void EnsureOpen();
        void EnsureClosed();
    }
}


