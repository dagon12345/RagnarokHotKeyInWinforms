using ApplicationLayer.Designer;
using ApplicationLayer.Interface;
using ApplicationLayer.Models.RagnarokModels;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Singleton.RagnarokSingleton;
using Domain.Constants;
using Domain.Model.DataModels;
using Infrastructure.Utilities;
using System;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationLayer.ChildForms
{
    public partial class ToggleApplicationForm : Form
    {
       
        private Keys lastKey;
        private SubjectService subject = new SubjectService();//subject triggers the Update() method inside notify function
        private readonly IBaseTableService _baseTableService;
        private readonly IUserSettingService _userSettingService;
        public string email;//get the users email from login

        public ToggleApplicationForm(IBaseTableService baseTableService,
            IUserSettingService userSettingService)
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            Designer();
            _baseTableService = baseTableService;
            _userSettingService = userSettingService;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _ = LoadAsync(); // Fire and forget safely
        }
        private async Task LoadAsync()
        {
            try
            {
                await Retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            this.toggleStatus();
        }
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }
        #region Designer
        private void Designer()
        {
            btnStart.Click += Button_Click;
            txtNotification.ReadOnly = true;
            txtNotification.BackColor = Color.FromArgb(23, 32, 42); // Deep navy
        }
        #endregion
        #region ToggleApplicationStateFunction (No Start Method)
        private bool toggleStatus()
        {
            bool isOn = this.btnStart.Text == "On";
            if (isOn)
            {
                //this.label.BackColor = Color.Crimson;
                this.btnStart.Text = "Off";
                //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_off;
                this.subject.Notify(new Utilities.Message(MessageCode.TURN_OFF, null));
                //this.label.Text = "Press the button to start!";
                //new SoundPlayer(Resources._4RTools.ETCResource.Speech_Off).Play();
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if (client != null)
                {
                    this.btnStart.BackColor = Color.Green;
                    this.btnStart.Text = "On";
                    //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_on;
                    this.subject.Notify(new Utilities.Message(MessageCode.TURN_ON, null));
                    //this.label.Text = "Press the button to stop!";
                   // this.label.ForeColor = Color.Black;
                    //new SoundPlayer(Resources._4RTools.ETCResource.Speech_On).Play();
                }
                else
                {
                    this.txtNotification.Text = "Please select a valid Ragnarok Client!";
                    this.txtNotification.ForeColor = Color.Red;
                }
            }
            return true;
        }
        private async Task Retrieve()
        {
            var toggleStateValue = await ReturnToggleKey();
            // Parse JSON and extract toggleStateKey
            var jsonObject = JsonSerializer.Deserialize<UserPreferences>(toggleStateValue.UserPreferences);
            this.txtKey.Text = jsonObject.toggleStateKey;

            txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            this.txtKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            this.txtKey.TextChanged += async (sender, e) => await onStatusToggleKeyChange(sender, e);

        }
        private async Task onStatusToggleKeyChange(object sender, EventArgs e)
        {
            var toggleStateValue = await ReturnToggleKey();
            //Get last key from profile before update it in json
            Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), txtKey.Text);
            KeyboardHook.Remove(lastKey);
            KeyboardHook.Add(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));


            // Deserialize JSON to update value
            var jsonObject = JsonSerializer.Deserialize<UserPreferences>(toggleStateValue.UserPreferences);
            if (jsonObject != null)
            {
                jsonObject.toggleStateKey = currentToggleKey.ToString(); // Update key
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                toggleStateValue.UserPreferences = updatedJson;

                // Persist changes
                await _userSettingService.SaveChangesAsync(toggleStateValue);
            }
            else
            {
                return;
            }
            lastKey = currentToggleKey; //Refresh lastKey to update 
        }
        #endregion ToggleApplicationStateFunction


    }
}
