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

namespace NXTCamView.StripCommands
{
    public class StripCommand
    {
        public event EventHandler< EventArgs > Completed;

        /// <summary>
        /// Called to determine if an action can be taken
        /// Used to set the enablement state of buttons
        /// </summary>
        /// <returns></returns>
        public virtual bool CanExecute()
        {
            return false;
        }

        /// <summary>
        /// Called to cause an action to happen
        /// Returns true if the aciton was completes successfully
        /// </summary>
        /// <returns></returns>
        public virtual bool Execute()
        {
            return true;
        }

        public virtual void OnCompeted()
        {
            if( Completed != null )
            {
                Completed(this, new EventArgs());
            }
        }

        /// <summary>
        /// Only true for command that can toggle between latched on/latched off
        /// </summary>
        public virtual bool HasExecuted()
        {
            return false;
        }

        protected void setState(State state)
        {
            AppState.Instance.State = state;
        }
    }
}
