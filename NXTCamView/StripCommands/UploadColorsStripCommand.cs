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
using NXTCamView.Properties;

namespace NXTCamView.StripCommands
{
    public class UploadColorsStripCommand : StripCommand
    {
        private ColorForm _form;
        private ISerialProvider _serialProvider;

        public UploadColorsStripCommand(ColorForm form, ISerialProvider serialProvider)
        {
            _serialProvider = serialProvider;
            _form = form;
        }

        public override bool CanExecute()
        {
            return AppState.Inst.State == State.Connected && !_form.IsAnyOverlapped();
        }

        public override bool Execute()
        {
            int red = 0;
            int green = 16;
            int blue = 32;
            try
            {
                int TOTAL_COLORS = ColorForm.TRACKED_COLORS - 1;
                //prepare the color map
                byte[] colorMap = new byte[3*16];
                for( int objectNum = 0; objectNum < ColorForm.TRACKED_COLORS; objectNum++ )
                {
                    Color minColor = Settings.Default.MinColors[objectNum];
                    Color maxColor = Settings.Default.MaxColors[objectNum];
                    if( ColorUtils.IsNotSet(minColor, maxColor) ) continue; //skip as black == not set
                    for( int offset = 0; offset < 16; offset++ )
                    {
                        //this is weird, but the gets us to the layout we need
                        //div by 17 to get it back to 0-15
                        byte redMask =
                            (byte)((offset >= minColor.R / 17 && offset <= maxColor.R / 17) ? (1 << (TOTAL_COLORS-objectNum)) : 0);
                        colorMap[red + offset] |= redMask;

                        byte greenMask =
                            (byte)((offset >= minColor.G / 17 && offset <= maxColor.G / 17) ? (1 << (TOTAL_COLORS - objectNum)) : 0);
                        colorMap[green + offset] |= greenMask;

                        byte blueMask =
                            (byte)((offset >= minColor.B / 17 && offset <= maxColor.B / 17) ? (1 << (TOTAL_COLORS - objectNum)) : 0);
                        colorMap[blue + offset] |= blueMask;
                    }
                }

                dumpColorMap( colorMap );

                SetColorMapCommand cmd = new SetColorMapCommand( _serialProvider );
                cmd.ColorMap = new List< byte >(colorMap);
                cmd.Execute();
                if( cmd.IsSuccessful )
                {
                    Settings.Default.UploadedMinColors = (Color[]) Settings.Default.MinColors.Clone();
                    Settings.Default.UploadedMaxColors = (Color[]) Settings.Default.MaxColors.Clone();
                    Settings.Default.LastColorUpload = DateTime.Now;
                    TrackingForm.Instance.SetupColors();

                    MessageBox.Show(_form, "Color upload was successful!", Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show(_form, cmd.ErrorDescription, Application.ProductName, MessageBoxButtons.OK,
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

        private static void dumpColorMap( byte[] colorMap )
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