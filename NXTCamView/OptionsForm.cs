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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using DevInfo;
using NXTCamView.Commands;
using NXTCamView.Properties;
using NXTCamView.Resources;

namespace NXTCamView
{
    public partial class OptionsForm : Form
    {
        private SerialPort _serialPort;
        private SerialPort _testSerialPort;
        private bool _isClosing;
        private bool _isChangeUploadedAdv;
        private List< CustomRegisterSettingControl > _customSettings;
        private bool _isChangeUploaded;
        private bool _hasDoneTest;
        private bool _hasSavedSettings;
        private const int MAX_UPLOAD_REGISTERS = 8;
        static private Dictionary< string, string > _friendlyNameByPort;

        public OptionsForm(SerialPort serialPort)
        {
            InitializeComponent();
            
            Icon = Icon.FromHandle(((Bitmap)AppImages.Options).GetHicon());

            _serialPort = serialPort;
            SuspendLayout();

            List<StrIntBinder> items = new List< StrIntBinder >();
            items.Add(new StrIntBinder("19.2k", 19200));
            items.Add(new StrIntBinder("38.4k", 38400));
            items.Add(new StrIntBinder("115.2k", 115200));
            items.Add(new StrIntBinder("230.4k", 230400));
            items.Add(new StrIntBinder("460.8k", 460800));
            cobBaudRate.Items.AddRange(items.ToArray());

            items.Clear();
            items.Add(new StrIntBinder("8", 8));
            items.Add(new StrIntBinder("7", 7));
            cobDataBits.Items.AddRange(items.ToArray());

            items.Clear();
            items.Add(new StrIntBinder("Even", (int)Parity.Even));
            items.Add(new StrIntBinder("Mark", (int)Parity.Mark));
            items.Add(new StrIntBinder("None", (int)Parity.None));
            items.Add(new StrIntBinder("Odd", (int)Parity.Odd));
            items.Add(new StrIntBinder("Space", (int)Parity.Space));
            cobParity.Items.AddRange(items.ToArray());

            items.Clear();
            //items.Add(new StrIntBinder("None", (int)StopBits.None));
            items.Add(new StrIntBinder("1", (int)StopBits.One));
            items.Add(new StrIntBinder("1.5", (int)StopBits.OnePointFive));
            items.Add(new StrIntBinder("2", (int)StopBits.Two));
            cobStopBits.Items.AddRange(items.ToArray());

            items.Clear();
            items.Add(new StrIntBinder("None", (int)Handshake.None));
            items.Add(new StrIntBinder("RequestToSend", (int)Handshake.RequestToSend));
            items.Add(new StrIntBinder("RequestToSendXOnXOff", (int)Handshake.RequestToSendXOnXOff));
            items.Add(new StrIntBinder("XOnXOff", (int)Handshake.XOnXOff));
            cobHandshake.Items.AddRange(items.ToArray());

            string settings = Settings.Default.CustomRegisters;
            string[] registerSettings = settings.Split(':');
            //handle not settings
            if( registerSettings.Length != MAX_UPLOAD_REGISTERS ) registerSettings = new string[MAX_UPLOAD_REGISTERS];
            _customSettings = new List< CustomRegisterSettingControl >();
            for (int i = 0; i < MAX_UPLOAD_REGISTERS; i++)
            {
                CustomRegisterSettingControl control = new CustomRegisterSettingControl(i, registerSettings[i], nudReg_ValueChanged);
                control.Left = 20;
                control.Top = 38 + (i*26);
                _customSettings.Add(control);
                tabNXTCamSettingsAdv.Controls.Add(control);
            }
            ResumeLayout();
        }

        public class StrIntBinder
        {
            public readonly string Text;
            public readonly int IntValue;

