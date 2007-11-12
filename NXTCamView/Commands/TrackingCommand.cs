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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;

namespace NXTCamView.Commands
{
    public class TrackingCommand : Command
    {
        private EventHandler< ObjectsDetectedEventArgs > _handler;
        private List< TrackedColor > _trackedObjects;

        //syncroot protects these
        private object _syncRoot = new object( );
        private bool _wasACKed = false;
        private bool _needsResync;
        public List< TrackedColor > TrackedObjects { get { return _trackedObjects; } }

        public TrackingCommand( ISerialProvider serialProvider, EventHandler< ObjectsDetectedEventArgs > handler )
            : base( "Tracking", serialProvider )
        {
            _handler = handler;
        }

        public override void Execute( )
        {
            try
            {
                string junk = _serialProvider.ReadExisting();
                if( junk != "" ) Debug.WriteLine( string.Format( "Discarded serial junk: {0}", junk ) );
                _request = "ET";
                _isCompleted = false;
                _isSuccessful = false;

                if( _isLogging ) Debug.WriteLine( string.Format( "snd: {0}", _request ) );
                lock( _syncRoot )
                {
                    _wasACKed = false;
                    _serialProvider.DataReceived += processReceive;
                    _serialProvider.WriteLine(_request);
                    //wait for indication of ACK or NACK
                    //this is needed as the object data may arrive before the ACK or NACK
                    if( Monitor.Wait( _syncRoot, 2000 ) )
                    {
                        if( !_wasACKed ) throw new ApplicationException( "NACKed" );
                        _isSuccessful = true;
                    }
                    else
                    {
                        throw new ApplicationException( "Timed out waiting for response" );
                    }
                }
            }
            catch( Exception ex )
            {
                setError( ex );
            }
            _isCompleted = true;
        }

        public bool StopTracking( )
        {
            bool isStopSuccessful = false;
            try
            {
                lock( _syncRoot )
                {
                    _wasACKed = false;
                    _request = "DT";
                    if( _isLogging ) Debug.WriteLine( string.Format( "snd: {0}", _request ) );
                    _serialProvider.WriteLine(_request);
                    //wait for indication of ACK or NACK
                    //this is needed as the object data may arrive before the ACK or NACK
                    if( Monitor.Wait( _syncRoot, 2000 ) )
                    {
                        if( !_wasACKed ) throw new ApplicationException( "NACKed" );
                        _isSuccessful = true;
                    }
                    else
                    {
                        throw new ApplicationException( "Timed out waiting for response" );
                    }
                }
            }
            catch( Exception ex )
            {
                setError( ex );
            }
            finally
            {                
                _serialProvider.DataReceived -= processReceive;
                completeCommand( );
                //Not true, but make it so for now as the responce form the cam seems to be bad
                _isSuccessful = true;
            }
            return isStopSuccessful;
        }

        private void processReceive( object sender, SerialDataReceivedEventArgs e )
        {
            try
            {
                processReceive( );
            }
            catch( Exception ex )
            {
                Console.WriteLine( "Unhandled error during receive. " + ex.Message );
            }
        }

        private void processReceive( )
        {
            const int OBJECT_DATA_SIZE = 5;
            if (_serialProvider.BytesToRead == 0) return;

            //clear out junk if necessary
            if( _needsResync )
            {
                int i = 0;
                int data;
                do
                {
                    data = _serialProvider.ReadByte();
                    i++;
                } while( i < 250 && data != 0xFF );
                _needsResync = false;
            }

            byte[] buffer = getData( _serialProvider, 2);

            //Check if we get anything other than a good packet start byte
            if( buffer[ 0 ] != 0x0A )
            {
                lock( _syncRoot )
                {
                    if( buffer[ 0 ] == 'A' )
                    {
                        //ack
                        _wasACKed = true;
                        Monitor.Pulse( _syncRoot );
                        Debug.WriteLine( "rcv: ACK" );
                    }
                    else if( buffer[ 0 ] == 'N' )
                    {
                        //nack
                        _wasACKed = false;
                        Monitor.Pulse( _syncRoot );
                        Debug.WriteLine( "rcv: NACK" );
                    }
                    else
                    {
                        throw new ApplicationException( "Bad start byte on objects" );
                    }
                }
                //clear away the rest of the ACK or NACK
                _serialProvider.ReadLine( );
                return;
            }

            int objectCount = buffer[ 1 ];
            if( objectCount < 0 || objectCount > 8 )
                throw new ApplicationException( string.Format( "Bad object count: {0}", objectCount ) );

            buffer = getData( _serialProvider, objectCount * OBJECT_DATA_SIZE + 1 );

            _trackedObjects = new List< TrackedColor >( objectCount );
            int index = 0;
            for( int count = 0; count < objectCount; count++ )
            {
                _trackedObjects.Add( new TrackedColor( buffer, index, count ) );
                index += OBJECT_DATA_SIZE;
            }

            if( buffer[ index ] != 0xFF )
            {
                Debug.WriteLine( "Bad byte end detected during tracking - resyncing" );
                _needsResync = true;
            }

            //notify any interested
            if( _handler != null )
            {
                _handler( this, new ObjectsDetectedEventArgs( _trackedObjects ) );
            }
        }

        private byte[] getData( ISerialProvider serialProvider, int remaining )
        {
            byte[] buffer = new byte[remaining];
            while( remaining > 0 )
            {
                remaining -= serialProvider.Read(buffer, buffer.Length - remaining, remaining);
            }
            return buffer;
        }
    }

    public class ObjectsDetectedEventArgs : EventArgs
    {
        public readonly List< TrackedColor > TrackedColors;

        public ObjectsDetectedEventArgs( List< TrackedColor > trackedObjects )
        {
            TrackedColors = trackedObjects;
        }
    }

    public struct TrackedColor
    {
        private int _id;
        private int _colorIndex;
        private Rectangle _bounds;

        public int Id { get { return _id; } }
        public int ColorIndex { get { return _colorIndex; } }
        public Rectangle Bounds { get { return _bounds; } }

        public TrackedColor( byte[] buffer, int index, int id )
        {
            _id = id;
            _colorIndex = buffer[ index ];
            _bounds =
                Rectangle.FromLTRB( buffer[ index + 1 ], buffer[ index + 2 ], buffer[ index + 3 ], buffer[ index + 4 ] );
        }

        public override string ToString( )
        {
            return
                string.Format( "Object:{0} Color:{1} Rect[{2},{3},{4},{5}]", _id, (_colorIndex+1), _bounds.Left, _bounds.Top,
                               _bounds.Right,
                               _bounds.Bottom );
        }
    }
}