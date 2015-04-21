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
using System.Diagnostics;
using System.Text;

namespace NXTCamView.Core
{
    public interface ITracer
    {
        void TraceWriteLine( string line );
        void TraceReadExisting( string data );
        void TraceRead( byte[] data );
        void TraceReadLine( string line );
    }

    public class Tracer : ITracer
    {
        public void TraceWriteLine( string line )
        {
            Debug.WriteLine( "Writing: "+line );
        }

        public void TraceReadExisting( string data )
        {
            Debug.WriteLine("Reading: " + data );
        }

        public void TraceRead( byte[] data )
        {
            string myString = Encoding.ASCII.GetString(data);
            Debug.WriteLine("Reading: " + myString);
        }

        public void TraceReadLine( string line )
        {
            Debug.WriteLine("Reading: " + line );
        }

        public static void TraceReadBtye( int i )
        {
            Debug.WriteLine("Reading: " + i );
        }
    }
}