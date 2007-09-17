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
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace NXTCamView.Resources
{
    static public class AppImages
    {
        private static AppImageReader _imageReader = new AppImageReader();

        public static readonly Image MainForm = _imageReader.GetImage("Capture.bmp");
        public static readonly Image Connect = _imageReader.GetImage("Connect.bmp");
        public static readonly Image Disconnect = _imageReader.GetImage("Disconnect.bmp");
        public static readonly Image NotConnected = _imageReader.GetImage("NotConnected.bmp");
        public static readonly Image Connected = _imageReader.GetImage("Connected.bmp");
        public static readonly Image ConnectedTracking = _imageReader.GetImage("ConnectedTracking.bmp");
        public static readonly Image ConnectedBusy = _imageReader.GetImage("ConnectedBusy.bmp");
        public static readonly Image Error = _imageReader.GetImage("Error.bmp");
        public static readonly Image Capture = _imageReader.GetImage("Capture.bmp");
        public static readonly Image OpenFile = _imageReader.GetImage("OpenFile.bmp");
        public static readonly Image SaveFile = _imageReader.GetImage("SaveFile.bmp");
        
        public static readonly Image Options = _imageReader.GetImage("Options.bmp");
        public static readonly Image Colors = _imageReader.GetImage("Colors.bmp");
        public static readonly Image Tracking = _imageReader.GetImage("Tracking.bmp");

        public static readonly Image UploadColors = _imageReader.GetImage("UploadColors.bmp");
        public static readonly Image ClearColor = _imageReader.GetImage("ClearColor.bmp");
        public static readonly Image ClearAllColors = _imageReader.GetImage("ClearAllColors.bmp");
        public static readonly Image HighlightColor = _imageReader.GetImage("HighlightColor.bmp");

        public static readonly Image SetColor = _imageReader.GetImage("SetColor.bmp");
        public static readonly Image AddToColor = _imageReader.GetImage("AddToColor.bmp");
        public static readonly Image RemoveFromColor = _imageReader.GetImage("RemoveFromColor.bmp");

        public static Icon GetIcon( Image image )
        {
            Icon icon = Icon.FromHandle(((Bitmap)image).GetHicon());
            return icon;
        }

        private class AppImageReader
        {
            public Image GetImage(string name)
            {
                string fullName = string.Format("NXTCamView.Resources.{0}", name);
                using( Stream stream = GetType().Assembly.GetManifestResourceStream(fullName) )
                {
                    if( stream == null )
                    {
                        //don't throw expection in static initializer
                        Debug.WriteLine(string.Format("ERROR - image not found:{0}",fullName));
                        return new Bitmap(16, 16);
                    }
                    Image image = Image.FromStream(stream);
                    ((Bitmap) image).MakeTransparent(((Bitmap) image).GetPixel(0, 0));
                    return image;
                }
            }
        }
    }    
}
