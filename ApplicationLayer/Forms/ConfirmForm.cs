using System;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class ConfirmForm : Form
    {
        public string TokenInput => txtTokenInput.Text.Trim();

        public ConfirmForm()
        {
            InitializeComponent();
            btnOk.Click += btnOk_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
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
    }
}
