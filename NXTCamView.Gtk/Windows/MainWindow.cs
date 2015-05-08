//
//  MainWindow.cs
//
//  Author:
//       David Lechner <david@lechnology.com>
//
//  Copyright (c) 2015 David Lechner
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Diagnostics;

using Gtk;
using Ninject;

using NXTCamView.Core;
using NXTCamView.Core.Commands;
using NXTCamView.Core.Comms;
using System.Threading;

namespace NXTCamView.Gtk.Windows
{
    public partial class MainWindow : Window
    {
        const uint statusbarContextId = 0;

        readonly IAppState appState;
        readonly ISettings settings;
        readonly ICommsPort commsPort;
        readonly ICommsPortFactory commsPortFactory;

        public MainWindow () : base (WindowType.Toplevel)
        {
            this.Build ();
            StockManager.Add(new StockItem() { StockId = "camera-photo" });
        }

        [Inject]
        public MainWindow (IAppState appState, ISettings settings,
            IConfigCommsPort commsPort, ICommsPortFactory commsPortFactory)
            : this ()
        {
            this.appState = appState;
            this.settings = settings;
            this.commsPort = commsPort;
            this.commsPortFactory = commsPortFactory;

            this.appState.StateChanged += AppState_StateChanged;
            this.appState.State = NXTCamView.Core.State.NotConnected;
        }

        void AppState_StateChanged (object sender, EventArgs e)
        {
            UpdateSensitivity ();
            UpdateStatusBar ();
        }

        void UpdateSensitivity ()
        {
            connectAction.Sensitive = appState.State != NXTCamView.Core.State.Connected;
            disconnectAction.Sensitive = appState.State == NXTCamView.Core.State.Connected;
        }

        void UpdateStatusBar ()
        {
            statusbar.Pop (statusbarContextId);
            switch (appState.State) {
            case NXTCamView.Core.State.Connected:
                statusbar.Push (statusbarContextId, "Connected");
                break;
            case NXTCamView.Core.State.ConnectedBusy:
                statusbar.Push (statusbarContextId, "Busy");
                break;
            case NXTCamView.Core.State.ConnectedTracking:
                statusbar.Push (statusbarContextId, "Tracking");
                break;
            case NXTCamView.Core.State.NotConnected:
                statusbar.Push (statusbarContextId, "Disconnected");
                break;
            }
        }

        protected override void OnDestroyed ()
        {
            base.OnDestroyed ();
            Application.Quit ();
        }

        protected void OnAboutActionActivated (object sender, EventArgs e)
        {
            var dialog = new AboutDialog ();
            dialog.Run ();
        }

        protected void OnPreferencesActionActivated (object sender, EventArgs e)
        {
            var dialog = new PreferencesDialog (settings);
            dialog.Run ();
        }

        protected void OnQuitActionActivated (object sender, EventArgs e)
        {
            Destroy ();
        }

        protected void OnConnectActionActivated (object sender, EventArgs e)
        {
            var isOk = false;
            try {
                // TODO: This code is duplicated with WinForms.StripCommands.ConnectStripCommand
                var pingCmd = new PingCommand (appState, commsPort);
                pingCmd.Execute ();
                isOk = pingCmd.IsSuccessful;
                if (isOk) {
                    var versionCmd = new GetVersionCommand (appState, commsPort);
                    versionCmd.Execute ();
                    isOk = versionCmd.IsSuccessful;
                    //version = isOk ? versionCmd.Version : "";
                }
            } catch (Exception ex) {
                var dialog = new MessageDialog (this, DialogFlags.Modal, MessageType.Error,
                    ButtonsType.Ok, "Failed to connect:\n%s", ex.Message);
                dialog.Run ();
            }
        }

        protected void OnDisconnectActionActivated (object sender, EventArgs e)
        {
            // TODO: This code is duplicated from WinForms.StripCommands.DisconnectStripCommand
            // TODO: Check for background tasks that are using the serial port

            //this is not so pretty, but we want to clear away any junk
            //that would cause a connect to fail later
            while (commsPort.BytesToRead > 0) {
                commsPort.ReadExisting ();
                Thread.Sleep (100);
            }

            appState.State = NXTCamView.Core.State.NotConnected;
            commsPort.EnsureClosed ();
        }
    }
}
