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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NXTCamView.Commands;
using NXTCamView.Comms;
using NXTCamView.Forms;
using NXTCamView.Properties;

namespace NXTCamView.StripCommands
{
    public class UploadColorsStripCommand : StripCommand
    {
        private readonly ColorForm _colorForm;
        private readonly IColorTarget _colorTarget;
        private readonly ICommsPort _commsPort;

        public UploadColorsStripCommand(IAppState appState, ColorForm colorForm, IColorTarget colorTarget, ICommsPort commsPort)
            : base(appState)
        {
            _commsPort = commsPort;
            _colorForm = colorForm;
            _colorTarget = colorTarget;
        }

        public override bool CanExecute()
        {
            return _appState.State == State.Connected && 
                !_colorForm.IsAnyOverlapped();
        }

        public override bool Execute()
        {
            const int red = 0;
            const int green = 16;
            const int blue = 32;
            try
            {
                const int totalColors = ColorForm.TrackedColors - 1;
                //prepare the color map
                var colorMap = new byte[3*16];
                for( int objectNum = 0; objectNum < ColorForm.TrackedColors; objectNum++ )
                {
                    Color minColor = Settings.Default.MinColors[objectNum];
                    Color maxColor = Settings.Default.MaxColors[objectNum];
                    if( ColorUtils.IsNotSet(minColor, maxColor) ) continue; //skip as black == not set
                    for( int offset = 0; offset < 16; offset++ )
                    {
                        //this is weird, but the gets us to the layout we need
                        //div by 17 to get it back to 0-15
                        var redMask =
                            (byte)((offset >= minColor.R / 17 && offset <= maxColor.R / 17) ? (1 << (totalColors-objectNum)) : 0);
                        colorMap[red + offset] |= redMask;

                        var greenMask =
                            (byte)((offset >= minColor.G / 17 && offset <= maxColor.G / 17) ? (1 << (totalColors - objectNum)) : 0);
                        colorMap[green + offset] |= greenMask;

                        var blueMask =
                            (byte)((offset >= minColor.B / 17 && offset <= maxColor.B / 17) ? (1 << (totalColors - objectNum)) : 0);
                        colorMap[blue + offset] |= blueMask;
                    }
                }

                DumpColorMap( colorMap );

                var cmd = new SetColorMapCommand( _appState, _commsPort ) {ColorMap = new List<byte>(colorMap)};
                cmd.Execute();
                if( cmd.IsSuccessful )
                {
                    Settings.Default.UploadedMinColors = (Color[]) Settings.Default.MinColors.Clone();
                    Settings.Default.UploadedMaxColors = (Color[]) Settings.Default.MaxColors.Clone();
                    Settings.Default.LastColorUpload = DateTime.Now;
                    _colorTarget.SetupColors();

                    MessageBox.Show(_colorForm, "Color upload was successful!", Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show(_colorForm, cmd.ErrorDescription, Application.ProductName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch( Exception ex )
            {
                Debug.WriteLine(string.Format("Error uploading colors {0}", ex));
                return false;
            }
            return true;
        }

        private static void DumpColorMap( byte[] colorMap )
        {
            Debug.Write("colorMap: ");
            for( int i = 0; i < 16*3; i++ )
            {
                if( i%16 == 0 ) Debug.Write("- ");
                Debug.Write(String.Format("{0:x} ", colorMap[i]));
            }
            Debug.WriteLine("");
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}