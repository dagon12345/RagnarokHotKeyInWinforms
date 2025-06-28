using ApplicationLayer.Dto;
using ApplicationLayer.Validator;
using System.Linq;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class ResetPasswordForm : Form
    {
        public ResetPasswordForm()
        {
            InitializeComponent();
            btnOk.Click += btnOk_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            var dto = new SignInRegistrationDto
            {
                Password = txtNewPassword.Text.Trim(),
                ConfirmPassword = txtConfirmPassword.Text.Trim()
            };

            // Instantiate validator (or inject via DI)
            var validator = new SignInRegistrationDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("\n", validationResult.Errors.Select(err => $"• {err.ErrorMessage}"));
                MessageBox.Show(errorMessages, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Please enter the confirmation code.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
