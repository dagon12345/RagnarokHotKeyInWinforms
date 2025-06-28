using ApplicationLayer.Dto;
using ApplicationLayer.Validator;
using System.Linq;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class SignInRegsitrationForm : Form
    {
        public SignInRegsitrationForm()
        {
            InitializeComponent();
            btnRegister.Click += btnRegister_Click;
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
        }

        private void btnRegister_Click(object sender, System.EventArgs e)
        {
            //All of the function of this form is inside the SignInForm
            //Validator below
            var dto = new SignInRegistrationDto
            {
                Password = txtPassword.Text.Trim(),
                ConfirmPassword = txtConfirmPassword.Text.Trim(),
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

            this.DialogResult = DialogResult.OK;
        }
    }
}
