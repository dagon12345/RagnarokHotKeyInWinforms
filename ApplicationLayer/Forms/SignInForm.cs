using ApplicationLayer.Interface;
using Domain.Constants;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using RagnarokHotKeyInWinforms;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ApplicationLayer.Forms
{
    public partial class SignInForm : Form
    {
        private readonly IGetUserInfo _getUserInfo;
        public SignInForm(IGetUserInfo getUserInfo)
        {
            InitializeComponent();
            _getUserInfo = getUserInfo;
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


            // Retrieve user information
            var userInfo = await _getUserInfo.GetUserInfo(credential.Token.AccessToken);

            if (userInfo != null)
            {
                MessageBox.Show($"Welcome {userInfo.Name}!");
                frm_Main main = new frm_Main();
                this.Hide();
                main.ShowDialog();
            }
            //NOTE: If the user cannot sign in then delete the folder inside the directory C:\Users\(NameOfPc)\AppData\Roaming
        }

        private void SignInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
