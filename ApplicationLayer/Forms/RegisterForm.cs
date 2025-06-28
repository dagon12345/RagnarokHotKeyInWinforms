using ApplicationLayer.Dto;
using ApplicationLayer.Service;
using ApplicationLayer.Validator;
using Infrastructure.Repositories.Interface;
using System.Linq;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly RegistrationService _registerServices;
        private readonly IStoredCredentialRepository _credentialRepository;
        public RegisterForm(RegistrationService registeredServices)
        {
            InitializeComponent();
            _registerServices = registeredServices;
          //  _credentialRepository = storedCredentialRepository;

            txtEmail.Text = "lanceandreiespina@yahoo.com";
            txtPassword.Text = "@September30";
        }

        private RegisterUserDto InitializeDto()
        {
            return new RegisterUserDto
            {
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                ConfirmPassword = txtConfirmPassword.Text.Trim()
            };
        }
        private void btnRegister_Click(object sender, System.EventArgs e)
        {

            //TODO: Search for the existing user first if already existed or else register new
            // Instantiate validator (or inject via DI)
            var dto = InitializeDto();
            var validator = new RegisterUserDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("\n", validationResult.Errors.Select(err => $"• {err.ErrorMessage}"));
                MessageBox.Show(errorMessages, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var email = dto.Email;
            var password = dto.Password;
            _registerServices.Register(email, password);

        }
    }
}
