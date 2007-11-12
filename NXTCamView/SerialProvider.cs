using System;
using System.Diagnostics;
using System.IO.Ports;
using NXTCamView.Commands;
using NXTCamView.Properties;

namespace NXTCamView
{
    class SerialProvider : ISerialProvider
    {
        public static ISerialProvider _instance = null;
        public static ISerialProvider Instance
        {
            get
            {
                if( _instance == null )
                {
                    _instance = new SerialProvider( );
                }
                return _instance;
            }
        }

        private SerialPort _serialPort;
        private bool _canReset;

        public static ISerialProvider CreateTestProvider(string COMPort, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake)
        {
            SerialPort serialPort = createSerialPort( COMPort, baudRate, parity, dataBits, stopBits, handshake );
            return new SerialProvider( serialPort );
        }

        private static SerialPort createSerialPortFromConfig(Settings settings)
        {
            return createSerialPort(
                settings.COMPort,
                settings.BaudRate,
                settings.Parity,
                settings.DataBits,
                settings.StopBits,                
                settings.Handshake);
        }

        private static SerialPort createSerialPort( string COMPort, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake )
        {
            SerialPort serialPort = new SerialPort( COMPort, baudRate, parity, dataBits, stopBits );
            serialPort.Handshake = handshake;
            serialPort.WriteTimeout = 2000;
            serialPort.ReadTimeout = 2000;
            serialPort.NewLine = "\r";
            return serialPort;
        }

        public SerialProvider()
        {
            //Normal port used which updates with config changes
            _serialPort = createSerialPortFromConfig( Settings.Default );
            Settings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(settings_PropertyChanged);
            _serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
            _canReset = true;
        }

        private SerialProvider( SerialPort serialPort )
        {
            //Explicit port used for testing
            _serialPort = serialPort;
            EnsureOpen( );
            _canReset = false;
        }

        void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Debug.WriteLine( string.Format( "Error recieved on COMPort - EventType: {0}", e.EventType ));
        }

        public void EnsureOpen( )
        {
            if( _serialPort == null )
            {
                _serialPort = createSerialPortFromConfig( Settings.Default );
            }
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Blowup trying to open COMPort. Will try again: " + ex);
                //The port may have been removed and added back in
                //lets copy it, dispose the old one and try again
                SerialPort newSerialPort =
                    createSerialPort( 
                        _serialPort.PortName, 
                        _serialPort.BaudRate, 
                        _serialPort.Parity,
                        _serialPort.DataBits, 
                        _serialPort.StopBits, 
                        _serialPort.Handshake );
                EnsureClosed( );
                _serialPort = newSerialPort;
                _serialPort.Open( );
            }
        }

        public void EnsureClosed()
        {
            try
            {
                if( _serialPort == null ) return;
                if( _serialPort.IsOpen )
                {
                    _serialPort.Close( );
                }
            }
            catch( Exception ex )
            {
                Debug.WriteLine( "Blowup trying to close COMPort: " + ex );
            }
            _serialPort.Dispose();
            //make sure we blow up!
            _serialPort = null;
        }


        private void settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Settings settings = (Settings)sender;
            if (settings != Settings.Default) throw new ApplicationException("Only allow changing of Settings.Default");
            try
            {
                switch (e.PropertyName)
                {
                    case "COMPort":
                    case "BaudRate":
                    case "Parity":
                    case "Handshake":
                    case "DataBits":
                    case "StopBits":
                        //drop the old port and get another
                        EnsureClosed();
                        _serialPort = createSerialPortFromConfig( settings );
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error changing propertied {0}", ex));
            }
        }

        public void WriteLine(string request)
        {
            try
            {
                _serialPort.WriteLine(request);
            }
            catch
            {
                EnsureClosed();
                EnsureOpen();
                _serialPort.WriteLine(request);
            }
        }

        public string ReadLine()
        {
            try
            {
                return _serialPort.ReadLine();
            }
            catch
            {
                resetPort( );
                return _serialPort.ReadLine();
            }
        }

        private void resetPort( )
        {
            if (_canReset)
            {
                EnsureClosed( );
                EnsureOpen( );
            }
        }

        public int Read(byte[] buffer, int i, int remaining)
        {
            try
            {
                return _serialPort.Read( buffer, i, remaining );
            }
            catch
            {
                resetPort(); 
                return _serialPort.Read(buffer, i, remaining);
            }
        }

        public string ReadExisting( )
        {
            try
            {
                return _serialPort.ReadExisting();
            }
            catch
            {
                resetPort(); 
                return _serialPort.ReadExisting();
            }            
        }

        public int ReadByte( )
        {
            try
            {
                return _serialPort.ReadByte();
            }
            catch
            {
                resetPort(); 
                return _serialPort.ReadByte();
            }            
        }

        public int BytesToRead
        {
            get { return _serialPort.BytesToRead; }
        }

        public event SerialDataReceivedEventHandler DataReceived
        {
            add
            {
                _serialPort.DataReceived += value;
            }
            remove
            {
                _serialPort.DataReceived -= value;
            }
        }
    }
}
