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

namespace NXTCamView.Core
{
    public enum State
    {
        NotConnected,
        Connected,
        ConnectedBusy,
        ConnectedTracking
    }

    public interface IAppState
    {
        event EventHandler<EventArgs> StateChanged;
        State State { get; set; }
    }

    public class AppState : IAppState
    {
        public event EventHandler<EventArgs> StateChanged;
        private State _state;

        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                if( StateChanged != null )
                {
                    StateChanged(this, new EventArgs());
                }
            }
        }
    }
}
