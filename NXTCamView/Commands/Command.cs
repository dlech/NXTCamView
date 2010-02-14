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
using NXTCamView.Comms;

namespace NXTCamView.Commands
{
    public abstract class Command
    {
        private readonly IAppState _appState;
        protected readonly ICommsPort _commsPort;
        protected bool _isCompleted;
        protected bool _isSuccessful;
        protected string _request;
        protected readonly bool _isLogging;
        protected string _errorDescription;
        protected bool _aborted;
        private readonly string _name;

        public bool IsCompleted { get { return _isCompleted; } }
        public bool IsSuccessful { get { return _isSuccessful; } }
        public string ErrorDescription { get { return _errorDescription; } }
        public bool Aborted { get { return _aborted; } }

        protected Command( IAppState appState, string name, ICommsPort commsPort )
            : this(appState,name, commsPort, true)
        {
        }

        internal Command(IAppState appState, string name, ICommsPort commsPort, bool isLogging)
        {
            _appState = appState;
            _name = name;
            _commsPort = commsPort;
            _isLogging = isLogging;
        }

        public virtual void Execute()
        {
        }

        protected void SendAndReceive()
        {

            _isCompleted = false;
            _isSuccessful = false;

            //Remove as this may be hamper tracking
            //string junk = _commsPort.ReadExisting();
            //if (junk != "") Debug.WriteLine(string.Format("Discarded serial junk: {0}", junk));

            if (_isLogging) Debug.WriteLine(string.Format("snd: {0}", _request));

            _commsPort.WriteLine(_request);
            string responce = _commsPort.ReadLine();

            if (_isLogging) Debug.WriteLine(string.Format("rcv: {0}", responce));
            _isSuccessful = responce == "ACK";
        }

        protected void setError(Exception ex)
        {
            _isSuccessful = false;
            string msg = string.Format("{0} Error: {1}", _name, ex.Message);
            Debug.WriteLine(msg);
            _errorDescription = msg;
        }

        protected void completeCommand()
        {
            _isCompleted = true;
        }

        public virtual bool CanExecute()
        {
            return _appState.State == State.Connected;
        }

        protected void SetState(State state)
        {
            _appState.State = state;
        }
    }

    public delegate void CommandComplete();
}