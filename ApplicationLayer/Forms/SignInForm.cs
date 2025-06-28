using ApplicationLayer.Dto;
using ApplicationLayer.Interface;
using ApplicationLayer.Service;
using ApplicationLayer.Validator;
using Domain.Constants;
using Domain.Model.DataModels;
using Domain.Security;
using FluentResults;
using Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RagnarokHotKeyInWinforms;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class SignInForm : Form
    {
        private readonly IGetUserInfo _getUserInfo;
        private readonly ISignIn _signIn;
        private readonly IStoredCredentialService _storedCredentialService;
        private readonly LoginService _loginService;
        private readonly PasswordRecoveryService _passwordRecoveryService;
        public SignInForm(IGetUserInfo getUserInfo, ISignIn signIn, IStoredCredentialService storedCredentialService, LoginService loginService,
            PasswordRecoveryService passwordRecoveryService)
        {
            InitializeComponent();
            _getUserInfo = getUserInfo;
            _signIn = signIn;
            _storedCredentialService = storedCredentialService;
            _loginService = loginService;
            _passwordRecoveryService = passwordRecoveryService;
        }


        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            bool fromLogin = false;
            await CheckCredentialsAsync(fromLogin);
            await SignIn(fromLogin);
        }
        private async Task CheckCredentialsAsync(bool fromLogin)
        {
            // Set the token file path based on your application name
            var tokenFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                GoogleConstants.GoogleApis, GoogleConstants.TokenFileName);
            // Check if there's a stored access token
            var credential = await _signIn.GoogleAlgorithm(GoogleConstants.GoogleApis);
            var searchCredential = await _storedCredentialService.FindCredential(credential.Token.AccessToken);
            if (searchCredential == null)
            {
                return;
            }
            var lastLoginTime = searchCredential.LastLoginTime;
            var storedAccessToken = searchCredential.AccessToken;

            //If the logged in time is less than 30 minutes then restore session else delete the existing login creds and sign in again.
            if (!string.IsNullOrEmpty(storedAccessToken) && (DateTime.UtcNow - lastLoginTime).TotalMinutes <= 10)  // Check if within 30 minutes
            {
                // Token is still valid, proceed to MainMenuForm
                OpenMainMenuForm(searchCredential.UserEmail);
            }
            // Delete the token response file if it exists
            else if (File.Exists(tokenFilePath))
            {
                try
                {
                    File.Delete(tokenFilePath); // Delete the token response file

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting token file: {ex.Message}");
                }
            }
        }
        private async Task SignIn(bool fromLogin)
        {

            var credential = await _signIn.GoogleAlgorithm(GoogleConstants.GoogleApis);
            // Retrieve user information
            var userInfo = await _getUserInfo.GetUserInfo(credential.Token.AccessToken);
            //Search through stored credential database
            var searchUser = await _storedCredentialService.SearchUser(userInfo.Email);
            var accessToken = credential.Token.AccessToken; 
            //Add new registered user if null
            if (searchUser == null)
            {
                var storedCredential = new StoredCredential
                {
                    AccessToken = accessToken,
                    LastLoginTime = DateTime.UtcNow,
                    Name = userInfo.Name,
                    UserEmail = userInfo.Email,
                    IsEmailConfirmed = true
                };

                //save once captured
                await _storedCredentialService.SaveCredentials(storedCredential);
                //Open the form once saved
                await OpenRegisterFormCreatePasswordAsync(storedCredential);

            }
            //If the search user is not null buts its value salt and password has was null then we open the register form for password creating.
            else if(searchUser.Salt.IsNullOrEmpty() && searchUser.PasswordHash.IsNullOrEmpty())
            {
                await OpenRegisterFormCreatePasswordAsync(searchUser);
                return;
            }
            else
            {
                //Update the stored credential for new lastlogin and accesstoken
                searchUser.LastLoginTime = DateTime.UtcNow;
                searchUser.AccessToken = credential.Token.AccessToken;
                await _storedCredentialService.SaveChangesAsync();
            }

            //Search existed user
            var searchExisitingUser = await _signIn.SearchExistingUser(userInfo.Email);
            if (searchExisitingUser == null)
            {
                //if not existed then add to database Email/ReferenceCode.
                BaseTable baseTable = new BaseTable
                {
                    ReferenceCode = Guid.NewGuid(),
                    Email = userInfo.Email,
                };
                await _signIn.CreateUser(baseTable);
            }
            //Open Form
            OpenMainMenuForm(searchUser.UserEmail);

            //NOTE: If the user cannot sign in then delete the folder inside the directory C:\Users\(NameOfPc)\AppData\Roaming
        }

        private async Task OpenRegisterFormCreatePasswordAsync(StoredCredential user)
        {
            //After registration open the confirmation form
            using (var signInRegistrationForm = new SignInRegsitrationForm()) // TextBox + OK button
            {
                Result result = new Result();

               // registerForm.ShowDialog();
                signInRegistrationForm.txtName.Text = user.Name;
                signInRegistrationForm.txtName.ReadOnly = true;
                signInRegistrationForm.txtEmail.Text = user.UserEmail;
                signInRegistrationForm.txtEmail.ReadOnly = true;

                if (signInRegistrationForm.ShowDialog() == DialogResult.OK)
                {
                   
                    var salt = SecurityHelper.GenerateSalt();
                    var hash = SecurityHelper.HashPassword(signInRegistrationForm.txtPassword.Text, salt);

                    //Set the password hashing and salting
                    user.Salt = salt;
                    user.PasswordHash = hash;
                    //Presist changes

                    await _storedCredentialService.SaveChangesAsync();

                    if (result.IsSuccess)
                    {
                        MessageBox.Show(result.Successes.FirstOrDefault()?.Message ?? "Registered successfully. Login now or click sign in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "Something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    //If the confirmation cancelled
                    MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "The registration cancelled", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        public static class Theme
        {
            public static Color BackgroundColor => Color.FromArgb(4, 47, 102);
            public static Color PrimaryColor => Color.FromArgb(33, 150, 243); 
            public static Color AccentColor => Color.FromArgb(255, 87, 34);      
            public static Color TextColor => Color.White;
            public static Color ButtonBackGroundColor => Color.FromArgb(25, 121, 169);

            //Font element
            public static Font DefaultFont => new Font("Segoe UI", 10, FontStyle.Regular);
            public static Font BoldFont => new Font("Segoe UI Semibold", 9, FontStyle.Bold);
        }

        private void ApplyDarkBlueTheme()
        {
            this.BackColor = Color.FromArgb(23, 32, 42); // Deep navy background
            txtPassword.UseSystemPasswordChar = true;
            lblHeader.Left = (this.ClientSize.Width - lblHeader.Width) / 2;
            foreach (Control ctrl in this.Controls)
            {
                ctrl.ForeColor = Color.White;

                if (ctrl is TextBox tb)
                {
                    tb.BackColor = Color.FromArgb(33, 47, 61);
                    tb.ForeColor = Color.White;
                    tb.BorderStyle = BorderStyle.FixedSingle;

                }

                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(41, 128, 185);
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }
        }


        private void SignInForm_Load(object sender, EventArgs e)
        {
            ApplyDarkBlueTheme();

        }
        private void OpenMainMenuForm(string email)
        {
            this.Hide(); // Hide the LoginForm
            var storedCredential = Program.ServiceProvider.GetRequiredService<IStoredCredentialService>();
            var userSignIn = Program.ServiceProvider.GetRequiredService<ISignIn>();
            var userSettings = Program.ServiceProvider.GetRequiredService<IUserSettingService>();
            var baseTable = Program.ServiceProvider.GetRequiredService<IBaseTableService>();
            var hashHer = Program.ServiceProvider.GetRequiredService<IHasher>();

            frm_Main mainMenuForm = new frm_Main(storedCredential, userSignIn, userSettings, baseTable, hashHer, email);
            mainMenuForm.ShowDialog();
        }

        private void SignInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnRegisterForm_Click(object sender, EventArgs e)
        {
            var registerService = Program.ServiceProvider.GetRequiredService<IRegistrationService>();
            var userCredentialsService = Program.ServiceProvider.GetRequiredService<IStoredCredentialService>();
            var hashHer = Program.ServiceProvider.GetRequiredService<IHasher>();
            var emailService = Program.ServiceProvider.GetRequiredService<IEmailService>();

            RegisterForm registerFormOpen = new RegisterForm(registerService, userCredentialsService, hashHer, emailService);
            registerFormOpen.ShowDialog();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var dto = new LoginDto
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text,
            };
            // Instantiate validator (or inject via DI)
            var validator = new LoginDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("\n", validationResult.Errors.Select(err => $"• {err.ErrorMessage}"));
                MessageBox.Show(errorMessages, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var login = await _loginService.Login(dto.Email, dto.Password);
            if(login)
            {
                OpenMainMenuForm(dto.Email);
                return;
            }
            MessageBox.Show("Invalid login credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void lblForgotPassword_Click(object sender, EventArgs e)
        {
            var dto = new EmailDto
            {
                Email = txtEmail.Text.Trim(),
            };

            // Instantiate validator (or inject via DI)
            var validator = new EmailDtoValidator();
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("\n", validationResult.Errors.Select(err => $"• {err.ErrorMessage}"));
                MessageBox.Show(errorMessages, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = await _passwordRecoveryService.SendResetLink(dto.Email);
            await OpenConfirmationAsync(user);
           
        }
        private async Task OpenConfirmationAsync(StoredCredential credential)
        {
            //Same confirmation form for registration
            using (var confirmForm = new ConfirmForm()) // TextBox + OK button
            {
                Result result = new Result();
                if (confirmForm.ShowDialog() == DialogResult.OK)
                {
                    var inputToken = confirmForm.txtTokenInput.Text;
                    var retrievedUser = await _passwordRecoveryService.ConfirmationOfToken(credential.UserEmail, inputToken);

                    if (retrievedUser != null)
                    {
                        using(var resetPassword = new ResetPasswordForm())
                        {
                            if(resetPassword.ShowDialog() == DialogResult.OK)
                            {
                                var reset = await _passwordRecoveryService.ResetPassword(retrievedUser.UserEmail, resetPassword.txtConfirmPassword.Text);
                                if(reset.IsSuccess)
                                {
                                    MessageBox.Show(result.Successes.FirstOrDefault()?.Message ?? "Password resetted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "Something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }

                }
                else
                {
                    //If the confirmation cancelled
                    MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "Cancelled the confirmation", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
