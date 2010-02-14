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
using System.IO;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXTCamView.VersionUpdater;

namespace NXTCamViewTests
{
    [TestClass]
    public class UpdaterTest
    {
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private XPathDocument _sampleDoc;
        private readonly Updater _updater = new Updater();

        [TestInitialize()]
        public void TestInitialize()
        {
            using (FileStream stream = File.OpenRead("sampleRSS.xml"))
            {
                _sampleDoc = new XPathDocument(stream);
            }
        }

        [TestMethod]
        public void ParsingReleaseInfo()
        {
            List<ReleaseInfo> releases = _updater.GetReleasesTest(_sampleDoc);
            Assert.IsTrue(releases.Count == 3);
            Assert.IsTrue(releases[0].ReleaseDate.Date.Equals(new DateTime(2007, 8, 12)));
            Assert.IsTrue(releases[0].Version.ToString() == "2.0.0");
        }

        [TestMethod]
        public void SortingReleaseInfo()
        {
            List<ReleaseInfo> releases = _updater.GetReleasesTest(_sampleDoc);
            Assert.IsTrue(releases.Count == 3);
            Assert.IsTrue(releases[0].ReleaseDate.Date.Equals(new DateTime(2007, 8, 12)));
            Assert.IsTrue(releases[0].Version.ToString() == "2.0.0");
            Assert.IsTrue(releases[1].ReleaseDate.Date.Equals(new DateTime(2007, 8, 14)));
            Assert.IsTrue(releases[1].Version.ToString() == "0.1.6");
            Assert.IsTrue(releases[2].ReleaseDate.Date.Equals(new DateTime(2007, 8, 12)));
            Assert.IsTrue(releases[2].Version.ToString() == "0.1.6");
        }
    }
}
