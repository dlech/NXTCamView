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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using NXTCamView.Core.Comms;

namespace NXTCamView.Core.Commands
{
    public class FetchFrameCommand : Command
    {
        /* ********************************************************
         * The bayerImage is in the bayer format shown below.
         *
         *      |   |   |   |   |   |   |   |   |   | . | 1 | 1 |
         *      |   |   |   |   |   |   |   |   |   | . | 7 | 7 |
         *      | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | . | 4 | 5 |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *    0 | G | R | G | R | G | R | G | R | G | . | G | R |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *    1 | B | G | B | G | B | G | B | G | B | . | B | G |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *    2 | G | R | G | R | G | R | G | R | G | . | G | R |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *    3 | B | G | B | G | B | G | B | G | B | . | B | G |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *    4 | G | R | G | R | G | R | G | R | G | . | G | R |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *    5 | B | G | B | G | B | G | B | G | B | . | B | G |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *    .   .   .   .   .   .   .   .   .   .   .   .   .
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *  142 | G | R | G | R | G | R | G | R | G | . | G | R |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *  143 | B | G | B | G | B | G | B | G | B | . | B | G |
         * -----+---+---+---+---+---+---+---+---+---+ . +---+---+
         *
         * The corners are calculated, then the edges, then the center.
         *
         */

        protected FetchFrameCommand(IAppState appState, string name, ICommsPort commsPort, BackgroundWorker worker) : base( appState, name, commsPort)
        {
            Worker = worker;
        }

        public const int ImageWidth = 176;
        public const int ImageHeight = 144;
        public const int PixelsPerPacket = ImageWidth;
        public const int PacketCount = PacketsInDump;
        protected const int BytesPerPacket = 3 + PixelsPerPacket;
        protected const int PacketsInDump = ImageHeight / 2;
        private readonly Dictionary<int, byte[]> _bytesByLine = new Dictionary<int, byte[]>();
        protected BackgroundWorker Worker;
        protected Bitmap BmBayer = new Bitmap(ImageWidth, ImageHeight);
        protected Bitmap BmInterpolated  = new Bitmap(ImageWidth,ImageHeight);
        public Bitmap Interpolated { get { return BmInterpolated; } }

        protected void SetAborted()
        {
            _isSuccessful = false;
            _aborted = true;
            _errorDescription = "Aborted";
            return;
        }

        public LinePair GetLinePair(byte[] buffer)
        {
            //176*144
            //88 * 72 packet with 2 lines in each
            if( buffer.Length != BytesPerPacket ) throw new ApplicationException("Bad byte count");
            if( buffer[0] != 0x0B ) throw new ApplicationException("Bad start byte");
            if( buffer[BytesPerPacket - 1] != 0x0F ) throw new ApplicationException("Bad end byte");
            int line = buffer[1];
            if( line < 0 || line > 144 ) throw new ApplicationException(string.Format("Bad line number {0}", line));
            var pixelData = new byte[PixelsPerPacket];
            Array.Copy(buffer, 2, pixelData, 0, pixelData.Length);
            _bytesByLine.Add(line, buffer);
            int x = 0;
            int y = line*2;
            LinePair linePair = new LinePair(y);
            //update the 2 lines now
            for( int index = 0; index < PixelsPerPacket; index++ )
            {
                if( index%2 == 0 )
                {
                    Color blue = Color.FromArgb(0x0, 0x0, (pixelData[index] & 0x0F) * 16);
                    linePair.Colors[x, 1] = blue;
                    BmBayer.SetPixel(x, y + 1, blue);
                    Color green = Color.FromArgb(0x0, (pixelData[index] >> 4) * 16, 0x0);
                    linePair.Colors[x + 1, 1] = green;
                    BmBayer.SetPixel(x + 1, y + 1, green);
                }
                else
                {
                    Color green = Color.FromArgb(0x0, (pixelData[index] & 0x0F) * 16, 0x0);
                    linePair.Colors[x, 0] = green;
                    BmBayer.SetPixel(x, y, green);
                    Color red = Color.FromArgb((pixelData[index] >> 4) * 16, 0x0, 0x0);
                    linePair.Colors[x + 1, 0] = red;
                    BmBayer.SetPixel(x + 1, y, red);
                    x+=2;
                }
            }
            return linePair;
        }

