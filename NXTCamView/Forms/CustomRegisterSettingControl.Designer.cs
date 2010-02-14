namespace NXTCamView.Forms
{
    partial class CustomRegisterSettingControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbRegisterEnabled = new System.Windows.Forms.CheckBox();
            this.nudValue = new System.Windows.Forms.NumericUpDown();
            this.nudReg = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReg)).BeginInit();
            this.SuspendLayout();
            // 
            // cbRegisterEnabled
            // 
            this.cbRegisterEnabled.AutoSize = true;
            this.cbRegisterEnabled.Location = new System.Drawing.Point(3, 5);
            this.cbRegisterEnabled.Name = "cbRegisterEnabled";
            this.cbRegisterEnabled.Size = new System.Drawing.Size(68, 17);
            this.cbRegisterEnabled.TabIndex = 8;
            this.cbRegisterEnabled.Text = "Register:";
            this.cbRegisterEnabled.UseVisualStyleBackColor = true;
            // 
            // nudValue
            // 
            this.nudValue.Location = new System.Drawing.Point(173, 4);
            this.nudValue.Maximum = new decimal(new int[] {
                                                              255,
                                                              0,
                                                              0,
                                                              0});
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(57, 20);
            this.nudValue.TabIndex = 10;
            // 
            // nudReg
            // 
            this.nudReg.Location = new System.Drawing.Point(69, 4);
            this.nudReg.Maximum = new decimal(new int[] {
                                                            255,
                                                            0,
                                                            0,
                                                            0});
            this.nudReg.Name = "nudReg";
            this.nudReg.Size = new System.Drawing.Size(57, 20);
            this.nudReg.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Value:";
            // 
            // CustomRegisterSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudReg);
            this.Controls.Add(this.nudValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRegisterEnabled);
            this.Name = "CustomRegisterSettingControl";
            this.Size = new System.Drawing.Size(280, 26);
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbRegisterEnabled;
        private System.Windows.Forms.NumericUpDown nudValue;
        private System.Windows.Forms.NumericUpDown nudReg;
        private System.Windows.Forms.Label label1;
    }
}


