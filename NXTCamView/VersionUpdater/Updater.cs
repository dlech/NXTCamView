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
using System.Windows.Forms;
using System.Xml.XPath;
using NXTCamView.Properties;

namespace NXTCamView.VersionUpdater
{
    public class Updater
    {
        public static readonly Updater Instance = new Updater();        

        public void CheckForUpdates()
        {
            CheckForUpdates(false);
        }

        public void CheckForUpdates( bool isAlwaysShowingResult )
        {
            List<ReleaseInfo> releases = getReleases();
            if( releases.Count > 0 )
            {
                Version latest = releases[0].Version;
                Version current = new Version(Application.ProductVersion);
                if( latest > current )             
                {
                    if (MessageBox.Show(
                        string.Format("A newer version of {0} is available at sourceforge.net.\n\nYour version:\t{1}\nLatest version:\t{2}\n\nShow release notes in a browser now?", Application.ProductName, current, latest),
                        string.Format("{0} - Check Updates", Application.ProductName),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        string link = releases[0].Link;
                        if (string.IsNullOrEmpty(link) ) link = "http://sourceforge.net/project/showfiles.php?group_id=203058";
                        Process.Start(link); 
                    }
                }
                else
                {
                    if( isAlwaysShowingResult )
                    {
                        MessageBox.Show(
                            string.Format("This is the lastest version of {0}.\nVersion: {1}", Application.ProductName, current),
                            string.Format("{0} - Check Updates", Application.ProductName),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }                    
                }
            }
            else
            {
                Debug.Write("CheckForUpdates failed to get any updates.");
            }
        }

        private List<ReleaseInfo> getReleases()
        {
            List<ReleaseInfo> releases = new List<ReleaseInfo>();
            try
            {                
                string releaseRssURL = Settings.Default.RRSReleaseFeed;
                if( string.IsNullOrEmpty(releaseRssURL) ) releaseRssURL = @"http://sourceforge.net/export/rss2_projfiles.php?group_id=203058";

                // Get the feed as a doc
                XPathDocument doc = new XPathDocument(releaseRssURL);
                releases = getReleases(doc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return releases;
        }

        //only used for unit testing
        public List< ReleaseInfo > GetReleasesTest(XPathDocument doc)
        {
            return getReleases(doc);
        }

        private List< ReleaseInfo > getReleases(XPathDocument doc)
        {
            List< ReleaseInfo > releases = new List< ReleaseInfo >();
            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator iter = nav.Select("rss/channel/item");
            while (iter.MoveNext())
            {
                ReleaseInfo info = ProcessNode(iter.Current);
                if( info != null )
                {
                    releases.Add(info);
                    Debug.WriteLine(info.ToString());
                }
            }
            //ensure the releases are ordered by version
            releases.Sort( new ReleaseInfoComparer() );
            return releases;
        }

        private ReleaseInfo ProcessNode(XPathNavigator lstNav)
        {
            string title = "";
            string link = "";
            string description = "";
            string author = "";
            string comment = "";
            string pubDate = "";

            // Get the item nodes
            XPathNodeIterator iterNews = lstNav.SelectDescendants(XPathNodeType.Element, false);

            while (iterNews.MoveNext())
            {
                string tag = iterNews.Current.Name;
                if( matches(tag, "title") )
                {
                    title = iterNews.Current.Value;
                }
                if( matches(tag, "description") ) 
                {
                    description = iterNews.Current.Value;
                }
                if( matches(tag, "author") )
                {
                    author = iterNews.Current.Value;
                }
                if( matches(tag, "comment") )
                {
                    comment = iterNews.Current.Value;
                }
                if (matches(tag, "pubDate"))
                {
                    pubDate = iterNews.Current.Value;
                }
                if (matches(tag, "link"))
                {
                    link = iterNews.Current.Value;
                }
            }
            return new ReleaseInfo(title, link, description, author, comment, pubDate);
        }

        static bool matches(string str1, string str2)
        {
            return str1.ToUpper() == str2.ToUpper();
        }

    }
}
