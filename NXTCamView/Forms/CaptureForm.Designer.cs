using NXTCamView.Controls;

namespace NXTCamView.Forms
{
    partial class CaptureForm
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
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.btnAbort = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.cbHighlightColors = new System.Windows.Forms.CheckBox();
            this.cbInterpolated = new System.Windows.Forms.CheckBox();
            this.hightlightTimer = new System.Windows.Forms.Timer(this.components);
            this.pbBayer = new TransparentPictureBox();
            this.pbInterpolated = new TransparentPictureBox();
            this.pnlProgress.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInterpolated)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(90, 6);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(96, 23);
            this.pbProgress.TabIndex = 4;
            // 
            // pnlProgress
            // 
            this.pnlProgress.BackColor = System.Drawing.SystemColors.Control;
            this.pnlProgress.Controls.Add(this.btnAbort);
            this.pnlProgress.Controls.Add(this.pbProgress);
            this.pnlProgress.Controls.Add(this.lbStatus);
            this.pnlProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlProgress.Location = new System.Drawing.Point(0, 229);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(517, 34);
            this.pnlProgress.TabIndex = 6;
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.Location = new System.Drawing.Point(430, 6);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 7;
            this.btnAbort.TabStop = false;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                          | System.Windows.Forms.AnchorStyles.Left)
                                                                         | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStatus.Location = new System.Drawing.Point(12, -2);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(412, 34);
            this.lbStatus.TabIndex = 6;
            this.lbStatus.Text = "Capturing";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.cbHighlightColors);
            this.pnlBottom.Controls.Add(this.cbInterpolated);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 200);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(517, 29);
            this.pnlBottom.TabIndex = 7;
            // 
            // cbHighlightColors
            // 
            this.cbHighlightColors.AutoSize = true;
            this.cbHighlightColors.Checked = global::NXTCamView.Properties.Settings.Default.ShowTrackNumber;
            this.cbHighlightColors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHighlightColors.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::NXTCamView.Properties.Settings.Default, "ShowTrackNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbHighlightColors.Location = new System.Drawing.Point(12, 6);
            this.cbHighlightColors.Name = "cbHighlightColors";
            this.cbHighlightColors.Size = new System.Drawing.Size(99, 17);
            this.cbHighlightColors.TabIndex = 9;
            this.cbHighlightColors.Text = "Highlight Colors";
            this.cbHighlightColors.UseVisualStyleBackColor = true;
            this.cbHighlightColors.CheckedChanged += new System.EventHandler(this.cbHighlightColors_CheckedChanged);
            // 
            // cbInterpolated
            // 
            this.cbInterpolated.AutoSize = true;
            this.cbInterpolated.Checked = true;
            this.cbInterpolated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInterpolated.Location = new System.Drawing.Point(120, 6);
            this.cbInterpolated.Name = "cbInterpolated";
            this.cbInterpolated.Size = new System.Drawing.Size(82, 17);
            this.cbInterpolated.TabIndex = 8;
            this.cbInterpolated.Text = "Interpolated";
            this.cbInterpolated.UseVisualStyleBackColor = true;
            this.cbInterpolated.CheckedChanged += new System.EventHandler(this.cbInterpolated_CheckedChanged);
            // 
            // hightlightTimer
            // 
            this.hightlightTimer.Interval = 500;
            this.hightlightTimer.Tick += new System.EventHandler(this.hightlightTimer_Tick);
            // 
            // pbBayer
            // 
            this.pbBayer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbBayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbBayer.HighlightColor = System.Drawing.Color.Yellow;
            this.pbBayer.Location = new System.Drawing.Point(0, 0);
            this.pbBayer.Margin = new System.Windows.Forms.Padding(0);
            this.pbBayer.MinimumSize = new System.Drawing.Size(176, 144);
            this.pbBayer.Name = "pbBayer";
            this.pbBayer.Size = new System.Drawing.Size(517, 229);
            this.pbBayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBayer.TabIndex = 1;
            this.pbBayer.TabStop = false;
            this.pbBayer.TransarentColorHigh = System.Drawing.Color.Empty;
            this.pbBayer.TransarentColorLow = System.Drawing.Color.Empty;
            this.pbBayer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_MouseMove);
            // 
            // pbInterpolated
            // 
            this.pbInterpolated.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbInterpolated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbInterpolated.HighlightColor = System.Drawing.Color.Yellow;
            this.pbInterpolated.Location = new System.Drawing.Point(0, 0);
            this.pbInterpolated.Margin = new System.Windows.Forms.Padding(0);
            this.pbInterpolated.MinimumSize = new System.Drawing.Size(176, 144);
            this.pbInterpolated.Name = "pbInterpolated";
            this.pbInterpolated.Size = new System.Drawing.Size(517, 229);
            this.pbInterpolated.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbInterpolated.TabIndex = 1;
            this.pbInterpolated.TabStop = false;
            this.pbInterpolated.TransarentColorHigh = System.Drawing.Color.Empty;
            this.pbInterpolated.TransarentColorLow = System.Drawing.Color.Empty;
            this.pbInterpolated.MouseLeave += new System.EventHandler(this.pb_MouseLeave);
            this.pbInterpolated.Click += new System.EventHandler(this.pbInterpolated_Click);
            this.pbInterpolated.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_MouseMove);
            // 
            // CaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(517, 263);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pbBayer);
            this.Controls.Add(this.pbInterpolated);
            this.Controls.Add(this.pnlProgress);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptureForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CaptureForm";
            this.Resize += new System.EventHandler(this.captureForm_Resize);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.captureForm_KeyUpDown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.captureForm_KeyUpDown);
            this.Load += new System.EventHandler(this.CaptureForm_Load);
            this.pnlProgress.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInterpolated)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TransparentPictureBox pbBayer;
        private System.Windows.Forms.ProgressBar pbProgress;
        private TransparentPictureBox pbInterpolated;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.CheckBox cbInterpolated;
        private System.Windows.Forms.CheckBox cbHighlightColors;
        private System.Windows.Forms.Timer hightlightTimer;
    }
}