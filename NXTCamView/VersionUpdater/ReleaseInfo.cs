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
using System.Text.RegularExpressions;

namespace NXTCamView.VersionUpdater
{
    public class ReleaseInfo
    {
        public readonly string Title = "";
        public readonly string Link = "";
        public readonly string Description = "";
        public readonly string Author = "";
        public readonly string Comment = "";
        public readonly string PubDate = "";
        private DateTime _releaseDate = DateTime.MinValue;

        public ReleaseInfo(string title, string link, string description, string author, string comment, string pubDate)
        {
            Title = title;
            Link = link;
            Description = description;
            Author = author;
            Comment = comment;
            PubDate = pubDate;
        }

        public Version Version
        {
            get 
            {
                Regex regex = new Regex(@"NXTCamView\sinstall\s(\d\.\d\.\d)\sreleased", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match match = regex.Match( Title );
                //capture[0] is in the form "1.2.3"
                if (match.Groups.Count == 2) return new Version(match.Groups[1].Value);
                return null;
            }
        }

        public DateTime ReleaseDate 
        { 
            get 
            {
                if( _releaseDate == DateTime.MinValue )
                {
                    try
                    {
                        _releaseDate = DateTime.Parse(PubDate);
                    }
                    catch
                    {
                        _releaseDate = DateTime.MinValue;
                    }
                }
                return _releaseDate;
            } 
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Title: {0}", Title);
            sb.AppendFormat("Link: {0}", Link);
            sb.AppendFormat("Description: {0}", Description);
            sb.AppendFormat("Author: {0}", Author);
            sb.AppendFormat("Comment: {0}", Comment);
            return sb.ToString();
        }
    }

    public class ReleaseInfoComparer : IComparer< ReleaseInfo >
    {
        public int Compare(ReleaseInfo x, ReleaseInfo y)
        {
            //compare by version first, then release date
            int diff = y.Version.CompareTo( x.Version );
            if( diff != 0 ) return diff;
            return y.ReleaseDate.CompareTo( x.ReleaseDate );
        }
    }
}