        public Bitmap CreateBitmap(int width, int height, byte[] data)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bitmap.LockBits(
                                 new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                 ImageLockMode.WriteOnly, bitmap.PixelFormat);
            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }


        internal enum PixelState
        {
            GreenEven = 0x0,
            Red = 0x1,
            Blue = 0x2,   
            GreenOdd = 0x3
        }

        protected Color GetInterpolatedColor(int x, int y)
        {
            var pixelState = (PixelState)(((y%2)<<1) | x%2);
            int red;
            int green;
            int blue;
            int gCount = 0;
            int rCount = 0;
            int bCount = 0;
            switch(pixelState)
            {
                case PixelState.Red:
                    red =
                        getColor(x, y, ref rCount).R;
                    green = 
                        getColor(x,y-1,ref gCount).G +
                        getColor(x,y+1,ref gCount).G +
                        getColor(x+1,y,ref gCount).G +
                        getColor(x+1,y,ref gCount).G;                
                    blue = 
                        getColor(x-1, y-1, ref bCount).B + 
                        getColor(x-1, y+1, ref bCount).B +
                        getColor(x+1, y-1, ref bCount).B + 
                        getColor(x+1, y+1, ref bCount).B;
                    break;
                case PixelState.GreenEven:
                    red = 
                        getColor(x-1, y, ref rCount).R + 
                        getColor(x+1, y, ref rCount).R;
                    green =
                        getColor(x, y, ref gCount).G;
                    blue = 
                        getColor(x, y-1, ref bCount).B + 
                        getColor(x, y+1, ref bCount).B;
                    break;
                case PixelState.Blue:
                    red =
                        getColor(x-1, y-1, ref rCount).R + 
                        getColor(x-1, y+1, ref rCount).R +
                        getColor(x+1, y-1, ref rCount).R + 
                        getColor(x+1, y+1, ref rCount).R;
                    green = 
                        getColor(x,y-1,ref gCount).G +
                        getColor(x,y+1,ref gCount).G +
                        getColor(x+1,y,ref gCount).G +
                        getColor(x-1,y,ref gCount).G;
                    blue =
                        getColor(x, y, ref bCount).B; 
                    break;            
                case PixelState.GreenOdd:
                    red = 
                        getColor(x, y-1, ref rCount).R + 
                        getColor(x, y+1, ref rCount).R;
                    green =
                        getColor(x, y, ref gCount).G;
                    blue = 
                        getColor(x-1, y, ref bCount).B + 
                        getColor(x+1, y, ref bCount).B;
                    break;
                default:
                    throw new Exception("bad state");
            }
            return Color.FromArgb(red/rCount,green/gCount,blue/bCount);
        }

        private Color getColor(int x, int y, ref int goodColors)
        {
            //Only get colors for valid pixels
            if (x<0 || x>=ImageWidth || y<0 || y >=ImageHeight)
            {
                return Color.Black;
            }
            goodColors++;
            return BmBayer.GetPixel(x, y);
        }

        protected static string DumpBytes(byte[] buffer)
        {
            char[] hexMap = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            var sb = new StringBuilder(buffer.Length * 3);
            foreach (byte b in buffer)
            {
                sb.Append(hexMap[b >> 4]);
                sb.Append(hexMap[b & 0x0f]);
                sb.Append(' ');
            }
            return sb.ToString();
        }

        protected void UpdateInterpolateImage()
        {
            byte[] colorData = new byte[ImageHeight*ImageWidth*3];
            int index = 0;
            //just do them all! (inefficient, but easy for now)
            for (int y = 0; y < ImageHeight; y++)
            {
                for( int x = 0; x < ImageWidth; x++ )
                {
                    Color color = GetInterpolatedColor(x, y);
                    colorData[index] = color.B;
                    colorData[index+1] = color.G;
                    colorData[index + 2] = color.R;
                    index += 3;
                }
                if (((100*(y+1)/ImageHeight) % 5) == 0) Worker.ReportProgress(100 + (100 * y / ImageHeight));
                if (Worker.CancellationPending)
                {
                    SetAborted();
                    return;
                }
            }
            //Create the bitmap
            BmInterpolated = CreateBitmap(ImageWidth, ImageHeight, colorData);
        }
    }

    public class LinePair
    {
        public readonly int Y;
        //Two lines worth of color
        public Color[,] Colors = new Color[FetchFrameCommand.ImageWidth, 2];

        public LinePair(int y)
        {
            Y = y;
        }
    }
}