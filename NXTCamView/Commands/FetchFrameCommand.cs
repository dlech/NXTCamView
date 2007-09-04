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
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;

namespace NXTCamView.Commands
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

        protected FetchFrameCommand(string name, SerialPort serialPort, BackgroundWorker worker) : base(name,serialPort)
        {
            _worker = worker;
        }

        public const int IMAGE_WIDTH = 176;
        public const int IMAGE_HEIGHT = 144;
        public const int PIXELS_PER_PACKET = IMAGE_WIDTH;
        public const int PACKET_COUNT = PACKETS_IN_DUMP;
        protected const int BYTES_PER_PACKET = 3 + PIXELS_PER_PACKET;
        protected const int PACKETS_IN_DUMP = IMAGE_HEIGHT / 2;
        private Dictionary<int, byte[]> bytesByLine = new Dictionary<int, byte[]>();
        protected BackgroundWorker _worker;
        protected Bitmap _bmBayer = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT);
        protected Bitmap _bmInterpolated  = new Bitmap(IMAGE_WIDTH,IMAGE_HEIGHT);
        public Bitmap Interpolated { get { return _bmInterpolated; } }

        protected void setAborted()
        {
            _isSuccessful = false;
            _aborted = true;
            _errorDescription = "Aborted";
            return;
        }

        public LinePair getLinePair(byte[] buffer)
        {
            //176*144
            //88 * 72 packet with 2 lines in each
            if( buffer.Length != BYTES_PER_PACKET ) throw new ApplicationException("Bad byte count");
            if( buffer[0] != 0x0B ) throw new ApplicationException("Bad start byte");
            if( buffer[BYTES_PER_PACKET - 1] != 0x0F ) throw new ApplicationException("Bad end byte");
            int line = buffer[1];
            if( line < 0 || line > 144 ) throw new ApplicationException(string.Format("Bad line number {0}", line));
            byte[] pixelData = new byte[PIXELS_PER_PACKET];
            Array.Copy(buffer, 2, pixelData, 0, pixelData.Length);
            bytesByLine.Add(line, buffer);
            int x = 0;
            int y = line*2;
            LinePair linePair = new LinePair(y);
            //update the 2 lines now
            for( int index = 0; index < PIXELS_PER_PACKET; index++ )
            {
                if( index%2 == 0 )
                {
                    Color blue = Color.FromArgb(0x0, 0x0, (pixelData[index] & 0x0F) * 16);
                    linePair.Colors[x, 1] = blue;
                    _bmBayer.SetPixel(x, y + 1, blue);
                    Color green = Color.FromArgb(0x0, (pixelData[index] >> 4) * 16, 0x0);
                    linePair.Colors[x + 1, 1] = green;
                    _bmBayer.SetPixel(x + 1, y + 1, green);
                }
                else
                {
                    Color green = Color.FromArgb(0x0, (pixelData[index] & 0x0F) * 16, 0x0);
                    linePair.Colors[x, 0] = green;
                    _bmBayer.SetPixel(x, y, green);
                    Color red = Color.FromArgb((pixelData[index] >> 4) * 16, 0x0, 0x0);
                    linePair.Colors[x + 1, 0] = red;
                    _bmBayer.SetPixel(x + 1, y, red);
                    x+=2;
                }
            }
            return linePair;
        }

        public Bitmap createBitmap(int width, int height, byte[] data)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
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

        protected Color getInterpolatedColor(int x, int y)
        {
            PixelState pixelState = (PixelState)(((y%2)<<1) | x%2);
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
            if (x<0 || x>=IMAGE_WIDTH || y<0 || y >=IMAGE_HEIGHT)
            {
                return Color.Black;
            }
            goodColors++;
            return _bmBayer.GetPixel(x, y);
        }

        protected static string DumpBytes(byte[] buffer)
        {
            char[] hexMap = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            StringBuilder sb = new StringBuilder(buffer.Length * 3);
            foreach (byte b in buffer)
            {
                sb.Append(hexMap[b >> 4]);
                sb.Append(hexMap[b & 0x0f]);
                sb.Append(' ');
            }
            return sb.ToString();
        }

        protected void updateInterpolateImage()
        {
            byte[] colorData = new byte[IMAGE_HEIGHT*IMAGE_WIDTH*3];
            int index = 0;
            //just do them all! (inefficient, but easy for now)
            for (int y = 0; y < IMAGE_HEIGHT; y++)
            {
                for( int x = 0; x < IMAGE_WIDTH; x++ )
                {
                    Color color = getInterpolatedColor(x, y);
                    colorData[index] = color.B;
                    colorData[index+1] = color.G;
                    colorData[index + 2] = color.R;
                    index += 3;
                }
                if (((100*(y+1)/IMAGE_HEIGHT) % 5) == 0) _worker.ReportProgress(100 + (100 * y / IMAGE_HEIGHT));
                if (_worker.CancellationPending)
                {
                    setAborted();
                    return;
                }
            }
            //Create the bitmap
            _bmInterpolated = createBitmap(IMAGE_WIDTH, IMAGE_HEIGHT, colorData);
        }
    }

    public class LinePair
    {
        public readonly int Y;
        //Two lines worth of color
        public Color[,] Colors = new Color[FetchFrameCommand.IMAGE_WIDTH, 2];

        public LinePair(int y)
        {
            Y = y;
        }
    }
}