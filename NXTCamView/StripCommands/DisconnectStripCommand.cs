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
namespace NXTCamView.StripCommands
{
    public class DisconnectStripCommand : StripCommand
    {
        public override bool CanExecute()
        {
            return AppState.Instance.State != State.NotConnected;
        }

        public override bool Execute()
        {
            //maybe need to consider tracking?
            AppState.Instance.State = State.NotConnected;
            OnCompeted();
            return true;
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}
