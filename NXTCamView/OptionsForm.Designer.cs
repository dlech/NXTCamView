namespace NXTCamView
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tcOptions = new System.Windows.Forms.TabControl();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cobHandshake = new System.Windows.Forms.ComboBox();
            this.cobStopBits = new System.Windows.Forms.ComboBox();
            this.cobDataBits = new System.Windows.Forms.ComboBox();
            this.cobParity = new System.Windows.Forms.ComboBox();
            this.cobBaudRate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.Label();
            this.btnRestoreDefaults = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cobCOMPorts = new System.Windows.Forms.ComboBox();
            this.tabNXTCamSettings = new System.Windows.Forms.TabPage();
            this.lbMessage = new System.Windows.Forms.Label();
            this.btnUploadRegisters = new System.Windows.Forms.Button();
            this.cbUseFlourescentLightFilter = new System.Windows.Forms.CheckBox();
            this.cbUseAutoAdjust = new System.Windows.Forms.CheckBox();
            this.cbUseAutoWhiteBalance = new System.Windows.Forms.CheckBox();
            this.tabNXTCamSettingsAdv = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.btnUploadRegistersAdv = new System.Windows.Forms.Button();
            this.lbMessageAdv = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tcOptions.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.tabNXTCamSettings.SuspendLayout();
            this.tabNXTCamSettingsAdv.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcOptions
            // 
            this.tcOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcOptions.Controls.Add(this.tabConnection);
            this.tcOptions.Controls.Add(this.tabNXTCamSettings);
            this.tcOptions.Controls.Add(this.tabNXTCamSettingsAdv);
            this.tcOptions.Location = new System.Drawing.Point(14, 12);
            this.tcOptions.Name = "tcOptions";
            this.tcOptions.SelectedIndex = 0;
            this.tcOptions.Size = new System.Drawing.Size(363, 364);
            this.tcOptions.TabIndex = 0;
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.panel1);
            this.tabConnection.Controls.Add(this.cobHandshake);
            this.tabConnection.Controls.Add(this.cobStopBits);
            this.tabConnection.Controls.Add(this.cobDataBits);
            this.tabConnection.Controls.Add(this.cobParity);
            this.tabConnection.Controls.Add(this.cobBaudRate);
            this.tabConnection.Controls.Add(this.label2);
            this.tabConnection.Controls.Add(this.label7);
            this.tabConnection.Controls.Add(this.label6);
            this.tabConnection.Controls.Add(this.label4);
            this.tabConnection.Controls.Add(this.label8);
            this.tabConnection.Controls.Add(this.label3);
            this.tabConnection.Controls.Add(this.lbResult);
            this.tabConnection.Controls.Add(this.btnRestoreDefaults);
            this.tabConnection.Controls.Add(this.btnTest);
            this.tabConnection.Controls.Add(this.label1);
            this.tabConnection.Controls.Add(this.cobCOMPorts);
            this.tabConnection.Location = new System.Drawing.Point(4, 22);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnection.Size = new System.Drawing.Size(355, 338);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Connection";
            this.tabConnection.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(78, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(261, 1);
            this.panel1.TabIndex = 8;
            // 
            // cobHandshake
            // 
            this.cobHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobHandshake.FormattingEnabled = true;
            this.cobHandshake.Location = new System.Drawing.Point(113, 91);
            this.cobHandshake.Name = "cobHandshake";
            this.cobHandshake.Size = new System.Drawing.Size(89, 21);
            this.cobHandshake.TabIndex = 7;
            // 
            // cobStopBits
            // 
            this.cobStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobStopBits.FormattingEnabled = true;
            this.cobStopBits.Location = new System.Drawing.Point(113, 174);
            this.cobStopBits.Name = "cobStopBits";
            this.cobStopBits.Size = new System.Drawing.Size(89, 21);
            this.cobStopBits.TabIndex = 7;
            // 
            // cobDataBits
            // 
            this.cobDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobDataBits.FormattingEnabled = true;
            this.cobDataBits.Location = new System.Drawing.Point(113, 147);
            this.cobDataBits.Name = "cobDataBits";
            this.cobDataBits.Size = new System.Drawing.Size(89, 21);
            this.cobDataBits.TabIndex = 7;
            // 
            // cobParity
            // 
            this.cobParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobParity.FormattingEnabled = true;
            this.cobParity.Location = new System.Drawing.Point(113, 118);
            this.cobParity.Name = "cobParity";
            this.cobParity.Size = new System.Drawing.Size(89, 21);
            this.cobParity.TabIndex = 7;
            // 
            // cobBaudRate
            // 
            this.cobBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobBaudRate.FormattingEnabled = true;
            this.cobBaudRate.Location = new System.Drawing.Point(113, 64);
            this.cobBaudRate.Name = "cobBaudRate";
            this.cobBaudRate.Size = new System.Drawing.Size(89, 21);
            this.cobBaudRate.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Stop bits:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Handshake:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Data bits:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Parity:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Settings:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Baud:";
            // 
            // lbResult
            // 
            this.lbResult.Location = new System.Drawing.Point(18, 296);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(320, 30);
            this.lbResult.TabIndex = 4;
            this.lbResult.Text = "test";
            // 
            // btnRestoreDefaults
            // 
            this.btnRestoreDefaults.Location = new System.Drawing.Point(18, 208);
            this.btnRestoreDefaults.Name = "btnRestoreDefaults";
            this.btnRestoreDefaults.Size = new System.Drawing.Size(89, 23);
            this.btnRestoreDefaults.TabIndex = 3;
            this.btnRestoreDefaults.Text = "Restore All";
            this.toolTip.SetToolTip(this.btnRestoreDefaults, "Restore the default COMPort settings");
            this.btnRestoreDefaults.UseVisualStyleBackColor = true;
            this.btnRestoreDefaults.Click += new System.EventHandler(this.btnRestoreDefaults_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(18, 270);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(89, 23);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "Test";
            this.toolTip.SetToolTip(this.btnTest, "Test this COMPort and its settings");
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Comport:";
            // 
            // cobCOMPorts
            // 
            this.cobCOMPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobCOMPorts.FormattingEnabled = true;
            this.cobCOMPorts.Location = new System.Drawing.Point(113, 14);
            this.cobCOMPorts.Name = "cobCOMPorts";
            this.cobCOMPorts.Size = new System.Drawing.Size(89, 21);
            this.cobCOMPorts.TabIndex = 1;
            // 
            // tabNXTCamSettings
            // 
            this.tabNXTCamSettings.Controls.Add(this.lbMessage);
            this.tabNXTCamSettings.Controls.Add(this.btnUploadRegisters);
            this.tabNXTCamSettings.Controls.Add(this.cbUseFlourescentLightFilter);
            this.tabNXTCamSettings.Controls.Add(this.cbUseAutoAdjust);
            this.tabNXTCamSettings.Controls.Add(this.cbUseAutoWhiteBalance);
            this.tabNXTCamSettings.Location = new System.Drawing.Point(4, 22);
            this.tabNXTCamSettings.Name = "tabNXTCamSettings";
            this.tabNXTCamSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabNXTCamSettings.Size = new System.Drawing.Size(355, 338);
            this.tabNXTCamSettings.TabIndex = 1;
            this.tabNXTCamSettings.Text = "NXTCam Settings";
            this.tabNXTCamSettings.UseVisualStyleBackColor = true;
            // 
            // lbMessage
            // 
            this.lbMessage.Location = new System.Drawing.Point(14, 140);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(317, 93);
            this.lbMessage.TabIndex = 5;
            this.lbMessage.Text = "result";
            // 
            // btnUploadRegisters
            // 
            this.btnUploadRegisters.Location = new System.Drawing.Point(17, 103);
            this.btnUploadRegisters.Name = "btnUploadRegisters";
            this.btnUploadRegisters.Size = new System.Drawing.Size(105, 23);
            this.btnUploadRegisters.TabIndex = 1;
            this.btnUploadRegisters.Text = "Upload Settings";
            this.btnUploadRegisters.UseVisualStyleBackColor = true;
            this.btnUploadRegisters.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // cbUseFlourescentLightFilter
            // 
            this.cbUseFlourescentLightFilter.AutoSize = true;
            this.cbUseFlourescentLightFilter.Location = new System.Drawing.Point(17, 64);
            this.cbUseFlourescentLightFilter.Name = "cbUseFlourescentLightFilter";
            this.cbUseFlourescentLightFilter.Size = new System.Drawing.Size(172, 17);
            this.cbUseFlourescentLightFilter.TabIndex = 0;
            this.cbUseFlourescentLightFilter.Text = "Use flourescent light filter";
            this.cbUseFlourescentLightFilter.UseVisualStyleBackColor = true;
            this.cbUseFlourescentLightFilter.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbUseAutoAdjust
            // 
            this.cbUseAutoAdjust.AutoSize = true;
            this.cbUseAutoAdjust.Location = new System.Drawing.Point(17, 41);
            this.cbUseAutoAdjust.Name = "cbUseAutoAdjust";
            this.cbUseAutoAdjust.Size = new System.Drawing.Size(149, 17);
            this.cbUseAutoAdjust.TabIndex = 0;
            this.cbUseAutoAdjust.Text = "Use AutoAdjust mode";
            this.cbUseAutoAdjust.UseVisualStyleBackColor = true;
            this.cbUseAutoAdjust.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbUseAutoWhiteBalance
            // 
            this.cbUseAutoWhiteBalance.AutoSize = true;
            this.cbUseAutoWhiteBalance.Location = new System.Drawing.Point(17, 18);
            this.cbUseAutoWhiteBalance.Name = "cbUseAutoWhiteBalance";
            this.cbUseAutoWhiteBalance.Size = new System.Drawing.Size(190, 17);
            this.cbUseAutoWhiteBalance.TabIndex = 0;
            this.cbUseAutoWhiteBalance.Text = "Use AutoWhiteBalance mode";
            this.cbUseAutoWhiteBalance.UseVisualStyleBackColor = true;
            this.cbUseAutoWhiteBalance.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // tabNXTCamSettingsAdv
            // 
            this.tabNXTCamSettingsAdv.Controls.Add(this.label5);
            this.tabNXTCamSettingsAdv.Controls.Add(this.btnUploadRegistersAdv);
            this.tabNXTCamSettingsAdv.Controls.Add(this.lbMessageAdv);
            this.tabNXTCamSettingsAdv.Location = new System.Drawing.Point(4, 22);
            this.tabNXTCamSettingsAdv.Name = "tabNXTCamSettingsAdv";
            this.tabNXTCamSettingsAdv.Padding = new System.Windows.Forms.Padding(3);
            this.tabNXTCamSettingsAdv.Size = new System.Drawing.Size(355, 338);
            this.tabNXTCamSettingsAdv.TabIndex = 2;
            this.tabNXTCamSettingsAdv.Text = "Advanced";
            this.tabNXTCamSettingsAdv.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(14, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(317, 19);
            this.label5.TabIndex = 6;
            this.label5.Text = "NXTCam registers:";
            // 
            // btnUploadRegistersAdv
            // 
            this.btnUploadRegistersAdv.Location = new System.Drawing.Point(17, 258);
            this.btnUploadRegistersAdv.Name = "btnUploadRegistersAdv";
            this.btnUploadRegistersAdv.Size = new System.Drawing.Size(114, 23);
            this.btnUploadRegistersAdv.TabIndex = 2;
            this.btnUploadRegistersAdv.Text = "Upload Settings";
            this.btnUploadRegistersAdv.UseVisualStyleBackColor = true;
            this.btnUploadRegistersAdv.Click += new System.EventHandler(this.btnUploadAdvanced_Click);
            // 
            // lbMessageAdv
            // 
            this.lbMessageAdv.Location = new System.Drawing.Point(14, 290);
            this.lbMessageAdv.Name = "lbMessageAdv";
            this.lbMessageAdv.Size = new System.Drawing.Size(317, 35);
            this.lbMessageAdv.TabIndex = 6;
            this.lbMessageAdv.Text = "result";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(289, 382);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(195, 382);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(391, 417);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tcOptions);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionsForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tcOptions.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.tabNXTCamSettings.ResumeLayout(false);
            this.tabNXTCamSettings.PerformLayout();
            this.tabNXTCamSettingsAdv.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcOptions;
        private System.Windows.Forms.TabPage tabConnection;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cobCOMPorts;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label lbResult;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.TabPage tabNXTCamSettings;
        private System.Windows.Forms.CheckBox cbUseAutoWhiteBalance;
        private System.Windows.Forms.CheckBox cbUseAutoAdjust;
        private System.Windows.Forms.CheckBox cbUseFlourescentLightFilter;
        private System.Windows.Forms.Button btnUploadRegisters;
        private System.Windows.Forms.Label lbMessage;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage tabNXTCamSettingsAdv;
        private System.Windows.Forms.Label lbMessageAdv;
        private System.Windows.Forms.Button btnUploadRegistersAdv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cobParity;
        private System.Windows.Forms.ComboBox cobBaudRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cobStopBits;
        private System.Windows.Forms.ComboBox cobDataBits;
        private System.Windows.Forms.ComboBox cobHandshake;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRestoreDefaults;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
    }
}