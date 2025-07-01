namespace ApplicationLayer.ChildForms
{
    partial class AutopotForm
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
            this.tabControlAutopot = new System.Windows.Forms.TabControl();
            this.tabPageAutopot = new System.Windows.Forms.TabPage();
            this.txtAutopotDelay = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSPpct = new System.Windows.Forms.NumericUpDown();
            this.txtHPpct = new System.Windows.Forms.NumericUpDown();
            this.txtSpKey = new System.Windows.Forms.TextBox();
            this.txtHpKey = new System.Windows.Forms.TextBox();
            this.tabPageSkillTimer = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtAutoRefreshDelay = new System.Windows.Forms.NumericUpDown();
            this.txtSkillTimerKey = new System.Windows.Forms.TextBox();
            this.tabControlAutopot.SuspendLayout();
            this.tabPageAutopot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutopotDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSPpct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHPpct)).BeginInit();
            this.tabPageSkillTimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoRefreshDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlAutopot
            // 
            this.tabControlAutopot.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlAutopot.Controls.Add(this.tabPageAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageSkillTimer);
            this.tabControlAutopot.Location = new System.Drawing.Point(1, 12);
            this.tabControlAutopot.Name = "tabControlAutopot";
            this.tabControlAutopot.SelectedIndex = 0;
            this.tabControlAutopot.Size = new System.Drawing.Size(221, 129);
            this.tabControlAutopot.TabIndex = 8;
            // 
            // tabPageAutopot
            // 
            this.tabPageAutopot.Controls.Add(this.txtAutopotDelay);
            this.tabPageAutopot.Controls.Add(this.label4);
            this.tabPageAutopot.Controls.Add(this.label12);
            this.tabPageAutopot.Controls.Add(this.label13);
            this.tabPageAutopot.Controls.Add(this.label14);
            this.tabPageAutopot.Controls.Add(this.label15);
            this.tabPageAutopot.Controls.Add(this.label16);
            this.tabPageAutopot.Controls.Add(this.txtSPpct);
            this.tabPageAutopot.Controls.Add(this.txtHPpct);
            this.tabPageAutopot.Controls.Add(this.txtSpKey);
            this.tabPageAutopot.Controls.Add(this.txtHpKey);
            this.tabPageAutopot.Location = new System.Drawing.Point(4, 25);
            this.tabPageAutopot.Name = "tabPageAutopot";
            this.tabPageAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutopot.Size = new System.Drawing.Size(213, 100);
            this.tabPageAutopot.TabIndex = 0;
            this.tabPageAutopot.Text = "Autopot";
            this.tabPageAutopot.UseVisualStyleBackColor = true;
            // 
            // txtAutopotDelay
            // 
            this.txtAutopotDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAutopotDelay.Location = new System.Drawing.Point(114, 64);
            this.txtAutopotDelay.Name = "txtAutopotDelay";
            this.txtAutopotDelay.ReadOnly = true;
            this.txtAutopotDelay.Size = new System.Drawing.Size(39, 16);
            this.txtAutopotDelay.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "%";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(157, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "%";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "SP";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(22, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "HP";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(159, 66);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(20, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "ms";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(74, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 14;
            this.label16.Text = "Delay";
            // 
            // txtSPpct
            // 
            this.txtSPpct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSPpct.Location = new System.Drawing.Point(114, 41);
            this.txtSPpct.Name = "txtSPpct";
            this.txtSPpct.ReadOnly = true;
            this.txtSPpct.Size = new System.Drawing.Size(39, 16);
            this.txtSPpct.TabIndex = 13;
            // 
            // txtHPpct
            // 
            this.txtHPpct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHPpct.Location = new System.Drawing.Point(114, 14);
            this.txtHPpct.Name = "txtHPpct";
            this.txtHPpct.ReadOnly = true;
            this.txtHPpct.Size = new System.Drawing.Size(39, 16);
            this.txtHPpct.TabIndex = 12;
            // 
            // txtSpKey
            // 
            this.txtSpKey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSpKey.Location = new System.Drawing.Point(51, 40);
            this.txtSpKey.Name = "txtSpKey";
            this.txtSpKey.Size = new System.Drawing.Size(57, 13);
            this.txtSpKey.TabIndex = 10;
            // 
            // txtHpKey
            // 
            this.txtHpKey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHpKey.Location = new System.Drawing.Point(51, 14);
            this.txtHpKey.Name = "txtHpKey";
            this.txtHpKey.Size = new System.Drawing.Size(57, 13);
            this.txtHpKey.TabIndex = 11;
            // 
            // tabPageSkillTimer
            // 
            this.tabPageSkillTimer.Controls.Add(this.label17);
            this.tabPageSkillTimer.Controls.Add(this.label18);
            this.tabPageSkillTimer.Controls.Add(this.label19);
            this.tabPageSkillTimer.Controls.Add(this.txtAutoRefreshDelay);
            this.tabPageSkillTimer.Controls.Add(this.txtSkillTimerKey);
            this.tabPageSkillTimer.Location = new System.Drawing.Point(4, 25);
            this.tabPageSkillTimer.Name = "tabPageSkillTimer";
            this.tabPageSkillTimer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSkillTimer.Size = new System.Drawing.Size(213, 100);
            this.tabPageSkillTimer.TabIndex = 2;
            this.tabPageSkillTimer.Text = "Skill Timer";
            this.tabPageSkillTimer.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(118, 19);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "seconds";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 46);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 13);
            this.label18.TabIndex = 18;
            this.label18.Text = "Key";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(11, 17);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 13);
            this.label19.TabIndex = 19;
            this.label19.Text = "Delay";
            // 
            // txtAutoRefreshDelay
            // 
            this.txtAutoRefreshDelay.Location = new System.Drawing.Point(55, 17);
            this.txtAutoRefreshDelay.Name = "txtAutoRefreshDelay";
            this.txtAutoRefreshDelay.ReadOnly = true;
            this.txtAutoRefreshDelay.Size = new System.Drawing.Size(57, 20);
            this.txtAutoRefreshDelay.TabIndex = 17;
            // 
            // txtSkillTimerKey
            // 
            this.txtSkillTimerKey.Location = new System.Drawing.Point(55, 43);
            this.txtSkillTimerKey.Name = "txtSkillTimerKey";
            this.txtSkillTimerKey.Size = new System.Drawing.Size(57, 20);
            this.txtSkillTimerKey.TabIndex = 16;
            // 
            // AutopotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 156);
            this.Controls.Add(this.tabControlAutopot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutopotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutopotForm";
            this.tabControlAutopot.ResumeLayout(false);
            this.tabPageAutopot.ResumeLayout(false);
            this.tabPageAutopot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutopotDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSPpct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHPpct)).EndInit();
            this.tabPageSkillTimer.ResumeLayout(false);
            this.tabPageSkillTimer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoRefreshDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlAutopot;
        private System.Windows.Forms.TabPage tabPageAutopot;
        private System.Windows.Forms.NumericUpDown txtAutopotDelay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown txtSPpct;
        private System.Windows.Forms.NumericUpDown txtHPpct;
        private System.Windows.Forms.TextBox txtSpKey;
        private System.Windows.Forms.TextBox txtHpKey;
        private System.Windows.Forms.TabPage tabPageSkillTimer;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown txtAutoRefreshDelay;
        private System.Windows.Forms.TextBox txtSkillTimerKey;
    }
}