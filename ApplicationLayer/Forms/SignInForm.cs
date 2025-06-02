using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.Model.DataModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Microsoft.Extensions.DependencyInjection;
using RagnarokHotKeyInWinforms;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class SignInForm : Form
    {
        private readonly IGetUserInfo _getUserInfo;
        private readonly ISignIn _signIn;
        private readonly IStoredCredentialService _storedCredentialService;
        public SignInForm(IGetUserInfo getUserInfo, ISignIn signIn, IStoredCredentialService storedCredentialService)
        {
            InitializeComponent();

            _getUserInfo = getUserInfo;
            _signIn = signIn;
            _storedCredentialService = storedCredentialService;
           
         
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            await SignIn();
        }
        private async Task SignIn()
        {

            var credential = await _signIn.GoogleAlgorithm(GoogleConstants.GoogleApis);
            // Retrieve user information
            var userInfo = await _getUserInfo.GetUserInfo(credential.Token.AccessToken);
            //Search through stored credential database
            var searchUser = await _storedCredentialService.SearchUser(userInfo.Email);
            if (searchUser == null)
            {
                var storedCredential = new StoredCredential();
                var accessToken = credential.Token.AccessToken;
                var lastTimeLogin = DateTime.Now;
                var Name = userInfo.Name;
                var Email = userInfo.Email;
                //save once captured
                await _storedCredentialService.SaveCredentials(storedCredential, accessToken, lastTimeLogin,
                    Name, Email);
            }
            else
            {
                //Update the stored credential for new lastlogin and accesstoken
                searchUser.LastLoginTime = DateTime.Now;
                searchUser.AccessToken = credential.Token.AccessToken;
                await _storedCredentialService.SaveChanges();
            }

            //Search existed user
            var searchExisitingUser = await _signIn.SearchExistingUser(userInfo.Email);
            if (searchExisitingUser == null)
            {
                //if not existed then add to database Email/ReferenceCode.
                BaseTable baseTable = new BaseTable();
                await _signIn.CreateUser(baseTable, userInfo.Email);
            }
            //Open Form
            OpenMainMenuForm();

            //NOTE: If the user cannot sign in then delete the folder inside the directory C:\Users\(NameOfPc)\AppData\Roaming
        }
        private void SignInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
        private async void SignInForm_Load(object sender, EventArgs e)
        {
            // Set the token file path based on your application name
            var tokenFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                GoogleConstants.GoogleApis, GoogleConstants.TokenFileName);
            // Check if there's a stored access token
            var credential = await _signIn.GoogleAlgorithm(GoogleConstants.GoogleApis);
            var searchCredential = await _storedCredentialService.FindCredential(credential.Token.AccessToken);
            if(searchCredential == null)
            {
                return;
            }
            var lastLoginTime = searchCredential.LastLoginTime;
            var storedAccessToken = searchCredential.AccessToken;

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
            var storedCredential = Program.ServiceProvider.GetRequiredService<IStoredCredentialService>();
            var userSignIn = Program.ServiceProvider.GetRequiredService<ISignIn>();
            frm_Main mainMenuForm = new frm_Main(storedCredential, userSignIn);
            mainMenuForm.ShowDialog();
        }
    }
}
