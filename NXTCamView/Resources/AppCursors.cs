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
using System.IO;
using System.Windows.Forms;

namespace NXTCamView.Resources
{
    static public class AppCursors
    {
        private static AppCursorReader _cursorReader = new AppCursorReader();

        public static readonly Cursor AddingColor = _cursorReader.GetCursor("AddingColor.cur");
        public static readonly Cursor RemovingColor = _cursorReader.GetCursor("RemovingColor.cur");

        private class AppCursorReader
        {
            public Cursor GetCursor(string name)
            {
                string fullName = string.Format("NXTCamView.Resources.{0}", name);
                using (Stream stream = GetType().Assembly.GetManifestResourceStream(fullName))
                {
                    if (stream == null)
                    {
                        //don't throw expection in static initializer
                        Debug.WriteLine(string.Format("ERROR - cursor not found:{0}", fullName));
                        return Cursors.Default;
                    }
                    Cursor cursor = new Cursor(stream);
                    return cursor;
                }
            }
        }
    }
}
