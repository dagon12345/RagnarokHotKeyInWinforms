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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            this.label1 = new System.Windows.Forms.Label();
            this.characterName = new System.Windows.Forms.Label();
            this.processCombobox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSupportedServer = new System.Windows.Forms.Label();
            this.pbSupportedServer = new System.Windows.Forms.ProgressBar();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtNewStatusKey = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtStatusKey = new System.Windows.Forms.TextBox();
            this.lblStatusToggle = new System.Windows.Forms.TextBox();
            this.btnStatusToggle = new System.Windows.Forms.Button();
            this.txtStatusToggleKey = new System.Windows.Forms.TextBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPageSpammer = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageProfiles = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tabAutoBuffStuff = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.tabAutoBuffSkill = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPageMacroSongs = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPageAtkDef = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPageMacroSwitch = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabControlAutopot.SuspendLayout();
            this.tabPageAutopot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutopotDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSPpct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHPpct)).BeginInit();
            this.tabPageSkillTimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoRefreshDelay)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabPageSpammer.SuspendLayout();
            this.tabPageProfiles.SuspendLayout();
            this.tabAutoBuffStuff.SuspendLayout();
            this.tabAutoBuffSkill.SuspendLayout();
            this.tabPageMacroSongs.SuspendLayout();
            this.tabPageAtkDef.SuspendLayout();
            this.tabPageMacroSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(577, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Character Name";
            // 
            // characterName
            // 
            this.characterName.AutoSize = true;
            this.characterName.Location = new System.Drawing.Point(667, 40);
            this.characterName.Name = "characterName";
            this.characterName.Size = new System.Drawing.Size(37, 13);
            this.characterName.TabIndex = 1;
            this.characterName.Text = "----------";
            // 
            // processCombobox
            // 
            this.processCombobox.FormattingEnabled = true;
            this.processCombobox.Location = new System.Drawing.Point(361, 37);
            this.processCombobox.Name = "processCombobox";
            this.processCombobox.Size = new System.Drawing.Size(194, 21);
            this.processCombobox.TabIndex = 2;
            this.processCombobox.SelectedIndexChanged += new System.EventHandler(this.processCombobox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(272, 40);
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
            // tabControlAutopot
            // 
            this.tabControlAutopot.Controls.Add(this.tabPageAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageSkillTimer);
            this.tabControlAutopot.Location = new System.Drawing.Point(2, 189);
            this.tabControlAutopot.Name = "tabControlAutopot";
            this.tabControlAutopot.SelectedIndex = 0;
            this.tabControlAutopot.Size = new System.Drawing.Size(228, 154);
            this.tabControlAutopot.TabIndex = 7;
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
            this.tabPageAutopot.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutopot.Name = "tabPageAutopot";
            this.tabPageAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutopot.Size = new System.Drawing.Size(220, 128);
            this.tabPageAutopot.TabIndex = 0;
            this.tabPageAutopot.Text = "Autopot";
            this.tabPageAutopot.UseVisualStyleBackColor = true;
            // 
            // txtAutopotDelay
            // 
            this.txtAutopotDelay.Location = new System.Drawing.Point(144, 77);
            this.txtAutopotDelay.Name = "txtAutopotDelay";
            this.txtAutopotDelay.ReadOnly = true;
            this.txtAutopotDelay.Size = new System.Drawing.Size(39, 20);
            this.txtAutopotDelay.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(187, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "%";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(187, 31);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "%";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(41, 56);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "SP";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(41, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(22, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "HP";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(189, 79);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(20, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "ms";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(104, 80);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 14;
            this.label16.Text = "Delay";
            // 
            // txtSPpct
            // 
            this.txtSPpct.Location = new System.Drawing.Point(144, 54);
            this.txtSPpct.Name = "txtSPpct";
            this.txtSPpct.ReadOnly = true;
            this.txtSPpct.Size = new System.Drawing.Size(39, 20);
            this.txtSPpct.TabIndex = 13;
            // 
            // txtHPpct
            // 
            this.txtHPpct.Location = new System.Drawing.Point(144, 27);
            this.txtHPpct.Name = "txtHPpct";
            this.txtHPpct.ReadOnly = true;
            this.txtHPpct.Size = new System.Drawing.Size(39, 20);
            this.txtHPpct.TabIndex = 12;
            // 
            // txtSpKey
            // 
            this.txtSpKey.Location = new System.Drawing.Point(81, 53);
            this.txtSpKey.Name = "txtSpKey";
            this.txtSpKey.Size = new System.Drawing.Size(57, 20);
            this.txtSpKey.TabIndex = 10;
            // 
            // txtHpKey
            // 
            this.txtHpKey.Location = new System.Drawing.Point(81, 27);
            this.txtHpKey.Name = "txtHpKey";
            this.txtHpKey.Size = new System.Drawing.Size(57, 20);
            this.txtHpKey.TabIndex = 11;
            // 
            // tabPageSkillTimer
            // 
            this.tabPageSkillTimer.Controls.Add(this.label17);
            this.tabPageSkillTimer.Controls.Add(this.label18);
            this.tabPageSkillTimer.Controls.Add(this.label19);
            this.tabPageSkillTimer.Controls.Add(this.txtAutoRefreshDelay);
            this.tabPageSkillTimer.Controls.Add(this.txtSkillTimerKey);
            this.tabPageSkillTimer.Location = new System.Drawing.Point(4, 22);
            this.tabPageSkillTimer.Name = "tabPageSkillTimer";
            this.tabPageSkillTimer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSkillTimer.Size = new System.Drawing.Size(220, 128);
            this.tabPageSkillTimer.TabIndex = 2;
            this.tabPageSkillTimer.Text = "Skill Timer";
            this.tabPageSkillTimer.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(139, 40);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "seconds";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(34, 64);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 13);
            this.label18.TabIndex = 18;
            this.label18.Text = "Key";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(34, 38);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 13);
            this.label19.TabIndex = 19;
            this.label19.Text = "Delay";
            // 
            // txtAutoRefreshDelay
            // 
            this.txtAutoRefreshDelay.Location = new System.Drawing.Point(74, 37);
            this.txtAutoRefreshDelay.Name = "txtAutoRefreshDelay";
            this.txtAutoRefreshDelay.ReadOnly = true;
            this.txtAutoRefreshDelay.Size = new System.Drawing.Size(57, 20);
            this.txtAutoRefreshDelay.TabIndex = 17;
            // 
            // txtSkillTimerKey
            // 
            this.txtSkillTimerKey.Location = new System.Drawing.Point(74, 61);
            this.txtSkillTimerKey.Name = "txtSkillTimerKey";
            this.txtSkillTimerKey.Size = new System.Drawing.Size(57, 20);
            this.txtSkillTimerKey.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel5);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.txtNewStatusKey);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.txtStatusKey);
            this.groupBox1.Controls.Add(this.lblStatusToggle);
            this.groupBox1.Controls.Add(this.btnStatusToggle);
            this.groupBox1.Controls.Add(this.txtStatusToggleKey);
            this.groupBox1.Location = new System.Drawing.Point(2, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 183);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.Location = new System.Drawing.Point(132, 147);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1, 30);
            this.panel5.TabIndex = 33;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(170, 156);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(37, 13);
            this.label20.TabIndex = 32;
            this.label20.Text = "Status";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(144, 150);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 31;
            this.pictureBox2.TabStop = false;
            // 
            // txtNewStatusKey
            // 
            this.txtNewStatusKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNewStatusKey.Location = new System.Drawing.Point(207, 151);
            this.txtNewStatusKey.Name = "txtNewStatusKey";
            this.txtNewStatusKey.Size = new System.Drawing.Size(45, 23);
            this.txtNewStatusKey.TabIndex = 30;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(39, 156);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(37, 13);
            this.label21.TabIndex = 29;
            this.label21.Text = "Status";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 150);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // txtStatusKey
            // 
            this.txtStatusKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtStatusKey.Location = new System.Drawing.Point(76, 151);
            this.txtStatusKey.Name = "txtStatusKey";
            this.txtStatusKey.Size = new System.Drawing.Size(45, 23);
            this.txtStatusKey.TabIndex = 27;
            // 
            // lblStatusToggle
            // 
            this.lblStatusToggle.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatusToggle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblStatusToggle.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusToggle.Location = new System.Drawing.Point(4, 110);
            this.lblStatusToggle.Name = "lblStatusToggle";
            this.lblStatusToggle.ReadOnly = true;
            this.lblStatusToggle.Size = new System.Drawing.Size(243, 16);
            this.lblStatusToggle.TabIndex = 5;
            this.lblStatusToggle.Text = "Press the button to start!";
            this.lblStatusToggle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnStatusToggle
            // 
            this.btnStatusToggle.Location = new System.Drawing.Point(78, 35);
            this.btnStatusToggle.Name = "btnStatusToggle";
            this.btnStatusToggle.Size = new System.Drawing.Size(100, 23);
            this.btnStatusToggle.TabIndex = 4;
            this.btnStatusToggle.Text = "Off";
            this.btnStatusToggle.UseVisualStyleBackColor = true;
            this.btnStatusToggle.Click += new System.EventHandler(this.btnStatusToggle_Click);
            // 
            // txtStatusToggleKey
            // 
            this.txtStatusToggleKey.Location = new System.Drawing.Point(78, 64);
            this.txtStatusToggleKey.Multiline = true;
            this.txtStatusToggleKey.Name = "txtStatusToggleKey";
            this.txtStatusToggleKey.Size = new System.Drawing.Size(100, 40);
            this.txtStatusToggleKey.TabIndex = 3;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPageSpammer);
            this.tabMain.Controls.Add(this.tabPageProfiles);
            this.tabMain.Controls.Add(this.tabAutoBuffStuff);
            this.tabMain.Controls.Add(this.tabAutoBuffSkill);
            this.tabMain.Controls.Add(this.tabPageMacroSongs);
            this.tabMain.Controls.Add(this.tabPageAtkDef);
            this.tabMain.Controls.Add(this.tabPageMacroSwitch);
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
            this.tabPageSpammer.Size = new System.Drawing.Size(786, 280);
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
            this.tabPageProfiles.Size = new System.Drawing.Size(786, 280);
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
            // tabAutoBuffSkill
            // 
            this.tabAutoBuffSkill.Controls.Add(this.label8);
            this.tabAutoBuffSkill.Location = new System.Drawing.Point(4, 22);
            this.tabAutoBuffSkill.Name = "tabAutoBuffSkill";
            this.tabAutoBuffSkill.Padding = new System.Windows.Forms.Padding(3);
            this.tabAutoBuffSkill.Size = new System.Drawing.Size(786, 280);
            this.tabAutoBuffSkill.TabIndex = 3;
            this.tabAutoBuffSkill.Text = "AutoBuff - Skills";
            this.tabAutoBuffSkill.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(330, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 20);
            this.label8.TabIndex = 15;
            this.label8.Text = "Auto buff skill";
            this.label8.Visible = false;
            // 
            // tabPageMacroSongs
            // 
            this.tabPageMacroSongs.Controls.Add(this.label9);
            this.tabPageMacroSongs.Location = new System.Drawing.Point(4, 22);
            this.tabPageMacroSongs.Name = "tabPageMacroSongs";
            this.tabPageMacroSongs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMacroSongs.Size = new System.Drawing.Size(786, 280);
            this.tabPageMacroSongs.TabIndex = 4;
            this.tabPageMacroSongs.Text = "Macro Songs";
            this.tabPageMacroSongs.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(330, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 20);
            this.label9.TabIndex = 15;
            this.label9.Text = "Macro Songs tab";
            this.label9.Visible = false;
            // 
            // tabPageAtkDef
            // 
            this.tabPageAtkDef.Controls.Add(this.label10);
            this.tabPageAtkDef.Location = new System.Drawing.Point(4, 22);
            this.tabPageAtkDef.Name = "tabPageAtkDef";
            this.tabPageAtkDef.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAtkDef.Size = new System.Drawing.Size(786, 280);
            this.tabPageAtkDef.TabIndex = 5;
            this.tabPageAtkDef.Text = "Attack & Def Mode";
            this.tabPageAtkDef.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(321, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(209, 20);
            this.label10.TabIndex = 16;
            this.label10.Text = "Attack and Defend Mode";
            this.label10.Visible = false;
            // 
            // tabPageMacroSwitch
            // 
            this.tabPageMacroSwitch.Controls.Add(this.label11);
            this.tabPageMacroSwitch.Location = new System.Drawing.Point(4, 22);
            this.tabPageMacroSwitch.Name = "tabPageMacroSwitch";
            this.tabPageMacroSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMacroSwitch.Size = new System.Drawing.Size(786, 280);
            this.tabPageMacroSwitch.TabIndex = 6;
            this.tabPageMacroSwitch.Text = "Macro Switch";
            this.tabPageMacroSwitch.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(331, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(151, 20);
            this.label11.TabIndex = 17;
            this.label11.Text = "Macro Switch Tab";
            this.label11.Visible = false;
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(721, 5);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 12;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(271, 10);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(193, 19);
            this.lblUserName.TabIndex = 12;
            this.lblUserName.Text = "Welcome back, {name}";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(361, 64);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(121, 23);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 689);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControlAutopot);
            this.Controls.Add(this.lblSupportedServer);
            this.Controls.Add(this.pbSupportedServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.processCombobox);
            this.Controls.Add(this.characterName);
            this.Controls.Add(this.label1);
            this.Name = "frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControlAutopot.ResumeLayout(false);
            this.tabPageAutopot.ResumeLayout(false);
            this.tabPageAutopot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutopotDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSPpct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHPpct)).EndInit();
            this.tabPageSkillTimer.ResumeLayout(false);
            this.tabPageSkillTimer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoRefreshDelay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabPageSpammer.ResumeLayout(false);
            this.tabPageSpammer.PerformLayout();
            this.tabPageProfiles.ResumeLayout(false);
            this.tabPageProfiles.PerformLayout();
            this.tabAutoBuffStuff.ResumeLayout(false);
            this.tabAutoBuffStuff.PerformLayout();
            this.tabAutoBuffSkill.ResumeLayout(false);
            this.tabAutoBuffSkill.PerformLayout();
            this.tabPageMacroSongs.ResumeLayout(false);
            this.tabPageMacroSongs.PerformLayout();
            this.tabPageAtkDef.ResumeLayout(false);
            this.tabPageAtkDef.PerformLayout();
            this.tabPageMacroSwitch.ResumeLayout(false);
            this.tabPageMacroSwitch.PerformLayout();
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
        private System.Windows.Forms.TabControl tabControlAutopot;
        private System.Windows.Forms.TabPage tabPageAutopot;
        private System.Windows.Forms.TabPage tabPageSkillTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPageSpammer;
        private System.Windows.Forms.TabPage tabPageProfiles;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabAutoBuffStuff;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabAutoBuffSkill;
        private System.Windows.Forms.TabPage tabPageMacroSongs;
        private System.Windows.Forms.TabPage tabPageAtkDef;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPageMacroSwitch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox lblStatusToggle;
        private System.Windows.Forms.Button btnStatusToggle;
        private System.Windows.Forms.TextBox txtStatusToggleKey;
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
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown txtAutoRefreshDelay;
        private System.Windows.Forms.TextBox txtSkillTimerKey;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txtNewStatusKey;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtStatusKey;
        private System.Windows.Forms.Button btnRefresh;
    }
}

