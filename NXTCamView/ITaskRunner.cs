using System;

namespace NXTCamView
{
    interface ITaskRunner
    {
        bool IsBusy( );
        void Abort( );
        event EventHandler< EventArgs > AbortCompleted;
    }
}
