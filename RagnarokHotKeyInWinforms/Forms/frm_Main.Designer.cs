namespace RagnarokHotKeyInWinforms
{
    partial class frm_Main
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
            this.label1 = new System.Windows.Forms.Label();
            this.characterName = new System.Windows.Forms.Label();
            this.processCombobox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSupportedServer = new System.Windows.Forms.Label();
            this.pbSupportedServer = new System.Windows.Forms.ProgressBar();
            this.brnRefresh = new System.Windows.Forms.Button();
            this.tabControlAutopot = new System.Windows.Forms.TabControl();
            this.tabPageAutopot = new System.Windows.Forms.TabPage();
            this.tabPageYggAutopot = new System.Windows.Forms.TabPage();
            this.tabPageSkillTimer = new System.Windows.Forms.TabPage();
            this.profileCb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPageSpammer = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageProfiles = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tabAutoBuffStuff = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControlAutopot.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPageSpammer.SuspendLayout();
            this.tabPageProfiles.SuspendLayout();
            this.tabAutoBuffStuff.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Character Name";
            // 
            // characterName
            // 
            this.characterName.AutoSize = true;
            this.characterName.Location = new System.Drawing.Point(271, 84);
            this.characterName.Name = "characterName";
            this.characterName.Size = new System.Drawing.Size(37, 13);
            this.characterName.TabIndex = 1;
            this.characterName.Text = "----------";
            // 
            // processCombobox
            // 
            this.processCombobox.FormattingEnabled = true;
            this.processCombobox.Location = new System.Drawing.Point(271, 26);
            this.processCombobox.Name = "processCombobox";
            this.processCombobox.Size = new System.Drawing.Size(121, 21);
            this.processCombobox.TabIndex = 2;
            this.processCombobox.SelectedIndexChanged += new System.EventHandler(this.processCombobox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ragnarok Client";
            // 
            // lblSupportedServer
            // 
            this.lblSupportedServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblSupportedServer.AutoSize = true;
            this.lblSupportedServer.Location = new System.Drawing.Point(4, 658);
            this.lblSupportedServer.Name = "lblSupportedServer";
            this.lblSupportedServer.Size = new System.Drawing.Size(150, 13);
            this.lblSupportedServer.TabIndex = 5;
            this.lblSupportedServer.Text = "Updating Supported Servers...";
            // 
            // pbSupportedServer
            // 
            this.pbSupportedServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pbSupportedServer.Location = new System.Drawing.Point(7, 674);
            this.pbSupportedServer.Name = "pbSupportedServer";
            this.pbSupportedServer.Size = new System.Drawing.Size(789, 10);
            this.pbSupportedServer.TabIndex = 4;
            // 
            // brnRefresh
            // 
            this.brnRefresh.Location = new System.Drawing.Point(398, 26);
            this.brnRefresh.Name = "brnRefresh";
            this.brnRefresh.Size = new System.Drawing.Size(75, 21);
            this.brnRefresh.TabIndex = 6;
            this.brnRefresh.Text = "Refresh";
            this.brnRefresh.UseVisualStyleBackColor = true;
            this.brnRefresh.Click += new System.EventHandler(this.brnRefresh_Click);
            // 
            // tabControlAutopot
            // 
            this.tabControlAutopot.Controls.Add(this.tabPageAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageYggAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageSkillTimer);
            this.tabControlAutopot.Location = new System.Drawing.Point(2, 189);
            this.tabControlAutopot.Name = "tabControlAutopot";
            this.tabControlAutopot.SelectedIndex = 0;
            this.tabControlAutopot.Size = new System.Drawing.Size(267, 154);
            this.tabControlAutopot.TabIndex = 7;
            // 
            // tabPageAutopot
            // 
            this.tabPageAutopot.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutopot.Name = "tabPageAutopot";
            this.tabPageAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutopot.Size = new System.Drawing.Size(259, 128);
            this.tabPageAutopot.TabIndex = 0;
            this.tabPageAutopot.Text = "Autopot";
            this.tabPageAutopot.UseVisualStyleBackColor = true;
            // 
            // tabPageYggAutopot
            // 
            this.tabPageYggAutopot.Location = new System.Drawing.Point(4, 22);
            this.tabPageYggAutopot.Name = "tabPageYggAutopot";
            this.tabPageYggAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageYggAutopot.Size = new System.Drawing.Size(259, 128);
            this.tabPageYggAutopot.TabIndex = 1;
            this.tabPageYggAutopot.Text = "Ygg";
            this.tabPageYggAutopot.UseVisualStyleBackColor = true;
            // 
            // tabPageSkillTimer
            // 
            this.tabPageSkillTimer.Location = new System.Drawing.Point(4, 22);
            this.tabPageSkillTimer.Name = "tabPageSkillTimer";
            this.tabPageSkillTimer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSkillTimer.Size = new System.Drawing.Size(259, 128);
            this.tabPageSkillTimer.TabIndex = 2;
            this.tabPageSkillTimer.Text = "Skill Timer";
            this.tabPageSkillTimer.UseVisualStyleBackColor = true;
            // 
            // profileCb
            // 
            this.profileCb.FormattingEnabled = true;
            this.profileCb.Location = new System.Drawing.Point(520, 26);
            this.profileCb.Name = "profileCb";
            this.profileCb.Size = new System.Drawing.Size(121, 21);
            this.profileCb.TabIndex = 8;
            this.profileCb.SelectedIndexChanged += new System.EventHandler(this.profileCb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(517, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Profile";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(2, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 183);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(53, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 40);
            this.label4.TabIndex = 11;
            this.label4.Text = "Toggle Application\r\nStatus Effect Area";
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPageSpammer);
            this.tabMain.Controls.Add(this.tabPageProfiles);
            this.tabMain.Controls.Add(this.tabAutoBuffStuff);
            this.tabMain.Location = new System.Drawing.Point(6, 345);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(794, 306);
            this.tabMain.TabIndex = 11;
            // 
            // tabPageSpammer
            // 
            this.tabPageSpammer.Controls.Add(this.label5);
            this.tabPageSpammer.Location = new System.Drawing.Point(4, 22);
            this.tabPageSpammer.Name = "tabPageSpammer";
            this.tabPageSpammer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSpammer.Size = new System.Drawing.Size(786, 236);
            this.tabPageSpammer.TabIndex = 0;
            this.tabPageSpammer.Text = "Skill Spammer";
            this.tabPageSpammer.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(233, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(356, 40);
            this.label5.TabIndex = 12;
            this.label5.Text = "This is where the AHK form will take effect\r\nWhere we config the attacker and def" +
    "ender";
            this.label5.Visible = false;
            // 
            // tabPageProfiles
            // 
            this.tabPageProfiles.Controls.Add(this.label6);
            this.tabPageProfiles.Location = new System.Drawing.Point(4, 22);
            this.tabPageProfiles.Name = "tabPageProfiles";
            this.tabPageProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProfiles.Size = new System.Drawing.Size(786, 236);
            this.tabPageProfiles.TabIndex = 1;
            this.tabPageProfiles.Text = "Profile Creation";
            this.tabPageProfiles.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(175, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(474, 40);
            this.label6.TabIndex = 13;
            this.label6.Text = "This is where the creation of profile. Mission is to sync this\r\nonline";
            this.label6.Visible = false;
            // 
            // tabAutoBuffStuff
            // 
            this.tabAutoBuffStuff.Controls.Add(this.label7);
            this.tabAutoBuffStuff.Location = new System.Drawing.Point(4, 22);
            this.tabAutoBuffStuff.Name = "tabAutoBuffStuff";
            this.tabAutoBuffStuff.Size = new System.Drawing.Size(786, 280);
            this.tabAutoBuffStuff.TabIndex = 2;
            this.tabAutoBuffStuff.Text = "AutoBuff - Stuffs";
            this.tabAutoBuffStuff.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(337, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Auto buff stuff";
            this.label7.Visible = false;
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 689);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.profileCb);
            this.Controls.Add(this.tabControlAutopot);
            this.Controls.Add(this.brnRefresh);
            this.Controls.Add(this.lblSupportedServer);
            this.Controls.Add(this.pbSupportedServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.processCombobox);
            this.Controls.Add(this.characterName);
            this.Controls.Add(this.label1);
            this.Name = "frm_Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControlAutopot.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabPageSpammer.ResumeLayout(false);
            this.tabPageSpammer.PerformLayout();
            this.tabPageProfiles.ResumeLayout(false);
            this.tabPageProfiles.PerformLayout();
            this.tabAutoBuffStuff.ResumeLayout(false);
            this.tabAutoBuffStuff.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label characterName;
        private System.Windows.Forms.ComboBox processCombobox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSupportedServer;
        private System.Windows.Forms.ProgressBar pbSupportedServer;
        private System.Windows.Forms.Button brnRefresh;
        private System.Windows.Forms.TabControl tabControlAutopot;
        private System.Windows.Forms.TabPage tabPageAutopot;
        private System.Windows.Forms.TabPage tabPageYggAutopot;
        private System.Windows.Forms.ComboBox profileCb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPageSkillTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPageSpammer;
        private System.Windows.Forms.TabPage tabPageProfiles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabAutoBuffStuff;
        private System.Windows.Forms.Label label7;
    }
}

