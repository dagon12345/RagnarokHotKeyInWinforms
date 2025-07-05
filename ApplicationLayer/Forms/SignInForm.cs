using ApplicationLayer.Designer;
using ApplicationLayer.Dto;
using ApplicationLayer.Interface;
using ApplicationLayer.Service;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Validator;
using Domain.Constants;
using Domain.Model;
using Domain.Model.DataModels;
using Domain.Security;
using FluentResults;
using Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using RagnarokHotKeyInWinforms;
using RagnarokHotKeyInWinforms.RagnarokHotKeyInWinforms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;

namespace ApplicationLayer.Forms
{
    public partial class SignInForm : Form
    {
        private readonly IGetUserInfo _getUserInfo;
        private readonly ISignIn _signIn;
        private readonly IStoredCredentialService _storedCredentialService;
        private readonly LoginService _loginService;
        private readonly PasswordRecoveryService _passwordRecoveryService;
        private readonly IUserSettingService _userSettingService;
        private readonly IBaseTableService _baseTableService;
        public SignInForm(IGetUserInfo getUserInfo, ISignIn signIn, IStoredCredentialService storedCredentialService, LoginService loginService,
            PasswordRecoveryService passwordRecoveryService, IUserSettingService userSettingService, IBaseTableService baseTableService)
        {
            InitializeComponent();
            this.Text = AppConfig.Name + " - " + GlobalConstants.Version; // get the latest version
            InitializeCustomComponents();
            Updater();
            _getUserInfo = getUserInfo;
            _signIn = signIn;
            _storedCredentialService = storedCredentialService;
            _loginService = loginService;
            _passwordRecoveryService = passwordRecoveryService;
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;
        }
        private void Updater()
        {
            #region Updater
            string currentVersion = GlobalConstants.Version;
            // 🌐 GitHub URLs
            string versionUrl = GlobalConstants.VersionUrl;
            string zipUrl = GlobalConstants.ZipUrl;

            string zipPath = @".\FerocityInstaller.zip";
            string extractPath = @".\Extracted";

            try
            {
                using (WebClient client = new WebClient())
                {
                    string latestVersion = client.DownloadString(versionUrl).Trim();

                    if (latestVersion != currentVersion)
                    {
                        DialogResult result = MessageBox.Show(
                            $"A new version ({latestVersion}) is available. Do you want to update?",
                            "Ferocity Update",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                        if (result == DialogResult.Yes)
                        {
                            if (File.Exists(zipPath))
                            {
                                File.Delete(zipPath);
                            }

                            client.DownloadFile(zipUrl, zipPath);
                            ZipFile.ExtractToDirectory(zipPath, extractPath);

                            Process msiexecProcess = new Process();
                            msiexecProcess.StartInfo.FileName = "msiexec";
                            msiexecProcess.StartInfo.Arguments = String.Format("/i FerocityInstaller.msi");
                            Application.Exit();
                            msiexecProcess.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update check failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            #endregion
        }

        private void InitializeCustomComponents()
        {
            txtEmail.Location = new System.Drawing.Point(75, 39);
            txtPassword.Location = new System.Drawing.Point(75, 70);
            btnLogin.Location = Location = new System.Drawing.Point(75, 110);
            btnSignIn.Location = Location = new System.Drawing.Point(190, 110);
            btnRegister.Location = Location = new System.Drawing.Point(305, 110);
            lblForgotPassword.Location = new System.Drawing.Point(310, 90);
            txtPassword.UseSystemPasswordChar = true;
            foreach (Control ctrl in this.Controls)
            {
                if(ctrl is Button btn)
                {
                    btn.Width = 100;
                    btn.Height = 35;

                }
                if (ctrl is TextBox txt)
                {
                    txt.Width = 330;
                    txt.Height = 20;
                }
            }
        }

        private void SignInForm_Load(object sender, EventArgs e)
        {
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            lblHeader.Font = new Font("Onyx", 20, FontStyle.Regular);
            lblHeader.Location = new System.Drawing.Point(180, 4);

        }
        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            await CheckCredentialsAsync();
            await SignIn();
        }
        private async Task CheckCredentialsAsync()
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
            if (!string.IsNullOrEmpty(storedAccessToken) && (DateTime.UtcNow - lastLoginTime).TotalMinutes <= 30)  // Check if within 30 minutes
            {
                // Token is still valid, proceed to MainMenuForm
                await CreateUserSettingAsync(searchCredential.UserEmail);
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
        private async Task SignIn()
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
                await _storedCredentialService.SaveChangesAsync(searchUser);
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
            //Search again because once we updated we got lost track of the user data due to settings.
            var searchUserAgain = await _storedCredentialService.SearchUser(userInfo.Email);
            await CreateUserSettingAsync(searchUserAgain.UserEmail);

            //Open Form
            OpenMainMenuForm(searchUserAgain.UserEmail);

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

                    await _storedCredentialService.SaveChangesAsync(user);

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

        private void OpenMainMenuForm(string email)
        {
            this.Hide();
            var userCredentialsService = Program.ServiceProvider.GetRequiredService<IStoredCredentialService>();
            var subjectService = Program.ServiceProvider.GetRequiredService<SubjectService>();
            var mainMenuForm = new frm_Main(email, userCredentialsService, subjectService);
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
        private async Task CreateUserSettingAsync(string userEmail)
        {
            var storedCreds = await _storedCredentialService.SearchUser(userEmail);
            var getBaseTable = await _baseTableService.SearchUser(userEmail);
            await _userSettingService.UpsertUser(getBaseTable.ReferenceCode, storedCreds.Name);
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
                var searchUser = await _storedCredentialService.SearchUser(dto.Email);
                //Update the stored credential for new lastlogin.
                searchUser.LastLoginTime = DateTime.UtcNow;
                await CreateUserSettingAsync(dto.Email);
                await _storedCredentialService.SaveChangesAsync(searchUser);

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
