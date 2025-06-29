using ApplicationLayer.Designer;
using ApplicationLayer.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Windows.Forms;

namespace ApplicationLayer.ChildForms
{
    public partial class LogoutForm : Form
    {
        private IStoredCredentialService _storedCredentialService;
        public string _userEmail;
        public LogoutForm(IStoredCredentialService storedCredentialService)
        {

            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            InitializeLogoutAsync();
            _storedCredentialService = storedCredentialService;
        }
        private async void InitializeLogoutAsync()
        {

            if (!_userEmail.IsNullOrEmpty())
            {

                //Set the LastLoginTime to munis 10 mins so we can trigger the function deleting of google current sign in
                try
                {
                    var searchUser = await _storedCredentialService.SearchUser(_userEmail);
                    searchUser.LastLoginTime = searchUser.LastLoginTime.AddHours(-1);//Minus One hour so that it will surely go to the google auth again
                    await _storedCredentialService.SaveChangesAsync(searchUser);
                }
                catch (Exception)
                {

                    return;
                }
                DialogResult = DialogResult.OK;
            }

        }

    }
}
