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
using NXTCamView.Properties;

namespace NXTCamView.Commands
{
    public enum TrackingMode
    {
        Object,
        Line
    }

    public class SetTrackingModeCommand : Command
    {
        public SetTrackingModeCommand(ISerialProvider serialProvider, TrackingMode mode )
            : base("Set Tracking Mode", serialProvider )
        {
            _mode = mode;
        }

        private TrackingMode _mode;

        public override void Execute()
        {
            try
            {
                SetState(State.ConnectedBusy);

                _request = _mode == TrackingMode.Line ? "TL" :"TO";
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

        internal static void SetModeInConfig( Settings settings, TrackingMode mode )
        {
            settings.TrackingMode = mode.ToString();
        }

        internal static TrackingMode GetModeFromConfig( Settings settings )
        {
            try
            {
                return (TrackingMode)Enum.Parse(typeof(TrackingMode), settings.TrackingMode );    
            }
            catch
            {
                //default to Object
                return TrackingMode.Object;
            }
            
        }
    }
}