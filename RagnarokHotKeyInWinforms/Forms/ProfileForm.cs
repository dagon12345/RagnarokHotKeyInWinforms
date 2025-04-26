using RagnarokHotKeyInWinforms.Model;
using System.Windows.Forms;
using static RagnarokHotKeyInWinforms.Model.ProfileSingleton;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class ProfileForm : Form
    {
        private frm_Main formMain; // Our Form
        public ProfileForm(frm_Main formMain)
        {
            InitializeComponent();
            this.formMain = formMain;

            foreach(string profile in Profile.ListAll())
            {
                if(profile != "Default") { this.lbProfilesList.Items.Add(profile); };
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            string newProfileName = this.txtProfileName.Text;
            if(string.IsNullOrEmpty(newProfileName)) { return; } //  If Empty then return default.

            ProfileSingleton.Create(newProfileName);
            this.lbProfilesList.Items.Add(newProfileName);
            this.formMain.refreshProfileList();
            this.txtProfileName.Text = ""; // Clear textbox
        }

        private void btnRemoveProfile_Click(object sender, System.EventArgs e)
        {
            //NOTE: lbProfileList is a ListBox
            if(this.lbProfilesList.SelectedItem == null) // if null
            {
                MessageBox.Show("No profile found! To delete a profile, first select an option from the Profile list.");
                return;
            }
            string selectedProfile = this.lbProfilesList.SelectedItem.ToString(); //Selected item in the ListBox
            if (selectedProfile == "Default")
            {
                MessageBox.Show("Cannot delete a Default profile!");//Don't remove the Default profile
            }
            else
            {
                ProfileSingleton.Delete(selectedProfile);
                this.lbProfilesList.Items.Remove(selectedProfile); // Remove the created profile
                this.formMain.refreshProfileList();//refresh the combobox Profile selection
            }
        }
    }
}
