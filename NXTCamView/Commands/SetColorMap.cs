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
using System.Collections.Generic;
using System.Text;

namespace NXTCamView.Commands
{
    public class SetColorMapCommand : Command
    {
        public SetColorMapCommand( ISerialProvider serialProvider )
            : base("Set Colors", serialProvider )
        {
        }

        private List<byte> _colorMap;
        public List<byte> ColorMap { get { return _colorMap; } set { _colorMap = value; } }

        public override void Execute()
        {
            try
            {
                SetState(State.ConnectedBusy);

                if( _colorMap == null ) throw new ApplicationException("No colorMap to set");
                if( _colorMap.Count != 16*3 ) throw new ApplicationException(string.Format("ColorMap needs to be 16*3 not {0}", _colorMap.Count));

                StringBuilder sb = new StringBuilder();

                foreach (byte color in _colorMap)
                {
                    sb.AppendFormat("{0} ", color);
                }
                //drop the space
                sb.Remove(sb.Length - 1, 1);
                _request = "SM " + sb;
                SendAndReceive();
                _isCompleted = true;
            }
            catch (Exception ex)
            {
                setError(ex);
            }
            finally
            {
                SetState(State.Connected);
                completeCommand();
            }
        }
    }
}