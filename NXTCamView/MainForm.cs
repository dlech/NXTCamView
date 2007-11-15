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
using System.Threading;
using System.Windows.Forms;
using Blue.Windows;
using NXTCamView.Commands;
using NXTCamView.Properties;
using NXTCamView.Resources;
using NXTCamView.StripCommands;
using NXTCamView.VersionUpdater;

namespace NXTCamView
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;

        private ISerialProvider _serialProvider;
        private OpenOptionsStripCommand openOptionsCmd;
        private OpenColorStripCommand openColorCmd;
        private OpenTrackingStripCommand openTrackingCmd;

        public MainForm()
        {
            _serialProvider = SerialProvider.Instance;
            Thread.CurrentThread.Name = "GuiThread";
            Icon = AppImages.GetIcon(AppImages.MainForm);
            InitializeComponent();
            
            //not 100% thread safe, but ok as the app doesn't do anything until this is up
            Instance = this;

            StickyWindowsUtil.MakeSticky(this);
            StickyWindow.Active = Settings.Default.SnapWindows;
            tsmSnapWindows.Checked = StickyWindow.Active;

            AppState.Inst.StateChanged += AppStateChanged;
            AppState.Inst.State = State.NotConnected;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        }

        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Debug.WriteLine( "Unhandled thread exception: " + e.Exception );
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TEST DEBUGGING
            //MessageBox.Show( this,
            //                 string.Format( "Unfortunately a problem has occurred and {0} needs to close.  \nSorry for the inconvienience", Application.ProductName ),
            //                 string.Format("{0} - {1}", 
            //                 Application.ProductName, 
            //                 Application.ProductVersion));

            //This is most probably caused by the USB cable being removed and causing an issue 
            //with an unhandled exception. 
            //See http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=523638&SiteID=1
            //Workaround is via the app config!
            Debug.WriteLine("Unhandled exception: " + e.ExceptionObject);            
        }

        private void tsbPing_Click(object sender, EventArgs e)
        {
            ping();
        }

        private void ping()
        {
            PingCommand cmd = new PingCommand( _serialProvider );
            cmd.Execute();
            if( cmd.IsSuccessful )
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
            ColorForm.Init();
            TrackingForm.Init();

            Size = new Size(1024, 800);
            Left = Screen.PrimaryScreen.WorkingArea.Width/2 - Width/2;
            Top = Screen.PrimaryScreen.WorkingArea.Height/2 - Height/2;
            
            //make sure we are on screen - we could be too high or too left if the screen is < window size
            if( !Screen.PrimaryScreen.WorkingArea.Contains( Left, Top ) )
            {
                Left = Math.Max( Screen.PrimaryScreen.WorkingArea.X, Left );
                Top = Math.Max(Screen.PrimaryScreen.WorkingArea.Y, Top );
            }

            setupStripCommands();

            if( Settings.Default.CheckForUpdates ) Updater.Instance.CheckForUpdates();

            tsslVersion.Text = "";

            setupMenus();
            setupSerialPort();
            updateAllEnablement();
        }

        private void setupStripCommands()
        {
            openOptionsCmd = new OpenOptionsStripCommand();
            openColorCmd = new OpenColorStripCommand();
            openTrackingCmd = new OpenTrackingStripCommand();
        }

        private void setupMenus()
        {
            //file menu
            StripCommand connectCmd = new ConnectStripCommand( _serialProvider );
            connectCmd.Completed += connectCmd_Completed;

            setupButtonAndMenu(tsbConnect, tsmConnect, "Connect", "Connect to the NXTCam", AppImages.Connect, connectCmd);            

            StripCommand disconnectCmd = new DisconnectStripCommand( SerialProvider.Instance );
            disconnectCmd.Completed += disconnectCmd_Completed;
            setupButtonAndMenu(tsbDisconnect, tsmDisconnect, "Disconnect", "Disconnect from the NXTCam", AppImages.Disconnect, disconnectCmd);

            setupButtonAndMenu(tsbCapture, tsmCapture, "Capture", "Capture an image from the NXTCam", AppImages.Capture, new CaptureStripCommand() );

            setupMenu(tsmOpenFile, "&Open", "Open a capture", AppImages.OpenFile, new OpenFileStripCommand( openFileDialog ));
            setupMenu(tsmSaveFile, "&Save", "Save a capture", AppImages.SaveFile, new SaveFileStripCommand(saveFileDialog, false));
            setupMenu(tsmSaveFileAs, "Save &As", "Save a capture with filename", null, new SaveFileStripCommand(saveFileDialog, true));
            
            //view menu
            ColorForm.Instance.VisibleChanged += form_VisibleChanged;
            setupButtonAndMenu(tsbOpenColors, tsmOpenColors, "&Colors", "Show/hide colors window", AppImages.Colors, openColorCmd);
            TrackingForm.Instance.VisibleChanged += form_VisibleChanged;
            setupButtonAndMenu(tsbOpenTracking, tsmOpenTracking, "&Tracking", "Show/hide tracking window", AppImages.Tracking, openTrackingCmd);

            //option menu
            setupMenu(tsmOpenOptions, "&Options", "Open application options", AppImages.Options, openOptionsCmd);
        }


        private bool _isConnected = false;

        public bool IsConnected { get { return _isConnected; } }

        public event EventHandler< EventArgs > ConnectionStateChanged;

        private void connectionStateChanged(bool isConnected)
        {            
            if (_isConnected != isConnected)
            {
                _isConnected = isConnected;
                if( ConnectionStateChanged != null )
                {
                    ConnectionStateChanged(this, new EventArgs());
                }
            }
        }

        void connectCmd_Completed(object sender, EventArgs e)
        {
            connectionStateChanged(true);
            ConnectStripCommand connectCmd = (ConnectStripCommand) sender;
            tsslVersion.Text = connectCmd.GetVersion();
        }

        private void disconnectCmd_Completed(object sender, EventArgs e)        
        {
            connectionStateChanged(false);
            tsslVersion.Text = "";
        }

        private void setupSerialPort()
        {
            bool isFound = false;
            string portSettings = Settings.Default.COMPort;
            foreach( string port in System.IO.Ports.SerialPort.GetPortNames() )
            {
                if( port.Equals( portSettings ))
                {
                    isFound = true;
                }
            }
            //create the serial provider from config
            if( !isFound )
            {
                if( MessageBox.Show(
                        string.Format("{0} is not a valid serial port on this PC. \nSet another port now?",
                                      Settings.Default.COMPort), ProductName, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes )
                {
                    openOptionsCmd.Execute();
                }
            }
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
            _serialProvider.EnsureClosed(); 
            Settings.Default.Save();
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

        private void AppStateChanged(object sender, EventArgs args)
        {
            if( InvokeRequired )
            {
                BeginInvoke(new EventHandler(AppStateChanged), new object[] {sender, args});
                return;
            }
            updateAllEnablement();
            updateConnectionState();
        }

        private void updateConnectionState()
        {
            switch( AppState.Inst.State )
            {
                case State.NotConnected:
                    setStatusState("Not connected", AppImages.NotConnected);
                    break;
                case State.Connected:
                    setStatusState("Connected", AppImages.Connected);
                    break;
                case State.ConnectedBusy:
                    setStatusState("Connected - Busy", AppImages.ConnectedBusy);
                    break;
                case State.ConnectedTracking:
                    setStatusState("Connected - Tracking", AppImages.ConnectedTracking);
                    break;
            }
        }

        private void setStatusState( string text, Image image )
        {
            tsslConnectionStatus.Text = text;
            tsslConnectionStatus.Image = image;
        }

        #region StripCommand Handling

        private void setupButtonAndMenu(ToolStripButton tsButton, ToolStripMenuItem tsMenu, string caption, string tip, Image image, StripCommand command)
        {
            setupItem(tsButton, caption, tip, image, command);
            setupMenu(tsMenu, caption, tip, image, command);
        }

        private void setupItem(ToolStripItem tsItem, string caption, string tip, Image image, StripCommand command)
        {
            tsItem.Text = caption;
            tsItem.Image = image;
            tsItem.Tag = command;
            tsItem.ToolTipText = tip;
            tsItem.Click += ToolItem_Click;
        }

        private void setupMenu(ToolStripMenuItem tsMenu, string caption, string tip, Image image, StripCommand command)
        {
            tsMenu.Text = caption;
            tsMenu.Image = image;
            tsMenu.Tag = command;
            tsMenu.ToolTipText = tip;
            tsMenu.Click += ToolItem_Click;
        }

        void form_VisibleChanged(object sender, EventArgs e)
        {
            //if one of the forms has changed state, then update toolbar enablements
            updateEnablement(tsToolBar.Items);
        }

        private void ToolItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item == null) throw new ApplicationException(string.Format("sender is not a toolstrip: {0}", sender));

            StripCommand cmd = item.Tag as StripCommand;
            if (cmd == null) throw new ApplicationException(string.Format("toolstrip has not command: {0}", item.Text));
            
            if (cmd.CanExecute()) cmd.Execute();

            updateAllEnablement();
        }

        private void updateAllEnablement()
        {
            updateEnablement( msMainMenu.Items );
            updateEnablement( tsToolBar.Items );
        }

        private void updateEnablement( ToolStripItemCollection items )
        {
            foreach (ToolStripItem item in items)
            {
                if (item != null)
                {
                    StripCommand cmd = item.Tag as StripCommand;
                    if( cmd != null )
                    {
                        item.Enabled = cmd.CanExecute();

                        ToolStripButton button = item as ToolStripButton;
                        if( button != null ) button.Checked = cmd.HasExecuted();

                        ToolStripMenuItem menu = item as ToolStripMenuItem;
                        if (menu != null) menu.Checked = cmd.HasExecuted();
                    }
                }
            }
        }

        //Called before an menu downs down to update the enablement based on the attached commands
        private void menu_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripItemCollection items = ((ToolStripMenuItem) sender).DropDownItems;
            updateEnablement( items );
        }

        #endregion
    }
}