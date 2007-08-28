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
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using Blue.Windows;
using NXTCamView.Commands;
using NXTCamView.Properties;
using NXTCamView.VersionUpdater;

namespace NXTCamView
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;
        
        public SerialPort SerialPort { get { return serialPort; } }
        public event EventHandler SerialPortChanged;

        public MainForm()
        {
            InitializeComponent();
            serialPort.NewLine = "\r";
            Settings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(settings_PropertyChanged);
            //not 100% thread safe, but ok as the app doesn't do anything until this is up
            Instance = this;

            StickyWindowsUtil.MakeSticky(this);
            StickyWindow.Active = Settings.Default.SnapWindows;
            tsmSnapWindows.Checked = StickyWindow.Active;
        }

        void settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                switch( e.PropertyName )
                {
                    case "COMPort":
                    case "BaudRate":
                    case "Parity":
                    case "Handshake":
                    case "DataBits":
                    case "StopBits":
                        //drop the old port and get another
                        if( serialPort.IsOpen ) serialPort.Close();
                        Settings settings = Settings.Default;

                        serialPort = new SerialPort(
                            settings.COMPort,
                            settings.BaudRate,
                            settings.Parity,
                            settings.DataBits,
                            settings.StopBits);
                        serialPort.WriteTimeout = settings.WriteTimeout;
                        serialPort.ReadTimeout = settings.ReadTimeout;
                        serialPort.NewLine = "\r";
                        serialPort.Open();
                        //notify listeners
                        if( SerialPortChanged != null )
                        {
                            SerialPortChanged( this,new EventArgs() );
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(string.Format("Error changing propertied {0}", ex));
            }
        }

        private void newCapture(object sender, EventArgs e)
        {
            capture();
        }

        private void capture()
        {
            CaptureForm form = new CaptureForm(serialPort);
            form.MdiParent = this;
            form.Visible = true;
            form.StartCapture();
        }

        private void tsbPing_Click(object sender, EventArgs e)
        {
            ping();
        }

        private void ping()
        {
            PingCommand cmd = new PingCommand(serialPort);
            cmd.Execute();
            if( cmd.IsSucessiful )
            {
                MessageBox.Show(this, "Success pinging!", Application.ProductName, MessageBoxButtons.OK,
                                MessageBoxIcon.None);
            }
            else
            {
                MessageBox.Show(this, string.Format("Error pinging: {0}", cmd.ErrorDescription),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void options_Click(object sender, EventArgs e)
        {
            showOptions();
        }

        private void showOptions()
        {
            OptionsForm form = new OptionsForm(serialPort);
            form.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CaptureForm form = new CaptureForm(serialPort);
                form.MdiParent = this;
                form.Show();
                form.LoadFile(openFileDialog.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(true);
        }

        private void saveFile(bool isSaveAs)
        {
            CaptureForm form = ActiveMdiChild as CaptureForm;
            if (form == null) return;
            if (form.Filename == "" || isSaveAs)
            {
                saveFileDialog.FileName = form.Filename != "" ? form.Filename : form.Text;
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                form.Filename = saveFileDialog.FileName;
            }
            form.SaveFile(form.Filename);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ping();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Size = new Size(1024, 800);
            Left = Screen.PrimaryScreen.WorkingArea.Width/2 - Width/2;
            Top = Screen.PrimaryScreen.WorkingArea.Height/2 - Height/2;

            ColorForm.Instance.VisibleChanged += ColorForm_VisibleChanged;
            TrackingForm.Instance.VisibleChanged += TrackingForm_VisibleChanged;

            if( Settings.Default.CheckForUpdates ) Updater.Instance.CheckForUpdates();

            setupSerialPort();
        }

        private void setupSerialPort()
        {
            bool isFound = false;
            string portSettings = Settings.Default.COMPort;
            foreach( string port in SerialPort.GetPortNames() )
            {
                if( port.Equals( portSettings ))
                {
                    isFound = true;
                }
            }
            if( !isFound )
            {
                if( MessageBox.Show(
                        string.Format("{0} is not a valid serial port on this PC. \nSet another port now?",
                                      Settings.Default.COMPort), ProductName, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes )
                {
                    showOptions();
                }
            }
            else
            {
                try
                {
                    serialPort.Open();
                }
                catch( Exception ex )
                {
                    Debug.WriteLine(string.Format("Error loading mainform {0}", ex));
                }
            }
        }

        void ColorForm_VisibleChanged(object sender, EventArgs e)
        {
            tsmColors.Checked = ColorForm.Instance.Visible;
        }

        private void TrackingForm_VisibleChanged(object sender, EventArgs e)
        {
            tsmTracking.Checked = TrackingForm.Instance.Visible;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox box = new AboutBox();
            box.ShowDialog(this);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort.IsOpen) serialPort.Close();
            Settings.Default.Save();
        }

        private void tsmTrackingColors_Click(object sender, EventArgs e)
        {
            ColorForm.Instance.SetVisibility(!tsmColors.Checked);
        }

        private void tsmTracking_Click(object sender, EventArgs e)
        {
            TrackingForm.Instance.SetVisibility(!tsmTracking.Checked);
        }

        private void tsmCascade_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tsmTileVertical_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void tsmArrangeIcons_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void tsmTileHorizontal_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void snapWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StickyWindow.Active = !StickyWindow.Active;
            Settings.Default.SnapWindows = StickyWindow.Active;
            tsmSnapWindows.Checked = StickyWindow.Active;
        }

        private void tsmCheckForUpdates_Click(object sender, EventArgs e)
        {
            Updater.Instance.CheckForUpdates( true );
        }
    }
}