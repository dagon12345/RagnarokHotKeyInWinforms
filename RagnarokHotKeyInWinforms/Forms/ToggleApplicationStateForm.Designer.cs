namespace RagnarokHotKeyInWinforms.Forms
{
    partial class ToggleApplicationStateForm
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
            this.txtStatusToggleKey = new System.Windows.Forms.TextBox();
            this.btnStatusToggle = new System.Windows.Forms.Button();
            this.lblStatusToggle = new System.Windows.Forms.Label();
            this.notifyIconTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // txtStatusToggleKey
            // 
            this.txtStatusToggleKey.Location = new System.Drawing.Point(102, 55);
            this.txtStatusToggleKey.Multiline = true;
            this.txtStatusToggleKey.Name = "txtStatusToggleKey";
            this.txtStatusToggleKey.Size = new System.Drawing.Size(100, 40);
            this.txtStatusToggleKey.TabIndex = 0;
            // 
            // btnStatusToggle
            // 
            this.btnStatusToggle.Location = new System.Drawing.Point(102, 26);
            this.btnStatusToggle.Name = "btnStatusToggle";
            this.btnStatusToggle.Size = new System.Drawing.Size(100, 23);
            this.btnStatusToggle.TabIndex = 1;
            this.btnStatusToggle.Text = "Off";
            this.btnStatusToggle.UseVisualStyleBackColor = true;
            this.btnStatusToggle.Click += new System.EventHandler(this.btnStatusToggle_Click);
            // 
            // lblStatusToggle
            // 
            this.lblStatusToggle.AutoSize = true;
            this.lblStatusToggle.Location = new System.Drawing.Point(91, 101);
            this.lblStatusToggle.Name = "lblStatusToggle";
            this.lblStatusToggle.Size = new System.Drawing.Size(122, 13);
            this.lblStatusToggle.TabIndex = 2;
            this.lblStatusToggle.Text = "Press the button to start!";
            // 
            // notifyIconTray
            // 
            this.notifyIconTray.Text = "notifyIconTray";
            this.notifyIconTray.Visible = true;
            // 
            // ToggleApplicationStateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 134);
            this.Controls.Add(this.lblStatusToggle);
            this.Controls.Add(this.btnStatusToggle);
            this.Controls.Add(this.txtStatusToggleKey);
            this.Name = "ToggleApplicationStateForm";
            this.Text = "ToggleApplicationStateForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStatusToggleKey;
        private System.Windows.Forms.Button btnStatusToggle;
        private System.Windows.Forms.Label lblStatusToggle;
        private System.Windows.Forms.NotifyIcon notifyIconTray;
    }
}