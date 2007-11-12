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

namespace NXTCamView.Commands
{
    public abstract class Command
    {
        protected ISerialProvider _serialProvider;
        protected bool _isCompleted;
        protected bool _isSuccessful;
        protected string _request;
        protected bool _isLogging;
        protected string _errorDescription;
        protected bool _aborted = false;
        protected string _name;

        public string Request { get { return _request; } set { _request = value; } }
        public bool IsCompleted { get { return _isCompleted; } set { _isCompleted = value; } }
        public bool IsSuccessful { get { return _isSuccessful; } }
        public ISerialProvider SerialProvider { get { return _serialProvider; } }
        public string ErrorDescription { get { return _errorDescription; } }
        public bool Aborted { get { return _aborted; } }

        protected Command( string name, ISerialProvider serialProvider )
            : this(name, serialProvider, true)
        {
        }

        protected Command(string name, ISerialProvider serialProvider, bool isLogging)
        {
            _name = name;
            _serialProvider = serialProvider;
            _isLogging = isLogging;
        }

        public virtual void Execute()
        {
        }

        public void SendAndReceive()
        {

            _isCompleted = false;
            _isSuccessful = false;

            //Remove as this may be hamper tracking
            //string junk = _serialPort.ReadExisting();
            //if (junk != "") Debug.WriteLine(string.Format("Discarded serial junk: {0}", junk));

            if (_isLogging) Debug.WriteLine(string.Format("snd: {0}", _request));

            _serialProvider.WriteLine(_request);
            string responce = _serialProvider.ReadLine();

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
            return AppState.Inst.State == State.Connected;
        }

        protected void SetState(State state)
        {
            AppState.Inst.State = state;
        }
    }

    public delegate void CommandComplete();
}