            public StrIntBinder(string text, int intValue)
            {
                Text = text;
                IntValue = intValue;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void OptionsForm_Load(object sender, System.EventArgs e)
        {
            Settings settings = Settings.Default;
            lbResult.Text = "";

            if( _friendlyNameByPort == null )
            {
                //repopulate the comports first time through
                _friendlyNameByPort = SerialHelper.GetPorts();
            }

            setupComportDropdown();

            cobBaudRate.SelectedIndex = getIndex(cobBaudRate,Settings.Default.BaudRate);
            cobDataBits.SelectedIndex = getIndex(cobDataBits, Settings.Default.DataBits);
            cobHandshake.SelectedIndex = getIndex(cobHandshake, (int) Settings.Default.Handshake);
            cobParity.SelectedIndex = getIndex(cobParity, (int) Settings.Default.Parity);
            cobStopBits.SelectedIndex = getIndex(cobStopBits, (int)Settings.Default.StopBits);

            cbUseAutoWhiteBalance.Checked = settings.AutoWhiteBalance;
            cbUseAutoAdjust.Checked = settings.AutoAdjustMode;
            cbUseFlourescentLightFilter.Checked = settings.FlourescentLightFilter;

            cbCheckForUpdates.Checked = settings.CheckForUpdates;

            _isChangeUploaded = true;
            lbMessage.Text = "";

            lbMessageAdv.Text = "";
            _isChangeUploadedAdv = true;

            _hasDoneTest = false;
            _hasSavedSettings = false;
        }

        private int getIndex(ComboBox comboBox, int intValue)
        {
            for (int index = 0; index < comboBox.Items.Count; index++)
            {
                StrIntBinder binder = (StrIntBinder)comboBox.Items[index];
                if (binder.IntValue == intValue) return index;
            }
            return -1;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private bool saveSettings()
        {
            //check whats changed and save if necessary
            Settings proposed = Settings.From(Settings.Default);
            
            if( proposed == null ) return false;

            //if we did a test then ensure we save to force the COMPort to reopen
            bool hasChanged = false;
            bool hasCommsChanged = false;
            if ( proposed.COMPort == null || !proposed.COMPort.Equals(cobCOMPorts.SelectedItem) || _hasDoneTest )
            {
                hasCommsChanged = true;
                proposed.COMPort = (string)cobCOMPorts.SelectedItem;
            }
            if (proposed.BaudRate != ((StrIntBinder)cobBaudRate.SelectedItem).IntValue)
            {
                hasCommsChanged = true;
                proposed.BaudRate = ((StrIntBinder)cobBaudRate.SelectedItem).IntValue;
            }
            if (proposed.Handshake != (Handshake)((StrIntBinder)cobHandshake.SelectedItem).IntValue)
            {
                hasCommsChanged = true;
                proposed.Handshake = (Handshake)((StrIntBinder)cobHandshake.SelectedItem).IntValue;
            }
            if (proposed.Parity != (Parity)((StrIntBinder)cobParity.SelectedItem).IntValue)
            {
                hasCommsChanged = true;
                proposed.Parity = (Parity)((StrIntBinder)cobParity.SelectedItem).IntValue;
            }
            if (proposed.DataBits != ((StrIntBinder)cobDataBits.SelectedItem).IntValue)
            {
                hasCommsChanged = true;
                proposed.DataBits = ((StrIntBinder)cobDataBits.SelectedItem).IntValue;
            }
            if (proposed.StopBits != (StopBits)((StrIntBinder)cobStopBits.SelectedItem).IntValue)
            {
                hasCommsChanged = true;
                proposed.StopBits = (StopBits)((StrIntBinder)cobStopBits.SelectedItem).IntValue;
            }
            if (proposed.CheckForUpdates != cbCheckForUpdates.Checked)
            {
                hasChanged = true;
                proposed.CheckForUpdates = cbCheckForUpdates.Checked;
            }
            if( hasChanged || hasCommsChanged)
            {
                //if we're connected and changing connection details we should offer to disconnect
                if( AppState.Inst.State != State.NotConnected && hasCommsChanged)
                {
                    DialogResult result = MessageBox.Show(
                        this,
                        "The NXTCam will be disconnected to change the connection details.",
                        Application.ProductName,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                    if( result == DialogResult.Cancel ) return false;
                    AppState.Inst.State = State.NotConnected;
                    //TODO - maybe offer to reconnect?
                }
                Settings.Default.CopyFrom(proposed);                
                _hasSavedSettings = true;
            }
            return true;
        }

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            if( backgroundWorker.IsBusy )
            {
                btnTest.Enabled = false;
                lbResult.Text = "Aborting";
                backgroundWorker.CancelAsync();

                //hack - must force the serial port to stop from this thread - hmmm
                _testSerialPort.Close();
            }
            else
            {
                btnTest.Enabled = true;
                btnTest.Text = "Abort";

                lbResult.ForeColor = Color.Black;
                lbResult.Text = "Sending ping...";
                lbResult.Refresh();

                _hasDoneTest = true;

                //setup a new port to test
                _testSerialPort = new SerialPort((string)cobCOMPorts.SelectedItem, 
                    ((StrIntBinder)cobBaudRate.SelectedItem).IntValue,
                    (Parity) ((StrIntBinder)cobParity.SelectedItem).IntValue,
                    ((StrIntBinder)cobDataBits.SelectedItem).IntValue,
                    (StopBits) ((StrIntBinder)cobStopBits.SelectedItem).IntValue);
                _testSerialPort.Handshake = (Handshake) ((StrIntBinder)cobHandshake.SelectedItem).IntValue;
                _testSerialPort.NewLine = "\r";
                _testSerialPort.WriteTimeout = _serialPort.WriteTimeout;
                _testSerialPort.ReadTimeout = _serialPort.ReadTimeout;                    

                backgroundWorker.RunWorkerAsync();
            }
        }

        private class Result
        {
            public Result(string message, Color color)
            {
                Message = message;
                Color = color;
            }

            public readonly string Message;
            public readonly Color Color;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!_testSerialPort.IsOpen) _testSerialPort.Open();
                PingCommand cmd = new PingCommand(_testSerialPort);
                e.Result = cmd;
                cmd.Execute();
                if( cmd.IsCompleted && cmd.IsSuccessful )
                {
                    e.Result = new Result("Success: NXTCam responded", Color.Green);
                }
                else
                {
                    setErrorMessage(e, cmd.ErrorDescription);
                }
            }
            catch( IOException ex )
            {
                setErrorMessage(e, ex.Message);
            }
        }

