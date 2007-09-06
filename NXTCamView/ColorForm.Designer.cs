namespace NXTCamView
{
    partial class ColorForm
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
            this.pnlColorMain = new System.Windows.Forms.Panel();
            this.cbHighLight = new System.Windows.Forms.CheckBox();
            this.cbGotoNext = new System.Windows.Forms.CheckBox();
            this.pnlColorPanels = new System.Windows.Forms.Panel();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.pnlSample = new System.Windows.Forms.Panel();
            this.llColorDetail = new System.Windows.Forms.LinkLabel();
            this.pnlActiveColor = new System.Windows.Forms.Panel();
            this.lbActiveColor = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipPanel = new System.Windows.Forms.ToolTip(this.components);
            this.pnlColorMain.SuspendLayout();
            this.pnlColorPanels.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlColorMain
            // 
            this.pnlColorMain.Controls.Add(this.pnlActiveColor);
            this.pnlColorMain.Controls.Add(this.cbHighLight);
            this.pnlColorMain.Controls.Add(this.cbGotoNext);
            this.pnlColorMain.Controls.Add(this.pnlColorPanels);
            this.pnlColorMain.Controls.Add(this.llColorDetail);
            this.pnlColorMain.Controls.Add(this.lbActiveColor);
            this.pnlColorMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlColorMain.Location = new System.Drawing.Point(0, 0);
            this.pnlColorMain.Name = "pnlColorMain";
            this.pnlColorMain.Size = new System.Drawing.Size(468, 67);
            this.pnlColorMain.TabIndex = 6;
            // 
            // cbHighLight
            // 
            this.cbHighLight.AutoSize = true;
            this.cbHighLight.Checked = global::NXTCamView.Properties.Settings.Default.HighlightColors;
            this.cbHighLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHighLight.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "HighlightColors", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbHighLight.Location = new System.Drawing.Point(255, 44);
            this.cbHighLight.Name = "cbHighLight";
            this.cbHighLight.Size = new System.Drawing.Size(75, 17);
            this.cbHighLight.TabIndex = 9;
            this.cbHighLight.Text = "Highlight";
            this.toolTip.SetToolTip(this.cbHighLight, "Hightlight the color range in the capture when selecting a color");
            this.cbHighLight.UseVisualStyleBackColor = true;
            this.cbHighLight.CheckedChanged += new System.EventHandler(this.cbHighLight_CheckedChanged);
            // 
            // cbGotoNext
            // 
            this.cbGotoNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbGotoNext.AutoSize = true;
            this.cbGotoNext.Checked = true;
            this.cbGotoNext.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGotoNext.Location = new System.Drawing.Point(336, 44);
            this.cbGotoNext.Name = "cbGotoNext";
            this.cbGotoNext.Size = new System.Drawing.Size(82, 17);
            this.cbGotoNext.TabIndex = 7;
            this.cbGotoNext.Text = "Goto next";
            this.cbGotoNext.UseVisualStyleBackColor = true;
            // 
            // pnlColorPanels
            // 
            this.pnlColorPanels.BackColor = System.Drawing.SystemColors.Control;
            this.pnlColorPanels.Controls.Add(this.btnClearAll);
            this.pnlColorPanels.Controls.Add(this.btnClear);
            this.pnlColorPanels.Controls.Add(this.btnUpload);
            this.pnlColorPanels.Controls.Add(this.pnlSample);
            this.pnlColorPanels.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlColorPanels.Location = new System.Drawing.Point(0, 0);
            this.pnlColorPanels.Name = "pnlColorPanels";
            this.pnlColorPanels.Size = new System.Drawing.Size(249, 67);
            this.pnlColorPanels.TabIndex = 8;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearAll.Location = new System.Drawing.Point(167, 40);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(70, 23);
            this.btnClearAll.TabIndex = 10;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(90, 40);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 23);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpload.Location = new System.Drawing.Point(13, 40);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(70, 23);
            this.btnUpload.TabIndex = 10;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // pnlSample
            // 
            this.pnlSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSample.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlSample.Location = new System.Drawing.Point(224, 9);
            this.pnlSample.Name = "pnlSample";
            this.pnlSample.Size = new System.Drawing.Size(22, 21);
            this.pnlSample.TabIndex = 3;
            this.pnlSample.Tag = "8";
            // 
            // llColorDetail
            // 
            this.llColorDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.llColorDetail.AutoSize = true;
            this.llColorDetail.Location = new System.Drawing.Point(424, 45);
            this.llColorDetail.Name = "llColorDetail";
            this.llColorDetail.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.llColorDetail.Size = new System.Drawing.Size(32, 13);
            this.llColorDetail.TabIndex = 6;
            this.llColorDetail.TabStop = true;
            this.llColorDetail.Text = "Hide";
            this.llColorDetail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llColorDetail_LinkClicked);
            // 
            // pnlActiveColor
            // 
            this.pnlActiveColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlActiveColor.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlActiveColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlActiveColor.Location = new System.Drawing.Point(255, 9);
            this.pnlActiveColor.Name = "pnlActiveColor";
            this.pnlActiveColor.Size = new System.Drawing.Size(29, 27);
            this.pnlActiveColor.TabIndex = 3;
            // 
            // lbActiveColor
            // 
            this.lbActiveColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbActiveColor.AutoSize = true;
            this.lbActiveColor.Location = new System.Drawing.Point(295, 17);
            this.lbActiveColor.Name = "lbActiveColor";
            this.lbActiveColor.Size = new System.Drawing.Size(35, 13);
            this.lbActiveColor.TabIndex = 5;
            this.lbActiveColor.Text = "color";
            // 
            // toolTipPanel
            // 
            this.toolTipPanel.IsBalloon = true;
            this.toolTipPanel.ShowAlways = true;
            this.toolTipPanel.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.toolTipPanel.ToolTipTitle = "Color Overlaps";
            // 
            // ColorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 71);
            this.Controls.Add(this.pnlColorMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Colors";
            this.SizeChanged += new System.EventHandler(this.ColorForm_SizeChanged);
            this.Move += new System.EventHandler(this.ColorForm_Move);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorForm_FormClosing);
            this.Load += new System.EventHandler(this.ColorForm_Load);
            this.pnlColorMain.ResumeLayout(false);
            this.pnlColorMain.PerformLayout();
            this.pnlColorPanels.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlColorMain;
        private System.Windows.Forms.LinkLabel llColorDetail;
        private System.Windows.Forms.Panel pnlActiveColor;
        private System.Windows.Forms.Label lbActiveColor;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolTip toolTipPanel;
        private System.Windows.Forms.Panel pnlColorPanels;
        private System.Windows.Forms.CheckBox cbGotoNext;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Panel pnlSample;
        private System.Windows.Forms.CheckBox cbHighLight;
    }
}