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
        private Label label;
        private TextBox textBox;
        private Button button;
        private Label labelAssigned;
        private Keys lastKey;
        private SubjectService subject = new SubjectService();//subject triggers the Update() method inside notify function
        private readonly IBaseTableService _baseTableService;
        private readonly IUserSettingService _userSettingService;
        public string email;//get the users email from login

        public ToggleApplicationForm(IBaseTableService baseTableService,
            IUserSettingService userSettingService)
        {
            InitializeComponent();
            InitializeCustomComponents();

            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);

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
        #region Design Region
        private void InitializeCustomComponents()
        {
            //Label (left, right)
            label = new Label();
            label.Text = "Click to start!";
            label.Location = new System.Drawing.Point(175, 12);
            label.AutoSize = true;

            labelAssigned = new Label();
            labelAssigned.Text = "Key";
            labelAssigned.Location = new System.Drawing.Point(0, 10);
            labelAssigned.AutoSize = true;

            // TextBox
            textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(30, 10);
            textBox.Width = 50;
            textBox.Height = 20;


            // Button
            button = new Button();
            button.Text = "Start";
            button.Location = new System.Drawing.Point(100, 10);
            button.Cursor = Cursors.Hand;
            button.Click += Button_Click;
            button.Width = 70;
            button.Height = 20;


            // Add controls to form
            this.Controls.Add(label);
            this.Controls.Add(textBox);
            this.Controls.Add(button);
            this.Controls.Add(labelAssigned);
        }
        
        #endregion

        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }

        #region ToggleApplicationStateFunction (No Start Method)
        private bool toggleStatus()
        {
            bool isOn = this.button.Text == "On";
            if (isOn)
            {
                this.label.BackColor = Color.Crimson;
                this.button.Text = "Off";
                //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_off;
                this.subject.Notify(new Utilities.Message(MessageCode.TURN_OFF, null));
                this.label.Text = "Press the button to start!";
                //new SoundPlayer(Resources._4RTools.ETCResource.Speech_Off).Play();
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if (client != null)
                {
                    this.button.BackColor = Color.Green;
                    this.button.Text = "On";
                    //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_on;
                    this.subject.Notify(new Utilities.Message(MessageCode.TURN_ON, null));
                    this.label.Text = "Press the button to stop!";
                    this.label.ForeColor = Color.Black;
                    //new SoundPlayer(Resources._4RTools.ETCResource.Speech_On).Play();
                }
                else
                {
                    this.label.Text = "Please select a valid Ragnarok Client!";
                    this.label.ForeColor = Color.Red;
                }
            }
            return true;
        }
        private async Task Retrieve()
        {
            var toggleStateValue = await ReturnToggleKey();
            // Parse JSON and extract toggleStateKey
            var jsonObject = JsonSerializer.Deserialize<UserPreferences>(toggleStateValue.UserPreferences);
            this.textBox.Text = jsonObject.toggleStateKey;

            textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            this.textBox.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            this.textBox.TextChanged += async (sender, e) => await onStatusToggleKeyChange(sender, e);

        }
        private async Task onStatusToggleKeyChange(object sender, EventArgs e)
        {
            var toggleStateValue = await ReturnToggleKey();
            //Get last key from profile before update it in json
            Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), textBox.Text);
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
