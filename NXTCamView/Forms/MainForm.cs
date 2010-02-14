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
#region using

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Ninject;
using NXTCamView.Commands;
using NXTCamView.Comms;
using NXTCamView.Controls;
using NXTCamView.Properties;
using NXTCamView.Resources;
using NXTCamView.StripCommands;
using NXTCamView.VersionUpdater;

#endregion

namespace NXTCamView.Forms
{
    public partial class MainForm : Form, IInitializable
    {
        private readonly IAppState _appState;
        private readonly ICommsPort _commsPort;
        private readonly ICommsPortFactory _commsPortFactory;
        private OpenOptionsStripCommand _openOptionsCmd;
        private OpenColorStripCommand _openColorCmd;
        private OpenTrackingStripCommand _openTrackingCmd;
        private bool _isConnected;
        private readonly IUpdater _updater;
        private readonly ColorForm _colorForm;
        private readonly TrackingForm _trackingForm;

        //for designer
        public MainForm()
        {
            InitializeComponent();
        }

        [Inject]
        public MainForm( IAppState appState, IUpdater updater, ColorForm colorForm, TrackingForm trackingForm,
                         IConfigCommsPort commsPort, ICommsPortFactory commsPortFactory )
        {
            _appState = appState;
            _commsPort = commsPort;
            _commsPortFactory = commsPortFactory;
            _updater = updater;
            _colorForm = colorForm;
            _trackingForm = trackingForm;
            Thread.CurrentThread.Name = "GuiThread";
            Icon = AppImages.GetIcon( AppImages.MainForm );
            InitializeComponent();

            StickyWindowsUtil.MakeSticky( this );
            StickyWindow.Active = Settings.Default.SnapWindows;
            tsmSnapWindows.Checked = StickyWindow.Active;

            _appState.StateChanged += appStateChanged;
            _appState.State = State.NotConnected;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
        }

        private static void Application_ThreadException( object sender, ThreadExceptionEventArgs e )
        {
            Debug.WriteLine( "Unhandled thread exception: " + e.Exception );
        }

        private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
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
            Debug.WriteLine( "Unhandled exception: " + e.ExceptionObject );
        }

        private void tsbPing_Click( object sender, EventArgs e )
        {
            ping();
        }

