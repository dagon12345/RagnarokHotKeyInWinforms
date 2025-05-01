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
            this.notifyIconTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblStatusToggle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtStatusToggleKey
            // 
            this.txtStatusToggleKey.Location = new System.Drawing.Point(86, 53);
            this.txtStatusToggleKey.Multiline = true;
            this.txtStatusToggleKey.Name = "txtStatusToggleKey";
            this.txtStatusToggleKey.Size = new System.Drawing.Size(100, 40);
            this.txtStatusToggleKey.TabIndex = 0;
            // 
            // btnStatusToggle
            // 
            this.btnStatusToggle.Location = new System.Drawing.Point(86, 24);
            this.btnStatusToggle.Name = "btnStatusToggle";
            this.btnStatusToggle.Size = new System.Drawing.Size(100, 23);
            this.btnStatusToggle.TabIndex = 1;
            this.btnStatusToggle.Text = "Off";
            this.btnStatusToggle.UseVisualStyleBackColor = true;
            this.btnStatusToggle.Click += new System.EventHandler(this.btnStatusToggle_Click);
            // 
            // notifyIconTray
            // 
            this.notifyIconTray.Text = "notifyIconTray";
            this.notifyIconTray.Visible = true;
            // 
            // lblStatusToggle
            // 
            this.lblStatusToggle.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatusToggle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblStatusToggle.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusToggle.Location = new System.Drawing.Point(12, 99);
            this.lblStatusToggle.Name = "lblStatusToggle";
            this.lblStatusToggle.ReadOnly = true;
            this.lblStatusToggle.Size = new System.Drawing.Size(243, 16);
            this.lblStatusToggle.TabIndex = 2;
            this.lblStatusToggle.Text = "Press the button to start!";
            this.lblStatusToggle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ToggleApplicationStateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 134);
            this.Controls.Add(this.lblStatusToggle);
            this.Controls.Add(this.btnStatusToggle);
            this.Controls.Add(this.txtStatusToggleKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ToggleApplicationStateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ToggleApplicationStateForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStatusToggleKey;
        private System.Windows.Forms.Button btnStatusToggle;
        private System.Windows.Forms.NotifyIcon notifyIconTray;
        private System.Windows.Forms.TextBox lblStatusToggle;
    }
}