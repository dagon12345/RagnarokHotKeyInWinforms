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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControlAutopot.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(344, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Character Name";
            // 
            // characterName
            // 
            this.characterName.AutoSize = true;
            this.characterName.Location = new System.Drawing.Point(344, 87);
            this.characterName.Name = "characterName";
            this.characterName.Size = new System.Drawing.Size(37, 13);
            this.characterName.TabIndex = 1;
            this.characterName.Text = "----------";
            // 
            // processCombobox
            // 
            this.processCombobox.FormattingEnabled = true;
            this.processCombobox.Location = new System.Drawing.Point(344, 29);
            this.processCombobox.Name = "processCombobox";
            this.processCombobox.Size = new System.Drawing.Size(121, 21);
            this.processCombobox.TabIndex = 2;
            this.processCombobox.SelectedIndexChanged += new System.EventHandler(this.processCombobox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(344, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ragnarok Client";
            // 
            // lblSupportedServer
            // 
            this.lblSupportedServer.AutoSize = true;
            this.lblSupportedServer.Location = new System.Drawing.Point(2, 411);
            this.lblSupportedServer.Name = "lblSupportedServer";
            this.lblSupportedServer.Size = new System.Drawing.Size(150, 13);
            this.lblSupportedServer.TabIndex = 5;
            this.lblSupportedServer.Text = "Updating Supported Servers...";
            // 
            // pbSupportedServer
            // 
            this.pbSupportedServer.Location = new System.Drawing.Point(5, 427);
            this.pbSupportedServer.Name = "pbSupportedServer";
            this.pbSupportedServer.Size = new System.Drawing.Size(795, 23);
            this.pbSupportedServer.TabIndex = 4;
            // 
            // brnRefresh
            // 
            this.brnRefresh.Location = new System.Drawing.Point(471, 29);
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
            this.tabControlAutopot.Controls.Add(this.tabPage2);
            this.tabControlAutopot.Location = new System.Drawing.Point(12, 160);
            this.tabControlAutopot.Name = "tabControlAutopot";
            this.tabControlAutopot.SelectedIndex = 0;
            this.tabControlAutopot.Size = new System.Drawing.Size(233, 152);
            this.tabControlAutopot.TabIndex = 7;
            // 
            // tabPageAutopot
            // 
            this.tabPageAutopot.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutopot.Name = "tabPageAutopot";
            this.tabPageAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutopot.Size = new System.Drawing.Size(225, 126);
            this.tabPageAutopot.TabIndex = 0;
            this.tabPageAutopot.Text = "Autopot";
            this.tabPageAutopot.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(225, 126);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
        private System.Windows.Forms.TabPage tabPage2;
    }
}

