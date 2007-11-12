using System.IO.Ports;

namespace NXTCamView.Commands
{
    public interface ISerialProvider
    {
        void EnsureOpen();
        void EnsureClosed( );

        void WriteLine( string request );
        string ReadLine( );
        int Read( byte[] buffer, int read, int i );
        string ReadExisting( );
        int ReadByte( );
        int BytesToRead { get; }
        event SerialDataReceivedEventHandler DataReceived;
    }
}
