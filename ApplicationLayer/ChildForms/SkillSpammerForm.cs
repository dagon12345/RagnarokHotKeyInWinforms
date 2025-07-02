using ApplicationLayer.Models.RagnarokModels;
using Infrastructure.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Text.Json;
using Domain.Model.DataModels;
using ApplicationLayer.Interface;
using ApplicationLayer.Designer;
using ApplicationLayer.Utilities;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Interface.RagnarokInterface;
using ApplicationLayer.Singleton.RagnarokSingleton;
using Domain.Constants;
using System.Threading;

namespace ApplicationLayer.ChildForms
{
    public partial class SkillSpammerForm : Form, IObserverService
    {
        private readonly IUserSettingService _userSettingService;
        private readonly IBaseTableService _baseTableService;
        private readonly SubjectService _subjectService;
        private ThreadUtility ThreadUtility;
        private CancellationTokenSource _debounceTokenSource;
        public string email;
        public SkillSpammerForm(IUserSettingService userSettingService, IBaseTableService baseTableService, SubjectService subjectService)
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            _subjectService = subjectService;
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;
        }

        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _subjectService.Attach(this);
            _ = LoadAsync();
        }
        public async void Update(ISubjectService subject)
        {
            switch ((subject as SubjectService).Message.code)
            {
                case MessageCode.TURN_ON:
                    await TriggerStartActions();
                    break;
                case MessageCode.TURN_OFF:
                    TriggerStopActions();
                    break;
            }
        }


        private async Task<T> GetDeserializedObject<T>(Func<Task<string>> getJsonData)
        {
            var jsonData = await getJsonData();
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        private async Task TriggerStartActions()
        {
            Client client = ClientSingleton.GetClient();
            var jsonObjectAhk = await GetDeserializedObject<Ahk>(async () => (await ReturnToggleKey()).Ahk);
            ThreadUtility = new ThreadUtility(_ =>
            {
                jsonObjectAhk?.AHKThreadExecution(client);
                Task.Delay(50).Wait(); // Safe exit
            });
        }
        private void TriggerStopActions()
        {
            ThreadUtility?.Stop();
        }

        private async Task LoadAsync()
        {
            try
            {
                await AhkRetrieval();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #region Ahk Region (Triggered with Start() method)
        private void DisableControlsIfSpeedBoost(Ahk jsonObject)
        {
            if (jsonObject.ahkMode == Ahk.SPEED_BOOST)
            {
                this.ahkSpeedBoost.Checked = true;
                this.chkMouseFlick.Enabled = false;
                this.chkNoShift.Enabled = false;
            }
            else
            {
                this.ahkCompatibility.Checked = true;
                this.chkMouseFlick.Enabled = true;
                this.chkNoShift.Enabled = true;
            }
        }

        private async Task AhkRetrieval()
        {
            //Default values of legend
            SetLegendDefaultValues();

            //remove handlers so that it wont trigger the onCheckedChange on loop
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox check)
                {
                    if (check.Enabled)
                        check.CheckStateChanged -= onCheckChange; // Remove event handler before modification
                }
            }
            foreach (Control c in this.groupAhkConfig.Controls)
            {
                if (c is RadioButton check)
                {
                    if (check.Enabled)
                        check.CheckedChanged -= ahkCompatibility_CheckedChanged; // Remove event handler before modification
                }
            }

            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Ahk>(userToggleState.Ahk);
            txtSpammerDelay.Text = jsonObject.AhkDelay.ToString() ?? "0";
            DisableControlsIfSpeedBoost(jsonObject);
            #region Keys that have key
            Dictionary<string, KeyConfig> ahkClones = new Dictionary<string, KeyConfig>(jsonObject.AhkEntries);
            foreach (KeyValuePair<string, KeyConfig> config in ahkClones)
            {
                ToggleCheckboxByName(config.Key, config.Value.ClickActive);
            }

            //Check the tab spammer for checkboxes. This will be triggered by the method ToggleCheckboxByName on loop.
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    if ((check.Name.Split(new[] { "chk" }, StringSplitOptions.None).Length == 2))
                    {
                        check.ThreeState = true; // Include the CheckState.Indeterminate three state insted of just true or false
                    };

                    if (check.Enabled)
                        check.CheckStateChanged += onCheckChange;
                }
            }
            #endregion  Keys that have key
            #region Keys that have no key in keyboard
            Dictionary<string, KeyConfigOthers> ahkCloneKeyConfig = new Dictionary<string, KeyConfigOthers>(jsonObject.AhkEntriesOthers);

            foreach (KeyValuePair<string, KeyConfigOthers> config in ahkCloneKeyConfig)
            {
                ToggleCheckboxByName(config.Key, config.Value.ClickActive);
            }

            foreach (Control c in this.keyConfig.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    if (check.Enabled)
                        check.CheckStateChanged += onCheckChangeKeyConfig;
                }
            }

            foreach (Control c in this.groupAhkConfig.Controls)
            {
                if (c is RadioButton check)
                {
                    if (check.Enabled)
                        check.CheckedChanged += ahkCompatibility_CheckedChanged; // Remove event handler before modification
                }
            }


            #endregion Keys that have no key in keyboard

            //SpPercent Controls
            txtSpammerDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            txtSpammerDelay.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            txtSpammerDelay.TextChanged += async (sender, e) => await txtSpammerDelayChanged(sender, e);
        }
        private async Task txtSpammerDelayChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Ahk>(userToggleState.Ahk);
            Key key = (Key)Enum.Parse(typeof(Key), txtSpammerDelay.Text);

            if (jsonObject != null)
            {
                jsonObject.AhkDelay = Convert.ToInt32(key);
                jsonObject.GetActionName();
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Ahk = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync(userToggleState);
            }
            else
            {
                return;
            }
        }

        private async void ahkCompatibility_CheckedChanged(object sender, EventArgs e)
        {
            //Dsiable the radiobutton to avoid threading conflict
            foreach (Control c in this.groupAhkConfig.Controls)
            {
                if (c is RadioButton check)
                {
                    if (check.Enabled)
                        check.CheckedChanged -= ahkCompatibility_CheckedChanged; // Remove event handler before modification
                }
            }


            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Ahk>(userToggleState.Ahk);
            if (ahkCompatibility.Checked)
            {
                jsonObject.ahkMode = Ahk.COMPATABILITY;
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Ahk = updatedJson;

                this.chkMouseFlick.Enabled = true;
                this.chkNoShift.Enabled = true;
            }
            else
            {
                jsonObject.ahkMode = Ahk.SPEED_BOOST;
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Ahk = updatedJson;
                this.chkMouseFlick.Enabled = false;
                this.chkNoShift.Enabled = false;
            }
            // Persist changes add to the object array in database
            await _userSettingService.SaveChangesAsync(userToggleState);

            //Enable the checkbox again after saving
            foreach (Control c in this.groupAhkConfig.Controls)
            {
                if (c is RadioButton check)
                {
                    if (check.Enabled)
                        check.CheckedChanged += ahkCompatibility_CheckedChanged; // Remove event handler before modification
                }
            }


        }
        private void ToggleCheckboxByName(string Name, bool state)
        {
            CheckBox checkbox = (CheckBox)this.Controls.Find(Name, true)[0];
            checkbox.CheckState = state ? CheckState.Checked : CheckState.Indeterminate;
        }
        private void SetLegendDefaultValues()
        {
            //No mouse click cbWithNoClick
            this.cbWithNoClick.ThreeState = true;
            this.cbWithNoClick.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.cbWithNoClick.AutoCheck = false;
            //With mouse click cbWithClick
            this.cbWithClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWithClick.ThreeState = true;
            this.cbWithClick.AutoCheck = false;
        }
        private async void onCheckChange(object sender, EventArgs e)
        {
            _debounceTokenSource?.Cancel(); // cancel previous
            _debounceTokenSource = new CancellationTokenSource();

            CheckBox checkbox = (CheckBox)sender;
            Key key = (Key)new KeyConverter().ConvertFromString(checkbox.Text);
            bool haveMouseClick = checkbox.CheckState == CheckState.Checked ? true : false;

            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Ahk>(userToggleState.Ahk);
            //If there is a checked key in checkbox then register this to the profile that was selected.
            if (checkbox.CheckState == CheckState.Checked || checkbox.CheckState == CheckState.Indeterminate)
            {
                //add every entry that the user changed
                jsonObject.AddAHKEntry(checkbox.Name, new KeyConfig(key, haveMouseClick));
            }

            else
            {
                //remove the entry if the user click unchecked
                jsonObject.RemoveAHKEntry(checkbox.Name);
            }
            var updatedJson = JsonSerializer.Serialize(jsonObject);
            userToggleState.Ahk = updatedJson;
            var token = _debounceTokenSource.Token;
            await Task.Delay(500, token) // Wait for user to pause
                .ContinueWith(async t =>
                {
                    if (!t.IsCanceled)
                    {
                        // Persist changes add to the object array in database
                        await _userSettingService.SaveChangesAsync(userToggleState);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void onCheckChangeKeyConfig(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            bool haveMouseClick = checkbox.CheckState == CheckState.Checked ? true : false;

            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Ahk>(userToggleState.Ahk);
            //If there is a checked key in checkbox then register this to the profile that was selected.
            if (checkbox.CheckState == CheckState.Checked)
            {
                //add every entry that the user changed
                jsonObject.AddAHKEntryKeyConfig(checkbox.Name, new KeyConfigOthers(haveMouseClick));
            }

            else
            {
                //remove the entry if the user click unchecked
                jsonObject.RemoveAHKEntryKeyConfig(checkbox.Name);
            }
            var updatedJson = JsonSerializer.Serialize(jsonObject);
            userToggleState.Ahk = updatedJson;
            // Persist changes add to the object array in database
            await _userSettingService.SaveChangesAsync(userToggleState);
        }


        #endregion Ahk Region
    }
}
