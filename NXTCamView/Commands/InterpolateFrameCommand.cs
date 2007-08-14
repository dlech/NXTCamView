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
using System.IO.Ports;
using NXTCamView.Commands;

public class InterpolateFrameCommand : FetchFrameCommand
{
    public InterpolateFrameCommand(BackgroundWorker worker, SerialPort serialPort, Image image)
        : base("Interpolate",serialPort, worker)
    {
        _bmBayer = (Bitmap)image;
    }

    public override void Execute()
    {
        ////DEBUG
        //_isSucessiful = true;
        //_isCompleted = true;
        //return;

        try
        {
            updateInterpolateImage();
            _isSucessiful = true;
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
}
