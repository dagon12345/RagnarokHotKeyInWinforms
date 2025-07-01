using ApplicationLayer.Designer;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class ConfirmForm : Form
    {
        public string TokenInput => txtTokenInput.Text.Trim();

        public ConfirmForm()
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            btnOk.Click += btnOk_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            txtMessage.ReadOnly = true;
            txtMessage.TextAlign = HorizontalAlignment.Center;
            txtMessage.Font = new Font("Segoe UI", 8, FontStyle.Bold);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTokenInput.Text))
            {
                MessageBox.Show("Please enter the confirmation code.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void ConfirmForm_Load(object sender, EventArgs e)
        {

        }
    }
}
