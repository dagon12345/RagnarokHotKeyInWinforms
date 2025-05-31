using ApplicationLayer.Interface;
using Domain.Constants;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using RagnarokHotKeyInWinforms;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class SignInForm : Form
    {
        private readonly IGetUserInfo _getUserInfo;
        private string tokenFileName = RagnarokConstants.TokenFileName;
        private string tokenFilePath;
        public SignInForm(IGetUserInfo getUserInfo)
        {
            InitializeComponent();
            _getUserInfo = getUserInfo;
            // Set the token file path based on your application name
            tokenFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Google.Apis.Auth", tokenFileName);
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = RagnarokConstants.ClientId,
                ClientSecret = RagnarokConstants.ClientSecret
            },
             new[] { "email", "profile" },
             "user",
             CancellationToken.None,
             new FileDataStore("Google.Apis.Auth"));



            // Store the access token and login time
            Properties.Settings.Default.AccessToken = credential.Token.AccessToken;
            Properties.Settings.Default.LastLoginTime = DateTime.Now;

            // Retrieve user information
            var userInfo = await _getUserInfo.GetUserInfo(credential.Token.AccessToken);

            Properties.Settings.Default.Name = userInfo.Name;  // Store user name
            Properties.Settings.Default.UserEmail = userInfo.Email;  // Store user email
            Properties.Settings.Default.Save();

            // Open MainMenuForm after successful login
            if (userInfo != null)
            {
                OpenMainMenuForm();
            }

            //NOTE: If the user cannot sign in then delete the folder inside the directory C:\Users\(NameOfPc)\AppData\Roaming
        }

        private void SignInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void SignInForm_Load(object sender, EventArgs e)
        {

            // Check if there's a stored access token
            var lastLoginTime = Properties.Settings.Default.LastLoginTime;
            var storedAccessToken = Properties.Settings.Default.AccessToken;

            //If the logged in time is less than 2 minutes then restore session else delete the existing login creds and sign in again.
            if (!string.IsNullOrEmpty(storedAccessToken) && (DateTime.Now - lastLoginTime).TotalMinutes <= 2)  // Check if within 2 minutes
            {
                // Token is still valid, proceed to MainMenuForm

                OpenMainMenuForm();
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
        private void OpenMainMenuForm()
        {
            this.Hide(); // Hide the LoginForm
            frm_Main mainMenuForm = new frm_Main();
            mainMenuForm.ShowDialog();
        }
    }
}
