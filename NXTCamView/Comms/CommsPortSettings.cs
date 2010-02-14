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
    public class CommsPortSettings
    {
        private readonly string _comPort;
        private readonly int _baudRate;
        private readonly Parity _parity;
        private readonly int _dataBits;
        private readonly StopBits _stopBits;
        private readonly Handshake _handshake;

        public CommsPortSettings( string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits,
                                  Handshake handshake )
        {
            _comPort = comPort;
            _baudRate = baudRate;
            _parity = parity;
            _dataBits = dataBits;
            _stopBits = stopBits;
            _handshake = handshake;
        }

        public Handshake Handshake
        {
            get { return _handshake; }
        }

        public StopBits StopBits
        {
            get { return _stopBits; }
        }

        public int DataBits
        {
            get { return _dataBits; }
        }

        public Parity Parity
        {
            get { return _parity; }
        }

        public int BaudRate
        {
            get { return _baudRate; }
        }

        public string ComPort
        {
            get { return _comPort; }
        }
    }
}