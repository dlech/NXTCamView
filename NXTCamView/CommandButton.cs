using System;
using System.Windows.Forms;
using NXTCamView.StripCommands;

namespace NXTCamView
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
