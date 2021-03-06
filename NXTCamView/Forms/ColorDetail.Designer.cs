﻿using NXTCamView.Controls;

namespace NXTCamView.Forms
{
    partial class ColorDetail
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
            this.pnlMatchingColors = new System.Windows.Forms.Panel();
            this.lblNoSet = new System.Windows.Forms.Label();
            this.lbMatches = new System.Windows.Forms.Label();
            this.nudRange = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.rbRed = new RangeBar();
            this.rbGreen = new RangeBar();
            this.rbBlue = new RangeBar();
            this.pnlMatchingColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRange)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMatchingColors
            // 
            this.pnlMatchingColors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMatchingColors.Controls.Add(this.lblNoSet);
            this.pnlMatchingColors.Location = new System.Drawing.Point(194, 9);
            this.pnlMatchingColors.Name = "pnlMatchingColors";
            this.pnlMatchingColors.Size = new System.Drawing.Size(200, 200);
            this.pnlMatchingColors.TabIndex = 0;
            this.pnlMatchingColors.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            // 
            // lblNoSet
            // 
            this.lblNoSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                          | System.Windows.Forms.AnchorStyles.Left)
                                                                         | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoSet.Location = new System.Drawing.Point(1, 1);
            this.lblNoSet.Name = "lblNoSet";
            this.lblNoSet.Size = new System.Drawing.Size(195, 197);
            this.lblNoSet.TabIndex = 0;
            this.lblNoSet.Text = "Color not tracked";
            this.lblNoSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoSet.Visible = false;
            // 
            // lbMatches
            // 
            this.lbMatches.AutoSize = true;
            this.lbMatches.Location = new System.Drawing.Point(191, 212);
            this.lbMatches.Name = "lbMatches";
            this.lbMatches.Size = new System.Drawing.Size(35, 13);
            this.lbMatches.TabIndex = 4;
            this.lbMatches.Text = "label1";
            // 
            // nudRange
            // 
            this.nudRange.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::NXTCamView.Properties.Settings.Default, "ColorTolerance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudRange.Location = new System.Drawing.Point(90, 155);
            this.nudRange.Maximum = new decimal(new int[] {
                                                              16,
                                                              0,
                                                              0,
                                                              0});
            this.nudRange.Name = "nudRange";
            this.nudRange.Size = new System.Drawing.Size(34, 20);
            this.nudRange.TabIndex = 5;
            this.nudRange.Value = global::NXTCamView.Properties.Settings.Default.ColorTolerance;
            this.nudRange.ValueChanged += new System.EventHandler(this.nudRange_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Color tolerance:";
            // 
            // lblWarning
            // 
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(3, 179);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(180, 32);
            this.lblWarning.TabIndex = 6;
            this.lblWarning.Text = "warning";
            // 
            // rbRed
            // 
            this.rbRed.BarHeight = 16;
            this.rbRed.ColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rbRed.ColorFunction = ColorFunction.SetColor;
            this.rbRed.DivisionCount = 15;
            this.rbRed.InnerColor = System.Drawing.Color.Red;
            this.rbRed.Location = new System.Drawing.Point(6, -14);
            this.rbRed.MarkHeight = 24;
            this.rbRed.Name = "rbRed";
            this.rbRed.Orientation = RangeBar.RangeBarOrientation.Horizontal;
            this.rbRed.RangeMaximum = 5;
            this.rbRed.RangeMinimum = 0;
            this.rbRed.ScaleOrientation = RangeBar.TopBottomOrientation.Bottom;
            this.rbRed.Size = new System.Drawing.Size(177, 65);
            this.rbRed.TabIndex = 3;
            this.rbRed.TickHeight = 6;
            this.rbRed.TotalMaximum = 15;
            this.rbRed.TotalMinimum = 0;
            this.rbRed.Value = 0;
            this.rbRed.ValueActive = false;
            this.rbRed.RangeChanging += new RangeBar.RangeChangedEventHandler(this.RangeChanging);
            // 
            // rbGreen
            // 
            this.rbGreen.BarHeight = 16;
            this.rbGreen.ColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbGreen.ColorFunction = ColorFunction.SetColor;
            this.rbGreen.DivisionCount = 15;
            this.rbGreen.InnerColor = System.Drawing.Color.Green;
            this.rbGreen.Location = new System.Drawing.Point(6, 39);
            this.rbGreen.MarkHeight = 24;
            this.rbGreen.Name = "rbGreen";
            this.rbGreen.Orientation = RangeBar.RangeBarOrientation.Horizontal;
            this.rbGreen.RangeMaximum = 5;
            this.rbGreen.RangeMinimum = 0;
            this.rbGreen.ScaleOrientation = RangeBar.TopBottomOrientation.Bottom;
            this.rbGreen.Size = new System.Drawing.Size(177, 65);
            this.rbGreen.TabIndex = 3;
            this.rbGreen.TickHeight = 6;
            this.rbGreen.TotalMaximum = 15;
            this.rbGreen.TotalMinimum = 0;
            this.rbGreen.Value = 0;
            this.rbGreen.ValueActive = false;
            this.rbGreen.RangeChanging += new RangeBar.RangeChangedEventHandler(this.RangeChanging);
            // 
            // rbBlue
            // 
            this.rbBlue.BarHeight = 16;
            this.rbBlue.ColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbBlue.ColorFunction = ColorFunction.SetColor;
            this.rbBlue.DivisionCount = 15;
            this.rbBlue.InnerColor = System.Drawing.Color.Blue;
            this.rbBlue.Location = new System.Drawing.Point(6, 89);
            this.rbBlue.MarkHeight = 24;
            this.rbBlue.Name = "rbBlue";
            this.rbBlue.Orientation = RangeBar.RangeBarOrientation.Horizontal;
            this.rbBlue.RangeMaximum = 5;
            this.rbBlue.RangeMinimum = 0;
            this.rbBlue.ScaleOrientation = RangeBar.TopBottomOrientation.Bottom;
            this.rbBlue.Size = new System.Drawing.Size(177, 65);
            this.rbBlue.TabIndex = 3;
            this.rbBlue.TickHeight = 6;
            this.rbBlue.TotalMaximum = 15;
            this.rbBlue.TotalMinimum = 0;
            this.rbBlue.Value = 0;
            this.rbBlue.ValueActive = false;
            this.rbBlue.RangeChanging += new RangeBar.RangeChangedEventHandler(this.RangeChanging);
            // 
            // ColorDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbRed);
            this.Controls.Add(this.rbGreen);
            this.Controls.Add(this.rbBlue);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.nudRange);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbMatches);
            this.Controls.Add(this.pnlMatchingColors);
            this.DoubleBuffered = true;
            this.Name = "ColorDetail";
            this.Size = new System.Drawing.Size(406, 234);
            this.Load += new System.EventHandler(this.ColorDetail_Load);
            this.pnlMatchingColors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudRange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMatchingColors;
        private RangeBar rbRed;
        private RangeBar rbGreen;
        private RangeBar rbBlue;
        private System.Windows.Forms.Label lbMatches;
        private System.Windows.Forms.NumericUpDown nudRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNoSet;
        public System.Windows.Forms.Label lblWarning;
    }
}