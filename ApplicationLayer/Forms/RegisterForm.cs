using ApplicationLayer.Designer;
using ApplicationLayer.Dto;
using ApplicationLayer.Interface;
using ApplicationLayer.Validator;
using Domain.Model.DataModels;
using Domain.Security;
using FluentResults;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly IRegistrationService _registerServices;
        private readonly IStoredCredentialService _storedCredentialsService;
        private readonly IHasher _hasher;
        private readonly IEmailService _emailService;
        public RegisterForm(IRegistrationService registeredServices, IStoredCredentialService storedCredentialService,
            IHasher hasher, IEmailService emailService)
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            _registerServices = registeredServices;
            _storedCredentialsService = storedCredentialService;

            //Set the Password and Confirm password to password characters
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            _hasher = hasher;
            _emailService = emailService;
        }

        private RegisterUserDto InitializeDto()
        {
            return new RegisterUserDto
            {
                Name = txtName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                ConfirmPassword = txtConfirmPassword.Text.Trim()
            };
        }
        private async void btnRegister_Click(object sender, System.EventArgs e)
        {
            var dto = InitializeDto();

            // Instantiate validator (or inject via DI)
            var validator = new RegisterUserDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("\n", validationResult.Errors.Select(err => $"• {err.ErrorMessage}"));
                MessageBox.Show(errorMessages, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Search for the existing user first if already existed or else register new
            var searchExistingUser = await SearchExistingUserAsync(dto.Email);
            if (searchExistingUser != null)
            {
                return; // User already handled via MessageBox inside the search method
            }

            //Register
            var name = dto.Name;
            var email = dto.Email;
            var password = dto.Password;
            var user = await _registerServices.Register(email, password, name);
            //After registration open the confirmation form
            await OpenConfirmationAsync(user);

        }
        private async Task<StoredCredential> SearchExistingUserAsync(string email)
        {
            var user =  await _storedCredentialsService.SearchUser(email);
            if (user == null) return null;
            if (user.UserEmail == email && user.IsEmailConfirmed == true && user.PasswordHash != null && user.Salt != null)
            {
                MessageBox.Show("This user already registered", "Already exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return user;    
            }
            if(user.UserEmail == email && user.IsEmailConfirmed == false)
            {
                if(MessageBox.Show("This user already registered but the code was not confirmed. Do you want to confirm?", "Already exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string rawGuid = Guid.NewGuid().ToString("N"); // 32-character hex without dashes
                    string confirmationCode = rawGuid.Substring(0, 6).ToUpper(); // E.g., "A2C9FD"

                    user.SetConfirmationCode(confirmationCode, _hasher, TimeSpan.FromMinutes(15));

                    //Once update it will garbage collect
                    await _storedCredentialsService.SaveChangesAsync(user);
                    //Send the email
                    await _emailService.SendConfirmationLinkAsync(user.UserEmail, confirmationCode);

                    //After registration open the confirmation form
                    await OpenConfirmationAsync(user);

                }
                else
                {
                    return user;
                }
            }
            return user;
        }
        private async Task OpenConfirmationAsync(StoredCredential user)
        {
            //After registration open the confirmation form
            using (var confirmForm = new ConfirmForm()) // TextBox + OK button
            {
                Result result = new Result();
                if (confirmForm.ShowDialog() == DialogResult.OK)
                {
                    var inputToken = confirmForm.txtTokenInput.Text;
                    result = await _registerServices.ConfirmEmail(user.UserEmail, inputToken);

                    if (result.IsSuccess)
                    {
                        MessageBox.Show(result.Successes.FirstOrDefault()?.Message ?? "User registration completed successfully!", "Success");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "Something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //If the confirmation cancelled
                    MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "User registered successfully, but cancelled the confirmation", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