        private void setErrorMessage(DoWorkEventArgs e, string message)
        {
            e.Result = backgroundWorker.CancellationPending ? 
                       new Result("Test aborted", Color.Red) :
                       new Result(message, Color.Red);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Result result = (Result)e.Result;
            lbResult.ForeColor = result.Color;
            lbResult.Text = result.Message;

            btnTest.Text = "Test";
            btnTest.Enabled = true;

            if( _testSerialPort.IsOpen) _testSerialPort.Close();

            //the form was trying to close but the busy test stopped it, so make it close
            if( _isClosing ) Close();
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //wait for any coms to stop
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = false;
                backgroundWorker.CancelAsync();
                _isClosing = true;
                return;
            }
            if( !_isChangeUploaded )
            {
                if (MessageBox.Show(this,"Changes to the NXTCam setting have not been uploaded.\nUpload them now?",ProductName,MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    tcOptions.SelectTab(tabNXTCamSettings);
                    tcOptions.Update();
                    //don't let the page close if the upload fails
                    e.Cancel = !uploadSettings();
                    return;
                }
            }
            if (!_isChangeUploadedAdv)
            {
                if (MessageBox.Show(this, "Changes to the Advanced NXTCam setting have not been uploaded.\nUpload them now?", ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    tcOptions.SelectTab(tabNXTCamSettingsAdv);
                    tcOptions.Update();
                    //don't let the page close if the upload fails
                    e.Cancel = !uploadSettingsAdv();
                    return;
                }
            }
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            uploadSettings();
        }

        private bool uploadSettings()
        {
            btnUploadRegisters.Enabled = false;
            btnUploadRegisters.Refresh();
            lbMessage.Text = "Uploading settings";
            lbMessage.ForeColor = Color.Black;
            lbMessage.Refresh();
            
            SetRegistersCommand cmd = new SetRegistersCommand(_serialPort);
            cmd.IsAutoWhiteBalance = cbUseAutoWhiteBalance.Checked;
            cmd.IsAutoAdjustMode = cbUseAutoAdjust.Checked;
            cmd.HasFlourescentLightFilter = cbUseFlourescentLightFilter.Checked;
            cmd.Execute();

            bool isOk = cmd.IsSuccessful;

            if (isOk)
            {
                //save the settings
                Settings settings = Settings.Default;
                settings.AutoWhiteBalance = cbUseAutoWhiteBalance.Checked;
                settings.AutoAdjustMode = cbUseAutoAdjust.Checked;
                settings.FlourescentLightFilter = cbUseFlourescentLightFilter.Checked;
                settings.Save();
            }

            lbMessage.Text = isOk ? "Upload succeeded" : cmd.ErrorDescription;
            lbMessage.ForeColor = isOk ? Color.Black : Color.Red;
            _isChangeUploaded = isOk;
            btnUploadRegisters.Enabled = true;

            return isOk;
        }

        private void cb_CheckedChanged(object sender, System.EventArgs e)
        {
            lbMessage.ForeColor = Color.Black;
            lbMessage.Text = "Changes not yet uploaded";
            _isChangeUploaded = false;
        }

        private void nudReg_ValueChanged(object sender, System.EventArgs e)
        {
            lbMessageAdv.ForeColor = Color.Black;
            lbMessageAdv.Text = "Changes not yet uploaded";
            _isChangeUploadedAdv = false;
        }

        private void btnUploadAdvanced_Click(object sender, System.EventArgs e)
        {
            uploadSettingsAdv();
        }

        private bool uploadSettingsAdv()
        {
            int count = 0;
            SetRegistersCommand cmd = new SetRegistersCommand(_serialPort);
            foreach( CustomRegisterSettingControl control in _customSettings )
            {
                if( control.RegisterEnabled )
                {
                    if (!cmd.Register.ContainsKey(control.Register))
                    {
                        cmd.Register.Add(control.Register, control.Value);
                        count++;
                    }
                    else
                    {
                        MessageBox.Show(this,string.Format("Register {0} is included more than once.  Upload aborted",control.Register), ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            if (count > 0)
            {
                btnUploadRegistersAdv.Enabled = false;
                btnUploadRegistersAdv.Refresh();
                lbMessageAdv.Text = "Uploading settings";
                lbMessageAdv.ForeColor = Color.Black;
                lbMessageAdv.Refresh();

                cmd.Execute();

                bool isOk = cmd.IsSuccessful;

                if( isOk )
                {
                    //save the settings
                    List< string > settings = new List< string >();
                    foreach( CustomRegisterSettingControl control in _customSettings )
                    {
                        settings.Add(control.GetConfigSetting());
                    }
                    Settings.Default.CustomRegisters = string.Join(":", settings.ToArray());
                }

                lbMessageAdv.Text = isOk ? "Upload succeeded" : cmd.ErrorDescription;
                lbMessageAdv.ForeColor = isOk ? Color.Black : Color.Red;
                _isChangeUploadedAdv = isOk;
                btnUploadRegistersAdv.Enabled = true;
                return isOk;
            }
            else
            {
                MessageBox.Show(this, "No registers are checked.  Nothing to upload.", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
        }

        private void btnRestoreDefaults_Click(object sender, System.EventArgs e)
        {
            cobBaudRate.SelectedIndex = getIndex(cobBaudRate, 115200);
            cobDataBits.SelectedIndex = getIndex(cobDataBits, 8);
            cobHandshake.SelectedIndex = getIndex(cobHandshake, (int)Handshake.None);
            cobParity.SelectedIndex = getIndex(cobParity, (int)Parity.None);
            cobStopBits.SelectedIndex = getIndex(cobStopBits, (int)StopBits.One);
        }

        private void OptionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if we did a test then ensure we save to force the COMPort to reopen
            if( !_hasSavedSettings && _hasDoneTest)
            {
                Settings.Default.COMPort = Settings.Default.COMPort;
                Settings.Default.Save();
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if( saveSettings() )
            {
                Close();
            }
        }

        private void setupComportDropdown()
        {
            string old = (string) cobCOMPorts.SelectedItem;
            if (old == null) old = Settings.Default.COMPort;
            cobCOMPorts.Items.Clear();
            cobCOMPorts.Items.AddRange(SerialPort.GetPortNames());
            cobCOMPorts.SelectedItem = old;
            setComPortName(old);
        }

        private void setComPortName(string old)
        {
            lbFriendlyName.Text = old != null && _friendlyNameByPort.ContainsKey(old) ? _friendlyNameByPort[old] : "";
        }

        private void cobCOMPorts_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            setComPortName((string) cobCOMPorts.SelectedItem);
        }

        private void btnRefreshList_Click(object sender, System.EventArgs e)
        {
            _friendlyNameByPort = SerialHelper.GetPorts();
            setupComportDropdown();
        }
    }
}