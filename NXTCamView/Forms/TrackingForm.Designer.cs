using NXTCamView.Controls;

namespace NXTCamView.Forms
{
    partial class TrackingForm
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
                _penBlack.Dispose();
                foreach( TrackingData trackingData in _trackingData )
                {
                    trackingData.Dispose();
                }
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInRangeTracked = new System.Windows.Forms.Label();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.lblMatchesPerFrame = new System.Windows.Forms.Label();
            this.lblOutOfRangeTracked = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lvColors = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chColor = new System.Windows.Forms.ColumnHeader();
            this.chColorMatchsTotal = new System.Windows.Forms.ColumnHeader();
            this.chColorMatchesFiltered = new System.Windows.Forms.ColumnHeader();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTotalTracked = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblUploadTimestamp = new System.Windows.Forms.Label();
            this.cbArea = new System.Windows.Forms.CheckBox();
            this.cbColorNumber = new System.Windows.Forms.CheckBox();
            this.cbSize = new System.Windows.Forms.CheckBox();
            this.cbLocation = new System.Windows.Forms.CheckBox();
            this.cbUseColor = new System.Windows.Forms.CheckBox();
            this.cbSolid = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlDetailColor = new System.Windows.Forms.Panel();
            this.btnSetDetailColor = new System.Windows.Forms.Button();
            this.nudAreaFilter = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.paintTimer = new System.Windows.Forms.Timer(this.components);
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.llShowHideDetail = new System.Windows.Forms.LinkLabel();
            this.cbtnPause = new CommandButton();
            this.cbtnStart = new CommandButton();
            this.cbtnStop = new CommandButton();
            this.pnlTrackedColors = new DoubleBufferedPanel();
            this.lblDummyDetail = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaFilter)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.pnlTrackedColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(124, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Frame count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Filtered Out:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Avg frame:";
            // 
            // lblInRangeTracked
            // 
            this.lblInRangeTracked.AutoSize = true;
            this.lblInRangeTracked.Location = new System.Drawing.Point(84, 40);
            this.lblInRangeTracked.Name = "lblInRangeTracked";
            this.lblInRangeTracked.Size = new System.Drawing.Size(14, 13);
            this.lblInRangeTracked.TabIndex = 2;
            this.lblInRangeTracked.Text = "0";
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.AutoSize = true;
            this.lblFrameCount.Location = new System.Drawing.Point(211, 22);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(14, 13);
            this.lblFrameCount.TabIndex = 2;
            this.lblFrameCount.Text = "0";
            // 
            // lblMatchesPerFrame
            // 
            this.lblMatchesPerFrame.AutoSize = true;
            this.lblMatchesPerFrame.Location = new System.Drawing.Point(211, 40);
            this.lblMatchesPerFrame.Name = "lblMatchesPerFrame";
            this.lblMatchesPerFrame.Size = new System.Drawing.Size(14, 13);
            this.lblMatchesPerFrame.TabIndex = 2;
            this.lblMatchesPerFrame.Text = "0";
            // 
            // lblOutOfRangeTracked
            // 
            this.lblOutOfRangeTracked.AutoSize = true;
            this.lblOutOfRangeTracked.Location = new System.Drawing.Point(84, 58);
            this.lblOutOfRangeTracked.Name = "lblOutOfRangeTracked";
            this.lblOutOfRangeTracked.Size = new System.Drawing.Size(14, 13);
            this.lblOutOfRangeTracked.TabIndex = 2;
            this.lblOutOfRangeTracked.Text = "0";
            // 
            // lblMessage
            // 
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(64, 440);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(282, 30);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvColors
            // 
            this.lvColors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                       this.chName,
                                                                                       this.chColor,
                                                                                       this.chColorMatchsTotal,
                                                                                       this.chColorMatchesFiltered});
            this.lvColors.GridLines = true;
            this.lvColors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvColors.Location = new System.Drawing.Point(18, 20);
            this.lvColors.Name = "lvColors";
            this.lvColors.Size = new System.Drawing.Size(229, 134);
            this.lvColors.TabIndex = 7;
            this.lvColors.UseCompatibleStateImageBehavior = false;
            this.lvColors.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "No.";
            this.chName.Width = 35;
            // 
            // chColor
            // 
            this.chColor.Text = "Color";
            this.chColor.Width = 55;
            // 
            // chColorMatchsTotal
            // 
            this.chColorMatchsTotal.Text = "Total";
            this.chColorMatchsTotal.Width = 65;
            // 
            // chColorMatchesFiltered
            // 
            this.chColorMatchesFiltered.Text = "Filtered";
            this.chColorMatchesFiltered.Width = 65;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Filtered In:";
            // 
            // lblTotalTracked
            // 
            this.lblTotalTracked.AutoSize = true;
            this.lblTotalTracked.Location = new System.Drawing.Point(84, 22);
            this.lblTotalTracked.Name = "lblTotalTracked";
            this.lblTotalTracked.Size = new System.Drawing.Size(14, 13);
            this.lblTotalTracked.TabIndex = 2;
            this.lblTotalTracked.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Total:";
            // 
            // lblUploadTimestamp
            // 
            this.lblUploadTimestamp.Location = new System.Drawing.Point(22, 153);
            this.lblUploadTimestamp.Name = "lblUploadTimestamp";
            this.lblUploadTimestamp.Size = new System.Drawing.Size(225, 20);
            this.lblUploadTimestamp.TabIndex = 2;
            this.lblUploadTimestamp.Text = "time";
            this.lblUploadTimestamp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbArea
            // 
            this.cbArea.AutoSize = true;
            this.cbArea.Checked = global::NXTCamView.Properties.Settings.Default.ShowTrackArea;
            this.cbArea.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbArea.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "ShowTrackArea", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbArea.Location = new System.Drawing.Point(149, 41);
            this.cbArea.Name = "cbArea";
            this.cbArea.Size = new System.Drawing.Size(53, 17);
            this.cbArea.TabIndex = 4;
            this.cbArea.Text = "Area";
            this.toolTip1.SetToolTip(this.cbArea, "Show the area of the tracked blob");
            this.cbArea.UseVisualStyleBackColor = true;
            this.cbArea.Click += new System.EventHandler(this.cb_Click);
            // 
            // cbColorNumber
            // 
            this.cbColorNumber.AutoSize = true;
            this.cbColorNumber.Checked = global::NXTCamView.Properties.Settings.Default.ShowTrackNumber;
            this.cbColorNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColorNumber.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "ShowTrackNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbColorNumber.Location = new System.Drawing.Point(149, 20);
            this.cbColorNumber.Name = "cbColorNumber";
            this.cbColorNumber.Size = new System.Drawing.Size(80, 17);
            this.cbColorNumber.TabIndex = 4;
            this.cbColorNumber.Text = "Color No.";
            this.toolTip1.SetToolTip(this.cbColorNumber, "Show the number of the tracked blob");
            this.cbColorNumber.UseVisualStyleBackColor = true;
            this.cbColorNumber.Click += new System.EventHandler(this.cb_Click);
            // 
            // cbSize
            // 
            this.cbSize.AutoSize = true;
            this.cbSize.Checked = global::NXTCamView.Properties.Settings.Default.ShowTrackSize;
            this.cbSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSize.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "ShowTrackSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbSize.Location = new System.Drawing.Point(91, 41);
            this.cbSize.Name = "cbSize";
            this.cbSize.Size = new System.Drawing.Size(50, 17);
            this.cbSize.TabIndex = 4;
            this.cbSize.Text = "Size";
            this.toolTip1.SetToolTip(this.cbSize, "Show the height/width of the tracked blob");
            this.cbSize.UseVisualStyleBackColor = true;
            this.cbSize.Click += new System.EventHandler(this.cb_Click);
            // 
            // cbLocation
            // 
            this.cbLocation.AutoSize = true;
            this.cbLocation.Checked = global::NXTCamView.Properties.Settings.Default.ShowTrackLocation;
            this.cbLocation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLocation.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "ShowTrackLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbLocation.Location = new System.Drawing.Point(12, 43);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(73, 17);
            this.cbLocation.TabIndex = 4;
            this.cbLocation.Text = "Location";
            this.toolTip1.SetToolTip(this.cbLocation, "Show the x/y location of the tracked blob");
            this.cbLocation.UseVisualStyleBackColor = true;
            // 
            // cbUseColor
            // 
            this.cbUseColor.AutoSize = true;
            this.cbUseColor.Checked = global::NXTCamView.Properties.Settings.Default.TrackInColor;
            this.cbUseColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseColor.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "TrackInColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbUseColor.Location = new System.Drawing.Point(12, 20);
            this.cbUseColor.Name = "cbUseColor";
            this.cbUseColor.Size = new System.Drawing.Size(57, 17);
            this.cbUseColor.TabIndex = 4;
            this.cbUseColor.Text = "Color";
            this.toolTip1.SetToolTip(this.cbUseColor, "Show the tracked blob in color");
            this.cbUseColor.UseVisualStyleBackColor = true;
            this.cbUseColor.Click += new System.EventHandler(this.cb_Click);
            // 
            // cbSolid
            // 
            this.cbSolid.AutoSize = true;
            this.cbSolid.Checked = global::NXTCamView.Properties.Settings.Default.TrackSolid;
            this.cbSolid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSolid.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "TrackSolid", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbSolid.Location = new System.Drawing.Point(91, 20);
            this.cbSolid.Name = "cbSolid";
            this.cbSolid.Size = new System.Drawing.Size(54, 17);
            this.cbSolid.TabIndex = 4;
            this.cbSolid.Text = "Solid";
            this.toolTip1.SetToolTip(this.cbSolid, "Show the tracked blob as a solid rectangle");
            this.cbSolid.UseVisualStyleBackColor = true;
            this.cbSolid.Click += new System.EventHandler(this.cb_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.pnlDetailColor);
            this.groupBox1.Controls.Add(this.btnSetDetailColor);
            this.groupBox1.Controls.Add(this.cbLocation);
            this.groupBox1.Controls.Add(this.cbSolid);
            this.groupBox1.Controls.Add(this.cbUseColor);
            this.groupBox1.Controls.Add(this.cbArea);
            this.groupBox1.Controls.Add(this.cbSize);
            this.groupBox1.Controls.Add(this.cbColorNumber);
            this.groupBox1.Location = new System.Drawing.Point(12, 573);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 89);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Blob Details";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Detail color:";
            // 
            // pnlDetailColor
            // 
            this.pnlDetailColor.BackColor = global::NXTCamView.Properties.Settings.Default.TrackingDetailColor;
            this.pnlDetailColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetailColor.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::NXTCamView.Properties.Settings.Default, "TrackingDetailColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.pnlDetailColor.Location = new System.Drawing.Point(91, 63);
            this.pnlDetailColor.Name = "pnlDetailColor";
            this.pnlDetailColor.Size = new System.Drawing.Size(21, 19);
            this.pnlDetailColor.TabIndex = 7;
            this.pnlDetailColor.DoubleClick += new System.EventHandler(this.btnSetDetailColor_Click);
            // 
            // btnSetDetailColor
            // 
            this.btnSetDetailColor.Location = new System.Drawing.Point(118, 61);
            this.btnSetDetailColor.Name = "btnSetDetailColor";
            this.btnSetDetailColor.Size = new System.Drawing.Size(40, 23);
            this.btnSetDetailColor.TabIndex = 6;
            this.btnSetDetailColor.Text = "Set";
            this.btnSetDetailColor.UseVisualStyleBackColor = true;
            this.btnSetDetailColor.Click += new System.EventHandler(this.btnSetDetailColor_Click);
            // 
            // nudAreaFilter
            // 
            this.nudAreaFilter.Increment = new decimal(new int[] {
                                                                     50,
                                                                     0,
                                                                     0,
                                                                     0});
            this.nudAreaFilter.Location = new System.Drawing.Point(199, 56);
            this.nudAreaFilter.Maximum = new decimal(new int[] {
                                                                   10000,
                                                                   0,
                                                                   0,
                                                                   0});
            this.nudAreaFilter.Name = "nudAreaFilter";
            this.nudAreaFilter.Size = new System.Drawing.Size(55, 21);
            this.nudAreaFilter.TabIndex = 5;
            this.nudAreaFilter.ThousandsSeparator = true;
            this.toolTip1.SetToolTip(this.nudAreaFilter, "The minimum for a blob to be shown");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Area filter:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.nudAreaFilter);
            this.groupBox2.Controls.Add(this.lblInRangeTracked);
            this.groupBox2.Controls.Add(this.lblTotalTracked);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblFrameCount);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblOutOfRangeTracked);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblMatchesPerFrame);
            this.groupBox2.Location = new System.Drawing.Point(12, 481);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 86);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tracking Statistics";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lvColors);
            this.groupBox3.Controls.Add(this.lblUploadTimestamp);
            this.groupBox3.Location = new System.Drawing.Point(278, 481);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(262, 181);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Uploaded Colors";
            // 
            // paintTimer
            // 
            this.paintTimer.Interval = 250;
            this.paintTimer.Tick += new System.EventHandler(this.paintTimer_Tick);
            // 
            // colorDialog
            // 
            this.colorDialog.SolidColorOnly = true;
            // 
            // llShowHideDetail
            // 
            this.llShowHideDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llShowHideDetail.AutoSize = true;
            this.llShowHideDetail.Location = new System.Drawing.Point(12, 449);
            this.llShowHideDetail.Name = "llShowHideDetail";
            this.llShowHideDetail.Size = new System.Drawing.Size(32, 13);
            this.llShowHideDetail.TabIndex = 11;
            this.llShowHideDetail.TabStop = true;
            this.llShowHideDetail.Text = "Hide";
            this.llShowHideDetail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowHideDetail_LinkClicked);
            // 
            // cbtnPause
            // 
            this.cbtnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbtnPause.Command = null;
            this.cbtnPause.ExecutionType = ExecutionType.OnClickExecute;
            this.cbtnPause.Location = new System.Drawing.Point(418, 444);
            this.cbtnPause.Name = "cbtnPause";
            this.cbtnPause.Size = new System.Drawing.Size(60, 23);
            this.cbtnPause.TabIndex = 0;
            this.cbtnPause.Text = "Pause";
            this.cbtnPause.UseVisualStyleBackColor = true;
            this.cbtnPause.MouseLeave += new System.EventHandler(this.btnPause_MouseLeave);
            // 
            // cbtnStart
            // 
            this.cbtnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbtnStart.Command = null;
            this.cbtnStart.ExecutionType = ExecutionType.OnClickExecute;
            this.cbtnStart.Location = new System.Drawing.Point(352, 444);
            this.cbtnStart.Name = "cbtnStart";
            this.cbtnStart.Size = new System.Drawing.Size(60, 23);
            this.cbtnStart.TabIndex = 0;
            this.cbtnStart.Text = "Start";
            this.cbtnStart.UseVisualStyleBackColor = true;
            // 
            // cbtnStop
            // 
            this.cbtnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbtnStop.Command = null;
            this.cbtnStop.ExecutionType = ExecutionType.OnClickExecute;
            this.cbtnStop.Location = new System.Drawing.Point(484, 444);
            this.cbtnStop.Name = "cbtnStop";
            this.cbtnStop.Size = new System.Drawing.Size(60, 23);
            this.cbtnStop.TabIndex = 0;
            this.cbtnStop.Text = "Stop";
            this.cbtnStop.UseVisualStyleBackColor = true;
            // 
            // pnlTrackedColors
            // 
            this.pnlTrackedColors.BackColor = System.Drawing.Color.White;
            this.pnlTrackedColors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTrackedColors.Controls.Add(this.lblDummyDetail);
            this.pnlTrackedColors.Location = new System.Drawing.Point(14, 6);
            this.pnlTrackedColors.Name = "pnlTrackedColors";
            this.pnlTrackedColors.Size = new System.Drawing.Size(528, 430);
            this.pnlTrackedColors.TabIndex = 0;
            // 
            // lblDummyDetail
            // 
            this.lblDummyDetail.AutoSize = true;
            this.lblDummyDetail.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDummyDetail.Location = new System.Drawing.Point(432, 402);
            this.lblDummyDetail.Name = "lblDummyDetail";
            this.lblDummyDetail.Size = new System.Drawing.Size(91, 16);
            this.lblDummyDetail.TabIndex = 0;
            this.lblDummyDetail.Text = "dummyDetail";
            this.lblDummyDetail.Visible = false;
            // 
            // TrackingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 673);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.llShowHideDetail);
            this.Controls.Add(this.cbtnPause);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbtnStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbtnStop);
            this.Controls.Add(this.pnlTrackedColors);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrackingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tracking";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.trackingFormFormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackingFormKeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackingFormKeyDown);
            this.Load += new System.EventHandler(this.trackingFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaFilter)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.pnlTrackedColors.ResumeLayout(false);
            this.pnlTrackedColors.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedPanel pnlTrackedColors;
        private CommandButton cbtnStart;
        private CommandButton cbtnStop;
        private CommandButton cbtnPause;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblInRangeTracked;
        private System.Windows.Forms.Label lblFrameCount;
        private System.Windows.Forms.Label lblMatchesPerFrame;
        private System.Windows.Forms.Label lblOutOfRangeTracked;
        private System.Windows.Forms.CheckBox cbSolid;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ListView lvColors;
        private System.Windows.Forms.ColumnHeader chColorMatchsTotal;
        private System.Windows.Forms.ColumnHeader chColor;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.CheckBox cbUseColor;
        private System.Windows.Forms.ColumnHeader chColorMatchesFiltered;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTotalTracked;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblUploadTimestamp;
        private System.Windows.Forms.CheckBox cbLocation;
        private System.Windows.Forms.CheckBox cbSize;
        private System.Windows.Forms.CheckBox cbColorNumber;
        private System.Windows.Forms.CheckBox cbArea;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudAreaFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDummyDetail;
        private System.Windows.Forms.Timer paintTimer;
        private System.Windows.Forms.Button btnSetDetailColor;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlDetailColor;
        private System.Windows.Forms.LinkLabel llShowHideDetail;
    }
}