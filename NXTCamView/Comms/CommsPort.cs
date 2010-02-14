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
using System.Diagnostics;
using System.IO.Ports;

namespace NXTCamView.Comms
{
    public class CommsPort : ICommsPort
    {
        protected SerialPort SerialPort;
        private readonly ITracer _tracer;
        protected CommsPortSettings Settings;

        public CommsPort( CommsPortSettings settings, ITracer tracer )
        {
            Settings = settings;
            SerialPort = CreateSerialPort( Settings );
            _tracer = tracer;
        }

        private static SerialPort CreateSerialPort( CommsPortSettings settings )
        {
            var port = new SerialPort( settings.ComPort, settings.BaudRate, settings.Parity, settings.DataBits, settings.StopBits );
            port.Handshake = settings.Handshake;
            port.WriteTimeout = 2000;
            port.ReadTimeout = 2000;
            port.NewLine = "\r";
            port.ErrorReceived += SerialPortErrorReceived;
            return port;
        }

        private static void SerialPortErrorReceived( object sender, SerialErrorReceivedEventArgs e )
        {
            Debug.WriteLine( string.Format( "Error recieved on COMPort - EventType: {0}", e.EventType ) );
        }


        public void WriteLine( string request )
        {
            try
            {
                tryWriteLine( request );
            }
            catch
            {
                EnsureClosed();
                EnsureOpen();
                tryWriteLine(request);
            }
        }

        private void tryWriteLine( string request )
        {
            _tracer.TraceWriteLine( request );
            SerialPort.WriteLine( request );
        }

        public string ReadLine()
        {
            try
            {
                string line = tryReadLine();
                return line;
            }
            catch
            {
                resetPort();
                string line = tryReadLine();
                return line;
            }
        }

        private string tryReadLine()
        {
            string line = SerialPort.ReadLine();
            _tracer.TraceReadLine( line );
            return line;
        }

        private void resetPort()
        {
            EnsureClosed();
            EnsureOpen();
        }

        public int Read( byte[] buffer, int i, int remaining )
        {
            try
            {
                int read = tryRead( buffer, i, remaining );
                return read;
            }
            catch
            {
                resetPort();
                int read = tryRead(buffer, i, remaining);
                return read;
            }
        }

        private int tryRead( byte[] buffer, int i, int remaining )
        {
            int read = SerialPort.Read( buffer, i, remaining );
            var data = new byte[read];
            Array.Copy( buffer, i, data, 0, read );
            _tracer.TraceRead( data );
            return read;
        }

        public string ReadExisting()
        {
            try
            {
                string data = tryReadExisting();
                return data;
            }
            catch
            {
                resetPort();
                string data = tryReadExisting();
                return data;

            }
        }

        private string tryReadExisting()
        {
            string data = SerialPort.ReadExisting();
            _tracer.TraceReadExisting( data );
            return data;
        }

        public int ReadByte()
        {
            try
            {
                var b = SerialPort.ReadByte();
                Tracer.TraceReadBtye( b );
                return b;
            }
            catch
            {
                resetPort();
                return SerialPort.ReadByte();
            }
        }

        public int BytesToRead
        {
            get { return SerialPort.BytesToRead; }
        }

        public event SerialDataReceivedEventHandler DataReceived
        {
            add { SerialPort.DataReceived += value; }
            remove { SerialPort.DataReceived -= value; }
        }

        public void EnsureOpen()
        {
            if ( SerialPort == null )
            {
                SerialPort = CreateSerialPort(Settings);
            }
            try
            {
                if ( !SerialPort.IsOpen )
                {
                    SerialPort.Open();
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine( "Blowup trying to open COMPort. Will try again: " + ex );
                //The port may have been removed and added back in
                //lets copy it, dispose the old one and try again
                EnsureClosed();

                SerialPort = CreateSerialPort(Settings);
                SerialPort.Open();
            }
        }

        public void EnsureClosed()
        {
            try
            {
                if ( SerialPort == null ) return;
                if ( SerialPort.IsOpen )
                {
                    SerialPort.Close();
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine( "Blowup trying to close COMPort: " + ex );
            }
            if ( SerialPort != null )
            {
                SerialPort.Dispose();
            }
            //make sure we blow up!
            SerialPort = null;
        }
    }
}


