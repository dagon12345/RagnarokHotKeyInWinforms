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
        public ToggleApplicationForm(IBaseTableService baseTableService,
            IUserSettingService userSettingService)
        {
            InitializeComponent();
            InitializeCustomComponents();
            ApplyDarkBlueTheme();

            _baseTableService = baseTableService;
            _userSettingService = userSettingService;
        }
        /*
         * Location = new Point(x, y)
           x – Horizontal distance in pixels from the left edge of the form.
           
           y – Vertical distance in pixels from the top edge of the form.
           
           So for new Point(20, 20):
           
           The control is placed 20 pixels from the left
           
           And 20 pixels down from the top
           
           Think of it like a coordinate system with the origin (0,0) at the top-left corner of the form.
         */
        private void InitializeCustomComponents()
        {
            //Label
            label = new Label();
            label.Text = "Click to start!";
            label.Location = new System.Drawing.Point(70, 20);
            label.AutoSize = true;

            labelAssigned = new Label();
            labelAssigned.Text = "Key";
            labelAssigned.Location = new System.Drawing.Point(0, 50);
            labelAssigned.AutoSize = true;

            // TextBox
            textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(30, 50);
            textBox.Width = 150;
            

            // Button
            button = new Button();
            button.Text = "Start";
            button.Location = new System.Drawing.Point(30, 90);
            button.Cursor = Cursors.Hand;
            button.Click += Button_Click;
            button.Width = 150;

            // Add controls to form
            this.Controls.Add(label);
            this.Controls.Add(textBox);
            this.Controls.Add(button);
            this.Controls.Add(labelAssigned);

            // Form properties
            this.Text = "Toggle Application";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(200, 150);
            this.FormBorderStyle = FormBorderStyle.None;
        }
        private void ApplyDarkBlueTheme()
        {
            this.BackColor = Color.FromArgb(23, 32, 42); // Deep navy background
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

                if(ctrl is Label lbl)
                {
                    lbl.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                }
            }
        }
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey(string userEmail)
        {
            var getBaseTable = await _baseTableService.SearchUser(userEmail);
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
        private async Task Retrieve(string userEmail)
        {
            var toggleStateValue = await ReturnToggleKey(userEmail);
            // Parse JSON and extract toggleStateKey
            var jsonObject = JsonSerializer.Deserialize<UserPreferences>(toggleStateValue.UserPreferences);
            this.textBox.Text = jsonObject.toggleStateKey;
            textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            this.textBox.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            this.textBox.TextChanged += async (sender, e) => await onStatusToggleKeyChange(sender, e);

        }
        private async Task onStatusToggleKeyChange(object sender, EventArgs e)
        {
            var email = "";
            var toggleStateValue = await ReturnToggleKey(email);
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
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
            lastKey = currentToggleKey; //Refresh lastKey to update 
        }
        #endregion ToggleApplicationStateFunction

        private void Button_Click(object sender, EventArgs e)
        {
            this.toggleStatus();
        }

    }
}
