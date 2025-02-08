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
            this.label3 = new System.Windows.Forms.Label();
            this.pbSupportedServer = new System.Windows.Forms.ProgressBar();
            this.brnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Character Name";
            // 
            // characterName
            // 
            this.characterName.AutoSize = true;
            this.characterName.Location = new System.Drawing.Point(12, 129);
            this.characterName.Name = "characterName";
            this.characterName.Size = new System.Drawing.Size(37, 13);
            this.characterName.TabIndex = 1;
            this.characterName.Text = "----------";
            // 
            // processCombobox
            // 
            this.processCombobox.FormattingEnabled = true;
            this.processCombobox.Location = new System.Drawing.Point(12, 189);
            this.processCombobox.Name = "processCombobox";
            this.processCombobox.Size = new System.Drawing.Size(121, 21);
            this.processCombobox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ragnarok Client";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Updating Supported Servers...";
            // 
            // pbSupportedServer
            // 
            this.pbSupportedServer.Location = new System.Drawing.Point(12, 26);
            this.pbSupportedServer.Name = "pbSupportedServer";
            this.pbSupportedServer.Size = new System.Drawing.Size(776, 23);
            this.pbSupportedServer.TabIndex = 4;
            // 
            // brnRefresh
            // 
            this.brnRefresh.Location = new System.Drawing.Point(139, 189);
            this.brnRefresh.Name = "brnRefresh";
            this.brnRefresh.Size = new System.Drawing.Size(75, 23);
            this.brnRefresh.TabIndex = 6;
            this.brnRefresh.Text = "Refresh";
            this.brnRefresh.UseVisualStyleBackColor = true;
            this.brnRefresh.Click += new System.EventHandler(this.brnRefresh_Click);
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.brnRefresh);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbSupportedServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.processCombobox);
            this.Controls.Add(this.characterName);
            this.Controls.Add(this.label1);
            this.Name = "frm_Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label characterName;
        private System.Windows.Forms.ComboBox processCombobox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pbSupportedServer;
        private System.Windows.Forms.Button brnRefresh;
    }
}

