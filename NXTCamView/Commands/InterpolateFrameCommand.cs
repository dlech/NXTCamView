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
using System.ComponentModel;
using System.Drawing;
using NXTCamView.Commands;
using NXTCamView.Comms;

namespace NXTCamView.Commands
{
    public class InterpolateFrameCommand : FetchFrameCommand
    {
        public InterpolateFrameCommand(IAppState appState, BackgroundWorker worker, ICommsPort commsPort, Image image)
            : base(appState, "Interpolate", commsPort, worker)
        {
            BmBayer = (Bitmap)image;
        }

        public override void Execute()
        {
            ////DEBUG
            //_isSuccessful = true;
            //_isCompleted = true;
            //return;

            try
            {
                UpdateInterpolateImage();
                _isSuccessful = true;
            }
            catch (Exception ex)
            {
                setError(ex);
            }
            finally
            {
                completeCommand();
            }
        }

        public override bool CanExecute()
        {
            return true;
        }
    }
}