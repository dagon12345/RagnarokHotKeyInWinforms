namespace ApplicationLayer.Forms
{
    partial class SignInForm
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
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRegister
            // 
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.Location = new System.Drawing.Point(311, 105);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(120, 44);
            this.btnRegister.TabIndex = 4;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegisterForm_Click);
            // 
            // btnSignIn
            // 
            this.btnSignIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSignIn.Image = global::ApplicationLayer.Properties.Resources.google_icon_icons_com_62736;
            this.btnSignIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSignIn.Location = new System.Drawing.Point(185, 105);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(120, 44);
            this.btnSignIn.TabIndex = 3;
            this.btnSignIn.Text = "Sign in";
            this.btnSignIn.UseVisualStyleBackColor = true;
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(5, 72);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Password";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(26, 40);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "Email";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(59, 69);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(372, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(59, 37);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(374, 20);
            this.txtEmail.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Location = new System.Drawing.Point(59, 105);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(120, 44);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Onyx", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(196, 4);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(103, 30);
            this.lblHeader.TabIndex = 9;
            this.lblHeader.Text = "Ferocity Tool";
            // 
            // SignInForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 161);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnSignIn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SignInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SignInForm_FormClosed);
            this.Load += new System.EventHandler(this.SignInForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblHeader;
    }
}