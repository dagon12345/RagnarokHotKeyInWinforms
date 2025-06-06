using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.Model.DataModels;
using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class ToggleApplicationStateForm : Form, IObserver
    {
        //This 3 properties is from our Utilities from RObserver
        private Subject subject;
        private ContextMenu contextMenu;
        private MenuItem menuItem;
        //Store key used for the last profile - necessarily to clean when change profile
        private Keys lastKey;
        private readonly IUserSettingService _userSettingService;
        private readonly IStoredCredentialService _storedCredentialService;
        private readonly ISignIn _signIn;
        private readonly IBaseTableService _baseTableService;
        public ToggleApplicationStateForm(Subject subject, IUserSettingService userSettingService,
            IStoredCredentialService storedCredentialService, ISignIn signIn, IBaseTableService baseTableService)
        {
            InitializeComponent();
            _userSettingService = userSettingService;
            _storedCredentialService = storedCredentialService; 
            _signIn = signIn;   
            _baseTableService = baseTableService;

            subject.Attach(this);
            this.subject = subject;
            KeyboardHook.Enable();
            InitializeContextualMenu();
        }
        public void Update(ISubject subject)
        {

            if ((subject as Subject).Message.code == MessageCode.PROFILE_CHANGED)
            {
                Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), txtStatusToggleKey.Text);
                KeyboardHook.Remove(lastKey); //Remove last key hook to prevent toggle with last profile key used.

                this.txtStatusToggleKey.Text = currentToggleKey.ToString();
                KeyboardHook.Add(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));
                lastKey = currentToggleKey;
            }
        }

        #region private methods
        private void InitializeContextualMenu()
        {
            this.contextMenu = new ContextMenu();
            this.menuItem = new MenuItem();

            this.contextMenu.MenuItems.AddRange(
                new MenuItem[] { this.menuItem });

            this.menuItem.Index = 0;
            this.menuItem.Text = "Close";
            //this.menuItem.Click += new EventHandler(this.notifyShutdownApplication);

            this.notifyIconTray.ContextMenu = this.contextMenu;

        }
        private bool toggleStatus()
        {
            bool isOn = this.btnStatusToggle.Text == "On";
            if(isOn)
            {
                this.btnStatusToggle.BackColor = Color.Crimson;
                this.btnStatusToggle.Text = "Off";
                //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_off;
                this.subject.Notify(new Utilities.Message(MessageCode.TURN_OFF, null));
                this.lblStatusToggle.Text = "Press the button to start!";
                //new SoundPlayer(Resources._4RTools.ETCResource.Speech_Off).Play();
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if(client != null)
                {
                    this.btnStatusToggle.BackColor = Color.Green;
                    this.btnStatusToggle.Text = "On";
                    //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_on;
                    this.subject.Notify(new Utilities.Message(MessageCode.TURN_ON, null));
                    this.lblStatusToggle.Text = "Press the button to stop!";
                    this.lblStatusToggle.ForeColor = Color.Black;
                    //new SoundPlayer(Resources._4RTools.ETCResource.Speech_On).Play();
                }
                else
                {
                    this.lblStatusToggle.Text = "Please select a valid Ragnarok Client!";
                    this.lblStatusToggle.ForeColor = Color.Red;
                }
            }
            return true;
        }

        private void notifyIconDoubleClick(object sender, MouseEventArgs e)
        {
            this.subject.Notify(new Utilities.Message(MessageCode.CLICK_ICON_TRAY, null));
        }

        private void notifyShutdownApplication(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.subject.Notify(new Utilities.Message(MessageCode.SHUTDOWN_APPLICATION, null));
        }
        #endregion

        private void btnStatusToggle_Click(object sender, EventArgs e)
        {
            this.toggleStatus();
        }


        private async void ToggleApplicationStateForm_Load(object sender, EventArgs e)
        {
            await Retrieve();
        }
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var credential = await _signIn.GoogleAlgorithm(GoogleConstants.GoogleApis);
            var storedCreds = await _storedCredentialService.FindCredential(credential.Token.AccessToken);
            var getBaseTable = await _baseTableService.SearchUser(storedCreds.UserEmail);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }
        private async Task Retrieve()
        {
            var toggleStateValue = await ReturnToggleKey();
            // Parse JSON and extract toggleStateKey
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(toggleStateValue.UserPreferences);
            this.txtStatusToggleKey.Text = jsonObject?["toggleStateKey"];
            txtStatusToggleKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            this.txtStatusToggleKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtStatusToggleKey.TextChanged += async (sender, e) => await onStatusToggleKeyChange(sender, e);
        }

        private async Task onStatusToggleKeyChange(object sender, EventArgs e)
        {
            var toggleStateValue = await ReturnToggleKey();
            //Get last key from profile before update it in json
            Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), txtStatusToggleKey.Text);
            KeyboardHook.Remove(lastKey);
            KeyboardHook.Add(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));


            // Deserialize JSON to update value
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(toggleStateValue.UserPreferences);
            if (jsonObject != null)
            {
                jsonObject["toggleStateKey"] = currentToggleKey.ToString(); // Update key
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                toggleStateValue.UserPreferences = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            lastKey = currentToggleKey; //Refresh lastKey to update 
        }
    }
}