        private void ping()
        {
            var cmd = new PingCommand( _appState, _commsPort );
            cmd.Execute();
            if ( cmd.IsSuccessful )
            {
                MessageBox.Show( this, "Success pinging!", Application.ProductName, MessageBoxButtons.OK,
                                 MessageBoxIcon.None );
            }
            else
            {
                MessageBox.Show( this, string.Format( "Error pinging: {0}", cmd.ErrorDescription ),
                                 Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void pingToolStripMenuItem_Click( object sender, EventArgs e )
        {
            ping();
        }

        private void mainFormLoad( object sender, EventArgs e )
        {
            Size = new Size( 1024, 800 );
            Left = Screen.PrimaryScreen.WorkingArea.Width/2 - Width/2;
            Top = Screen.PrimaryScreen.WorkingArea.Height/2 - Height/2;

            //make sure we are on screen - we could be too high or too left if the screen is < window size
            if ( !Screen.PrimaryScreen.WorkingArea.Contains( Left, Top ) )
            {
                Left = Math.Max( Screen.PrimaryScreen.WorkingArea.X, Left );
                Top = Math.Max( Screen.PrimaryScreen.WorkingArea.Y, Top );
            }

            setupStripCommands();

            if ( Settings.Default.CheckForUpdates ) _updater.CheckForUpdates();

            tsslVersion.Text = "";

            setupMenus();
            setupSerialPort();
            updateAllEnablement();
        }

        private void setupStripCommands()
        {
            _openOptionsCmd = new OpenOptionsStripCommand( _appState, _commsPort, _commsPortFactory );
            _openColorCmd = new OpenColorStripCommand( _appState, _colorForm );
            _openTrackingCmd = new OpenTrackingStripCommand( _appState, _trackingForm );
        }

        private void setupMenus()
        {
            //file menu
            StripCommand connectCmd = new ConnectStripCommand( _appState, _commsPort, this, _commsPortFactory );
            connectCmd.Completed += connectCmd_Completed;

            setupButtonAndMenu( tsbConnect, tsmConnect, "Connect", "Connect to the NXTCam", AppImages.Connect,
                                connectCmd );

            StripCommand disconnectCmd = new DisconnectStripCommand( _appState, this, _commsPort );
            disconnectCmd.Completed += disconnectCmd_Completed;
            setupButtonAndMenu( tsbDisconnect, tsmDisconnect, "Disconnect", "Disconnect from the NXTCam",
                                AppImages.Disconnect, disconnectCmd );

            setupButtonAndMenu( tsbCapture, tsmCapture, "Capture", "Capture an image from the NXTCam", AppImages.Capture,
                                new CaptureStripCommand( _appState, this, _colorForm, _commsPort ) );

            setupMenu( tsmOpenFile, "&Open", "Open a capture", AppImages.OpenFile,
                       new OpenFileStripCommand( _appState, this, _colorForm, openFileDialog, _commsPort ) );
            setupMenu( tsmSaveFile, "&Save", "Save a capture", AppImages.SaveFile,
                       new SaveFileStripCommand( _appState, this, saveFileDialog, false ) );
            setupMenu( tsmSaveFileAs, "Save &As", "Save a capture with filename", null,
                       new SaveFileStripCommand( _appState, this, saveFileDialog, true ) );

            //view menu
            _colorForm.VisibleChanged += form_VisibleChanged;
            setupButtonAndMenu( tsbOpenColors, tsmOpenColors, "&Colors", "Show/hide colors window", AppImages.Colors,
                                _openColorCmd );
            _trackingForm.VisibleChanged += form_VisibleChanged;
            setupButtonAndMenu( tsbOpenTracking, tsmOpenTracking, "&Tracking", "Show/hide tracking window",
                                AppImages.Tracking, _openTrackingCmd );

            //option menu
            setupMenu( tsmOpenOptions, "&Options", "Open application options", AppImages.Options, _openOptionsCmd );
        }

        public event EventHandler<EventArgs> ConnectionStateChanged;

        private void connectionStateChanged( bool isConnected )
        {
            if ( _isConnected == isConnected ) return;
            _isConnected = isConnected;

            if ( ConnectionStateChanged == null ) return;
            ConnectionStateChanged( this, new EventArgs() );
        }

        private void connectCmd_Completed( object sender, EventArgs e )
        {
            connectionStateChanged( true );
            var connectCmd = (ConnectStripCommand) sender;
            tsslVersion.Text = connectCmd.GetVersion();
        }

        private void disconnectCmd_Completed( object sender, EventArgs e )
        {
            connectionStateChanged( false );
            tsslVersion.Text = "";
        }

        private void setupSerialPort()
        {
            bool isFound = false;
            string portSettings = Settings.Default.COMPort;
            foreach ( string port in System.IO.Ports.SerialPort.GetPortNames() )
            {
                if ( port.Equals( portSettings ) )
                {
                    isFound = true;
                }
            }
            //create the serial provider from config
            if ( !isFound )
            {
                if ( MessageBox.Show(
                                        string.Format(
                                                         "{0} is not a valid serial port on this PC. \nSet another port now?",
                                                         Settings.Default.COMPort ), ProductName,
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button1 ) == DialogResult.Yes )
                {
                    _openOptionsCmd.Execute();
                }
            }
        }

        private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
        {
            var box = new AboutBox();
            box.ShowDialog( this );
        }

        private void exitToolStripMenuItem1_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void mainFormFormClosed( object sender, FormClosedEventArgs e )
        {
            _commsPort.EnsureClosed();
            Settings.Default.Save();
        }

        private void tsmCascade_Click( object sender, EventArgs e )
        {
            LayoutMdi( MdiLayout.Cascade );
        }

        private void tsmTileVertical_Click( object sender, EventArgs e )
        {
            LayoutMdi( MdiLayout.TileVertical );
        }

        private void tsmArrangeIcons_Click( object sender, EventArgs e )
        {
            LayoutMdi( MdiLayout.ArrangeIcons );
        }

        private void tsmTileHorizontal_Click( object sender, EventArgs e )
        {
            LayoutMdi( MdiLayout.TileHorizontal );
        }

        private void snapWindowsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            StickyWindow.Active = !StickyWindow.Active;
            Settings.Default.SnapWindows = StickyWindow.Active;
            tsmSnapWindows.Checked = StickyWindow.Active;
        }

        private void tsmCheckForUpdates_Click( object sender, EventArgs e )
        {
            _updater.CheckForUpdates( true );
        }

        private void appStateChanged( object sender, EventArgs args )
        {
            if ( InvokeRequired )
            {
                BeginInvoke( new EventHandler( appStateChanged ), new[] {sender, args} );
                return;
            }
            updateAllEnablement();
            updateConnectionState();
        }

        private void updateConnectionState()
        {
            switch ( _appState.State )
            {
                case State.NotConnected:
                    setStatusState( "Not connected", AppImages.NotConnected );
                    break;
                case State.Connected:
                    setStatusState( "Connected", AppImages.Connected );
                    break;
                case State.ConnectedBusy:
                    setStatusState( "Connected - Busy", AppImages.ConnectedBusy );
                    break;
                case State.ConnectedTracking:
                    setStatusState( "Connected - Tracking", AppImages.ConnectedTracking );
                    break;
            }
        }

        private void setStatusState( string text, Image image )
        {
            tsslConnectionStatus.Text = text;
            tsslConnectionStatus.Image = image;
        }

        #region StripCommand Handling

        private void setupButtonAndMenu( ToolStripButton tsButton, ToolStripMenuItem tsMenu, string caption, string tip,
                                         Image image, StripCommand command )
        {
            setupItem( tsButton, caption, tip, image, command );
            setupMenu( tsMenu, caption, tip, image, command );
        }

        private void setupItem( ToolStripItem tsItem, string caption, string tip, Image image, StripCommand command )
        {
            tsItem.Text = caption;
            tsItem.Image = image;
            tsItem.Tag = command;
            tsItem.ToolTipText = tip;
            tsItem.Click += toolItemClick;
        }

        private void setupMenu( ToolStripMenuItem tsMenu, string caption, string tip, Image image, StripCommand command )
        {
            tsMenu.Text = caption;
            tsMenu.Image = image;
            tsMenu.Tag = command;
            tsMenu.ToolTipText = tip;
            tsMenu.Click += toolItemClick;
        }

        private void form_VisibleChanged( object sender, EventArgs e )
        {
            //if one of the forms has changed state, then update toolbar enablements
            updateEnablement( tsToolBar.Items );
        }

        private void toolItemClick( object sender, EventArgs e )
        {
            var item = sender as ToolStripItem;
            if ( item == null )
                throw new ApplicationException( string.Format( "sender is not a toolstrip: {0}", sender ) );

            var cmd = item.Tag as StripCommand;
            if ( cmd == null )
                throw new ApplicationException( string.Format( "toolstrip has not command: {0}", item.Text ) );

            if ( cmd.CanExecute() ) cmd.Execute();

            updateAllEnablement();
        }

        private void updateAllEnablement()
        {
            updateEnablement( msMainMenu.Items );
            updateEnablement( tsToolBar.Items );
        }

        private void updateEnablement( ToolStripItemCollection items )
        {
            foreach ( ToolStripItem item in items )
            {
                if ( item == null ) continue;
                var cmd = item.Tag as StripCommand;
                if ( cmd == null ) continue;
                item.Enabled = cmd.CanExecute();

                var button = item as ToolStripButton;
                if ( button != null ) button.Checked = cmd.HasExecuted();

                var menu = item as ToolStripMenuItem;
                if ( menu != null ) menu.Checked = cmd.HasExecuted();
            }
        }

        //Called before an menu downs down to update the enablement based on the attached commands
        private void menu_DropDownOpening( object sender, EventArgs e )
        {
            ToolStripItemCollection items = ( (ToolStripMenuItem) sender ).DropDownItems;
            updateEnablement( items );
        }

        #endregion

        public void Initialize()
        {
            //setup the properties that would be cyclic
            _colorForm.MainForm = this;
            _trackingForm.MainForm = this;
        }
    }
}