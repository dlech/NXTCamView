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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorForm));
            this.pnlColorMain = new System.Windows.Forms.Panel();
            this.pnlActiveColor = new System.Windows.Forms.Panel();
            this.pnlColorPanels = new System.Windows.Forms.Panel();
            this.pnlSample = new System.Windows.Forms.Panel();
            this.lbActiveColor = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipPanel = new System.Windows.Forms.ToolTip(this.components);
            this.tsToolBar = new System.Windows.Forms.ToolStrip();
            this.tsbSetColor = new System.Windows.Forms.ToolStripButton();
            this.tsbAddToColor = new System.Windows.Forms.ToolStripButton();
            this.tsbRemoveFromColor = new System.Windows.Forms.ToolStripButton();
            this.tsbHighlight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.tsbClearAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUpload = new System.Windows.Forms.ToolStripButton();
            this.tslShowHide = new System.Windows.Forms.ToolStripLabel();
            this.pnlColorMain.SuspendLayout();
            this.pnlColorPanels.SuspendLayout();
            this.tsToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlColorMain
            // 
            this.pnlColorMain.Controls.Add(this.pnlActiveColor);
            this.pnlColorMain.Controls.Add(this.pnlColorPanels);
            this.pnlColorMain.Controls.Add(this.lbActiveColor);
            this.pnlColorMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlColorMain.Location = new System.Drawing.Point(0, 25);
            this.pnlColorMain.Name = "pnlColorMain";
            this.pnlColorMain.Size = new System.Drawing.Size(468, 44);
            this.pnlColorMain.TabIndex = 6;
            // 
            // pnlActiveColor
            // 
            this.pnlActiveColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlActiveColor.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlActiveColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlActiveColor.Location = new System.Drawing.Point(255, 9);
            this.pnlActiveColor.Name = "pnlActiveColor";
            this.pnlActiveColor.Size = new System.Drawing.Size(29, 27);
            this.pnlActiveColor.TabIndex = 3;
            // 
            // pnlColorPanels
            // 
            this.pnlColorPanels.BackColor = System.Drawing.SystemColors.Control;
            this.pnlColorPanels.Controls.Add(this.pnlSample);
            this.pnlColorPanels.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlColorPanels.Location = new System.Drawing.Point(0, 0);
            this.pnlColorPanels.Name = "pnlColorPanels";
            this.pnlColorPanels.Size = new System.Drawing.Size(249, 44);
            this.pnlColorPanels.TabIndex = 8;
            // 
            // pnlSample
            // 
            this.pnlSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSample.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlSample.Location = new System.Drawing.Point(224, 9);
            this.pnlSample.Name = "pnlSample";
            this.pnlSample.Size = new System.Drawing.Size(22, 21);
            this.pnlSample.TabIndex = 3;
            this.pnlSample.Tag = "8";
            // 
            // lbActiveColor
            // 
            this.lbActiveColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            // tsToolBar
            // 
            this.tsToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSetColor,
            this.tsbAddToColor,
            this.tsbRemoveFromColor,
            this.tsbHighlight,
            this.toolStripSeparator1,
            this.tsbClear,
            this.tsbClearAll,
            this.toolStripSeparator2,
            this.tsbUpload,
            this.tslShowHide});
            this.tsToolBar.Location = new System.Drawing.Point(0, 0);
            this.tsToolBar.Name = "tsToolBar";
            this.tsToolBar.Size = new System.Drawing.Size(468, 25);
            this.tsToolBar.TabIndex = 7;
            this.tsToolBar.Text = "Show";
            // 
            // tsbSetColor
            // 
            this.tsbSetColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSetColor.Image = ((System.Drawing.Image)(resources.GetObject("tsbSetColor.Image")));
            this.tsbSetColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSetColor.Name = "tsbSetColor";
            this.tsbSetColor.Size = new System.Drawing.Size(23, 22);
            this.tsbSetColor.Text = "tsbSetColor";
            // 
            // tsbAddToColor
            // 
            this.tsbAddToColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddToColor.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddToColor.Image")));
            this.tsbAddToColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddToColor.Name = "tsbAddToColor";
            this.tsbAddToColor.Size = new System.Drawing.Size(23, 22);
            this.tsbAddToColor.Text = "tsbAddColor";
            // 
            // tsbRemoveFromColor
            // 
            this.tsbRemoveFromColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveFromColor.Image = ((System.Drawing.Image)(resources.GetObject("tsbRemoveFromColor.Image")));
            this.tsbRemoveFromColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveFromColor.Name = "tsbRemoveFromColor";
            this.tsbRemoveFromColor.Size = new System.Drawing.Size(23, 22);
            this.tsbRemoveFromColor.Text = "tsbRemoveColor";
            // 
            // tsbHighlight
            // 
            this.tsbHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHighlight.Image = ((System.Drawing.Image)(resources.GetObject("tsbHighlight.Image")));
            this.tsbHighlight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHighlight.Name = "tsbHighlight";
            this.tsbHighlight.Size = new System.Drawing.Size(23, 22);
            this.tsbHighlight.Text = "tsbHighlight";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClear
            // 
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(23, 22);
            this.tsbClear.Text = "tsbClear";
            // 
            // tsbClearAll
            // 
            this.tsbClearAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClearAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbClearAll.Image")));
            this.tsbClearAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearAll.Name = "tsbClearAll";
            this.tsbClearAll.Size = new System.Drawing.Size(23, 22);
            this.tsbClearAll.Text = "tsbClearAll";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUpload
            // 
            this.tsbUpload.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpload.Image")));
            this.tsbUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpload.Name = "tsbUpload";
            this.tsbUpload.Size = new System.Drawing.Size(75, 22);
            this.tsbUpload.Text = "tsbUpload";
            // 
            // tslShowHide
            // 
            this.tslShowHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslShowHide.Image = ((System.Drawing.Image)(resources.GetObject("tslShowHide.Image")));
            this.tslShowHide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tslShowHide.IsLink = true;
            this.tslShowHide.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.tslShowHide.Name = "tslShowHide";
            this.tslShowHide.Size = new System.Drawing.Size(28, 22);
            this.tslShowHide.Text = "Hide";
            // 
            // ColorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 67);
            this.Controls.Add(this.pnlColorMain);
            this.Controls.Add(this.tsToolBar);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Colors";
            this.SizeChanged += new System.EventHandler(this.ColorForm_SizeChanged);
            this.Move += new System.EventHandler(this.ColorForm_Move);
            this.MouseLeave += new System.EventHandler(this.ColorForm_MouseLeave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorForm_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ColorForm_KeyUpDown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ColorForm_KeyUpDown);
            this.Load += new System.EventHandler(this.ColorForm_Load);
            this.pnlColorMain.ResumeLayout(false);
            this.pnlColorMain.PerformLayout();
            this.pnlColorPanels.ResumeLayout(false);
            this.tsToolBar.ResumeLayout(false);
            this.tsToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlColorMain;
        private System.Windows.Forms.Panel pnlActiveColor;
        private System.Windows.Forms.Label lbActiveColor;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolTip toolTipPanel;
        private System.Windows.Forms.Panel pnlColorPanels;
        private System.Windows.Forms.Panel pnlSample;
        private System.Windows.Forms.ToolStrip tsToolBar;
        private System.Windows.Forms.ToolStripButton tsbSetColor;
        private System.Windows.Forms.ToolStripButton tsbRemoveFromColor;
        private System.Windows.Forms.ToolStripButton tsbAddToColor;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripButton tsbClearAll;
        private System.Windows.Forms.ToolStripButton tsbUpload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbHighlight;
        private System.Windows.Forms.ToolStripLabel tslShowHide;
    }
}