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
using System.Windows.Forms;
using NXTCamView.StripCommands;

namespace NXTCamView.Controls
{
    public enum ExecutionType
    {
        OnClickExecute,
        OnDownUpToggle
    }

    public class CommandButton : Button
    {
        private StripCommand _command;
        private ExecutionType _type = ExecutionType.OnClickExecute;

        public event EventHandler<EventArgs> Completed;
        public StripCommand Command 
        {
            get { return _command; } 
            set
            {
                _command = value;
                if (_command != null)
                {
                    Enabled = _command.CanExecute();
                }
            } 
        }
        public ExecutionType ExecutionType { get { return _type; } set { _type = value; } }

        public CommandButton( )
        {
        }

        public CommandButton( StripCommand command )
        {
            _command = command;            
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (_type == ExecutionType.OnClickExecute) doExecute();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (_type == ExecutionType.OnDownUpToggle) doExecute();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (_type == ExecutionType.OnDownUpToggle) doExecute();
        }

        private void doExecute()
        {
            if (_command == null) throw new ApplicationException("No command attached to command button");
            if (!_command.CanExecute()) throw new ApplicationException("Command button should not be enabled");
           
            //do it!
            _command.Execute();

            UpdateEnablement();

            OnCompleted();
        }

        public void UpdateEnablement()
        {
            if (_command == null) throw new ApplicationException("No command attached to command button");
            Enabled = _command.CanExecute();
        }

        /// <summary>
        /// Use this to determine the latch state & enablement when the command completes
        /// </summary>
        protected virtual void OnCompleted()
        {
            if (Completed != null) Completed(this, new EventArgs());
        }
    }
}


