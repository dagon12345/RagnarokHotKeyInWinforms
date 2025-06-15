using _4RTools.Model;
using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.Model.DataModels;
using Microsoft.Extensions.DependencyInjection;
using RagnarokHotKeyInWinforms.Forms;
using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
namespace RagnarokHotKeyInWinforms
{
    public partial class frm_Main : Form, IObserver
    {
        private Subject subject = new Subject();//subject triggers the Update() method inside notify function
        private System.Windows.Forms.Timer progressTimer;
        private int progressIncrement; // Increment value for each tick
        private int targetProgress; // Target progress value
        List<ClientDTO> clients = new List<ClientDTO>(); // list of clients with address initiated
        private List<BuffContainer> stuffContainers = new List<BuffContainer>();
        private Keys lastKey;
        private _4RThread mainThread;

        private readonly IStoredCredentialService _storedCredentialService;
        private readonly ISignIn _signIn;
        private readonly IUserSettingService _userSettingService;
        private readonly IBaseTableService _baseTableService;

        public frm_Main(IStoredCredentialService storedCredentialService, ISignIn signIn,
            IUserSettingService userSettingService, IBaseTableService baseTableService)
        {

            this.subject.Attach(this);
            InitializeComponent();
            KeyboardHook.Enable();
            #region Interfaces
            _storedCredentialService = storedCredentialService;
            _signIn = signIn;
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;
            #endregion

            this.Text = AppConfig.Name + " - " + AppConfig.Version; // Window title
            //SetAutobuffSkillWindow();//AutoBuff Skill
            //SetSongMacroWindow(); // Macro Song Form
            //SetAtkDefWindow();//AtkDef tab page
            //SetMacroSwitchWindow();
        }

        #region ToggleApplicationStateFunction (No Start Method)
        private bool toggleStatus()
        {
            bool isOn = this.btnStatusToggle.Text == "On";
            if (isOn)
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
                if (client != null)
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

        private void btnStatusToggle_Click(object sender, EventArgs e)
        {
            this.toggleStatus();
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
            var jsonObject = JsonSerializer.Deserialize<UserPreferences>(toggleStateValue.UserPreferences);
            this.txtStatusToggleKey.Text = jsonObject.toggleStateKey;
            txtStatusToggleKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
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
        #region AutopotSettings(Triggered with Start() method)
        private async Task RetrieveAutopot()
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Autopot>(userToggleState.Autopot);

            txtHpKey.Text = jsonObject.hpKey.ToString() ?? "0";
            txtSpKey.Text = jsonObject.spKey.ToString() ?? "0";
            txtHPpct.Text = jsonObject.hpPercent.ToString() ?? "0";
            txtSPpct.Text = jsonObject.spPercent.ToString() ?? "0";
            txtAutopotDelay.Text = jsonObject.delay.ToString() ?? "0";

            //HPkey Controls
            txtHpKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtHpKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHpKey.TextChanged += async (sender, e) => await onHpTextChange(sender, e);

            //HPPercent Controls
            txtHPpct.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtHPpct.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHPpct.TextChanged += async (sender, e) => await txtHPpctTextChanged(sender, e);

            //SPKey controls
            txtSpKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSpKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSpKey.TextChanged += async (sender, e) => await onSpTextChange(sender, e);

            //SpPercent Controls
            txtSPpct.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSPpct.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSPpct.TextChanged += async (sender, e) => await txtSPpctTextChanged(sender, e);


            //Delay
            txtAutopotDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtAutopotDelay.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtAutopotDelay.TextChanged += async (sender, e) => await txtAutopotDelayTextChanged(sender, e);
        }
        #region HpTexAndPercent Autopot
        private async Task onHpTextChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Autopot>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtHpKey.Text);

            if (jsonObject != null)
            {
                jsonObject.hpKey = key;
                jsonObject.GetActionName();
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Autopot = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        private async Task txtHPpctTextChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Autopot>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtHPpct.Text);

            if (jsonObject != null)
            {
                jsonObject.hpPercent = Convert.ToInt32(key);
                jsonObject.GetActionName();
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Autopot = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }

        }
        #endregion
        #region SpTextAndPercent Autopot
        private async Task onSpTextChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Autopot>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtSpKey.Text);

            if (jsonObject != null)
            {
                jsonObject.spKey = key;
                jsonObject.GetActionName();
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Autopot = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }

        private async Task txtSPpctTextChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Autopot>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtSPpct.Text);

            if (jsonObject != null)
            {
                jsonObject.spPercent = Convert.ToInt32(key);
                jsonObject.GetActionName();
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Autopot = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        private async Task txtAutopotDelayTextChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Autopot>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtAutopotDelay.Text);

            if (jsonObject != null)
            {
                jsonObject.delay = Convert.ToInt32(key);
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Autopot = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        #endregion
        #endregion AutopotSettings
        #region SkillTimer(Triggered with Start() method)
        private async Task SkillTimerRetrieve()
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoRefreshSpammer>(userToggleState.AutoRefreshSpammer);

            txtAutoRefreshDelay.Text = jsonObject.refreshDelay.ToString() ?? "0";
            txtSkillTimerKey.Text = jsonObject.refreshKey.ToString() ?? "0";

            //Default values from hotkeys
            this.txtSkillTimerKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtSkillTimerKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtSkillTimerKey.TextChanged += new EventHandler(this.onSkillTimerKeyChange);
            this.txtAutoRefreshDelay.ValueChanged += new EventHandler(this.txtAutoRefreshDelayTextChanged);
        }
        private async void onSkillTimerKeyChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoRefreshSpammer>(userToggleState.AutoRefreshSpammer);

            Key key = (Key)Enum.Parse(typeof(Key), txtSkillTimerKey.Text.ToString());

            if (jsonObject != null)
            {
                jsonObject.refreshKey = key;
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.AutoRefreshSpammer = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        private async void txtAutoRefreshDelayTextChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoRefreshSpammer>(userToggleState.AutoRefreshSpammer);

            Key key = (Key)Enum.Parse(typeof(Key), txtAutoRefreshDelay.Text.ToString());

            if (jsonObject != null)
            {
                jsonObject.refreshDelay = Convert.ToInt32(key);
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.AutoRefreshSpammer = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        #endregion SkillTimer
        #region Status Recovery Effect Form(Triggered with Start() method)
        private async Task RetrieveStatusEffect()
        {
            try
            {
                var userToggleState = await ReturnToggleKey();
                var jsonObject = JsonSerializer.Deserialize<StatusRecovery>(userToggleState.StatusRecovery);

                if (jsonObject.buffMapping.Count > 0)
                {
                    txtStatusKey.Text = jsonObject.buffMapping[EffectStatusIDs.SILENCE].ToString();
                }

                this.txtStatusKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                this.txtStatusKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                this.txtStatusKey.TextChanged += new EventHandler(onStatusKeyChange);
                this.txtNewStatusKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                this.txtNewStatusKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                this.txtNewStatusKey.TextChanged += new EventHandler(on3RDStatusKeyChange);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
            }

        }
        #region private method
        private async void onStatusKeyChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<StatusRecovery>(userToggleState.StatusRecovery);
            Key key = (Key)Enum.Parse(typeof(Key), txtStatusKey.Text.ToString());

            if (jsonObject != null)
            {
                jsonObject.AddKeyToBuff(EffectStatusIDs.POISON, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.SILENCE, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.BLIND, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.CONFUSION, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.HALLUCINATIONWALK, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.HALLUCINATION, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.CURSE, key);

                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.StatusRecovery = updatedJson;
                // Persist changes add to the object array in database
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;

            }
        }
        private async void on3RDStatusKeyChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<StatusRecovery>(userToggleState.StatusRecovery);
            Key key = (Key)Enum.Parse(typeof(Key), txtNewStatusKey.Text.ToString());

            if (jsonObject != null)
            {
                jsonObject.AddKeyToBuff(EffectStatusIDs.PROPERTYUNDEAD, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.BLOODING, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.MISTY_FROST, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.CRITICALWOUND, key);
                jsonObject.AddKeyToBuff(EffectStatusIDs.OVERHEAT, key);

                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.StatusRecovery = updatedJson;
                // Persist changes add to the object array in database
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;

            }
        }
        #endregion
        #endregion Status Effect From
        #region Stuff Auto Buff (Triggered with Start() method)
        public async Task RetrieveStuffAutobuffForm()
        {

            //Load the stuff containers
            stuffContainers.Add(new BuffContainer(this.PotionsGP, Buff.GetPotionsBuffs()));
            stuffContainers.Add(new BuffContainer(this.ElementalsGP, Buff.GetElementalsBuffs()));
            stuffContainers.Add(new BuffContainer(this.BoxesGP, Buff.GetBoxesBuffs()));
            stuffContainers.Add(new BuffContainer(this.FoodsGP, Buff.GetFoodBuffs()));
            stuffContainers.Add(new BuffContainer(this.ScrollBuffsGP, Buff.GetScrollBuffs()));
            stuffContainers.Add(new BuffContainer(this.EtcGP, Buff.GetETCBuffs()));

            //trigger the containers and textboxes doRender()
            new BuffRenderer(stuffContainers, toolTipAutoBuff).doRender();

            //Retrieve the setting
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoBuff>(userToggleState.Autobuff);

            Dictionary<EffectStatusIDs, Key> autoBuffClones = new Dictionary<EffectStatusIDs, Key>(jsonObject.buffMapping);
            // Assign key values to corresponding textboxes
            foreach (KeyValuePair<EffectStatusIDs, Key> config in autoBuffClones)
            {
                bool found = false;

                foreach (BuffContainer container in stuffContainers) // Iterate over all containers
                {
                    Control[] foundControls = container.container.Controls.Find(config.Key.ToString(), true);
                    if (foundControls.Length > 0 && foundControls[0] is TextBox textBox)
                    {
                        textBox.Text = config.Value.ToString(); // Set the assigned key
                        found = true;

                        break; // Stop searching once found
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"Textbox for '{config.Key}' not found in any group!");
                }

            }
            // Attach event handlers to textboxes across all GroupBoxes
            foreach (BuffContainer container in stuffContainers)
            {
                foreach (Control c in container.container.Controls)
                {
                    if (c is TextBox textBox)
                    {
                        textBox.TextChanged += onTextChange;
                    }
                }
            }

        }
        private async void onTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (!string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    if (!Enum.TryParse(txtBox.Name, out EffectStatusIDs statusID))
                    {
                        Console.WriteLine($"Invalid EffectStatusID from TextBox name: {txtBox.Name}");
                        return;
                    }

                    if (!Enum.TryParse(txtBox.Text, out Key key))
                    {
                        Console.WriteLine($"Invalid Key input: {txtBox.Text}");
                        return;
                    }

                    var userToggleState = await ReturnToggleKey();
                    var jsonObject = JsonSerializer.Deserialize<AutoBuff>(userToggleState.Autobuff);

                    jsonObject.AddKeyToBuff(statusID, key);

                    var updatedJson = JsonSerializer.Serialize(jsonObject);
                    userToggleState.Autobuff = updatedJson;


                    await _userSettingService.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in onTextChange: {ex.Message}");
            }
        }
        #endregion Stuff Auto Buff (No Start() Method)
        #region Ahk Region (Triggered with Start() method)
        private async Task AhkRetrieval()
        {
            //Default values of legend
            SetLegendDefaultValues();

            //remove handlers so that it wont trigger the onCheckedChange on loop
            foreach (Control c in this.tabPageSpammer.Controls)
            {
                if (c is CheckBox check)
                {
                    if (check.Enabled)
                        check.CheckStateChanged -= onCheckChange; // Remove event handler before modification
                }
            }

            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AHK>(userToggleState.Ahk);
            txtSpammerDelay.Text = jsonObject.AhkDelay.ToString() ?? "0";
            await DisableControlsIfSpeedBoost();
            #region Keys that have key
            Dictionary<string, KeyConfig> ahkClones = new Dictionary<string, KeyConfig>(jsonObject.AhkEntries);
            foreach (KeyValuePair<string, KeyConfig> config in ahkClones)
            {
                ToggleCheckboxByName(config.Key, config.Value.ClickActive);
            }

            //Check the tab spammer for checkboxes. This will be triggered by the method ToggleCheckboxByName on loop.
            foreach (Control c in this.tabPageSpammer.Controls)
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
            #endregion Keys that have no key in keyboard

            //SpPercent Controls
            txtSpammerDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSpammerDelay.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSpammerDelay.TextChanged += async (sender, e) => await txtSpammerDelayChanged(sender, e);
        }
        private async Task txtSpammerDelayChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AHK>(userToggleState.Ahk);
            Key key = (Key)Enum.Parse(typeof(Key), txtSpammerDelay.Text);

            if (jsonObject != null)
            {
                jsonObject.AhkDelay = Convert.ToInt32(key);
                jsonObject.GetActionName();
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Ahk = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        private async Task DisableControlsIfSpeedBoost()
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AHK>(userToggleState.Ahk);
            if (jsonObject.ahkMode == AHK.SPEED_BOOST)
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


        private async void ahkCompatibility_CheckedChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AHK>(userToggleState.Ahk);
            if (ahkCompatibility.Checked)
            {
                jsonObject.ahkMode = AHK.COMPATABILITY;
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Ahk = updatedJson;

                this.chkMouseFlick.Enabled = true;
                this.chkNoShift.Enabled = true;
            }
            else
            {
                jsonObject.ahkMode = AHK.SPEED_BOOST;
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.Ahk = updatedJson;
                this.chkMouseFlick.Enabled = false;
                this.chkNoShift.Enabled = false;
            }
            // Persist changes add to the object array in database
            await _userSettingService.SaveChangesAsync();
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
            CheckBox checkbox = (CheckBox)sender;
            Key key = (Key)new KeyConverter().ConvertFromString(checkbox.Text);
            bool haveMouseClick = checkbox.CheckState == CheckState.Checked ? true : false;

            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AHK>(userToggleState.Ahk);
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
            // Persist changes add to the object array in database
            await _userSettingService.SaveChangesAsync();
        }

        private async void onCheckChangeKeyConfig(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            bool haveMouseClick = checkbox.CheckState == CheckState.Checked ? true : false;

            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AHK>(userToggleState.Ahk);
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
            await _userSettingService.SaveChangesAsync();
        }

        #endregion Ahk Region

        public async Task<T> GetDeserializedObject<T>(Func<Task<string>> getJsonData)
        {
            var jsonData = await getJsonData();
            return JsonSerializer.Deserialize<T>(jsonData);
        }


        private async Task TriggerStartActions()
        {
            //Done
            var jsonObjectAhk = await GetDeserializedObject<AHK>(async () => (await ReturnToggleKey()).Ahk);
            //Done
            var jsonObjectAutopot = await GetDeserializedObject<Autopot>(async () => (await ReturnToggleKey()).Autopot);
            //Done
            var jsonObjectAutoRefresh = await GetDeserializedObject<AutoRefreshSpammer>(async () => (await ReturnToggleKey()).AutoRefreshSpammer);
            //Done
            var jsonObjectStatusRecovery = await GetDeserializedObject<StatusRecovery>(async () => (await ReturnToggleKey()).StatusRecovery);
            //Done
            var jsonObjectAutoBuff = await GetDeserializedObject<AutoBuff>(async () => (await ReturnToggleKey()).Autobuff);
            mainThread = new _4RThread(_ =>
            {
                jsonObjectAhk?.AHKThreadExecution(ClientSingleton.GetClient());
                jsonObjectAutopot?.AutopotThreadExecution(ClientSingleton.GetClient(), 0);
                jsonObjectAutoRefresh?.AutorefreshThreadExecution(ClientSingleton.GetClient());
                jsonObjectStatusRecovery?.RestoreStatusThread(ClientSingleton.GetClient());
                jsonObjectAutoBuff?.AutoBuffThread(ClientSingleton.GetClient());
                Task.Delay(50).Wait(); // Safe exit
            });
        }

        private void TriggerStopActions()
        {
            mainThread?.Stop();
        }
        public async void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.TURN_ON:
                    Client client = ClientSingleton.GetClient();
                    if (client != null)
                    {
                        characterName.Text = ClientSingleton.GetClient().ReadCharacterName();
                    }

                    await TriggerStartActions();
                    break;
                case MessageCode.TURN_OFF:
                    TriggerStopActions();
                    break;
                case MessageCode.SERVER_LIST_CHANGED:
                    this.refreshProcessList();
                    break;
                case MessageCode.CLICK_ICON_TRAY:
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    break;
                case MessageCode.SHUTDOWN_APPLICATION:
                    this.ShutdownApplication();
                    break;
            }
        }
        //addform used for each forms
        public void addForm(TabPage tp, Form f)
        {
            if (!tp.Controls.Contains(f))
            {
                tp.Controls.Add(f);
                f.Dock = DockStyle.Fill;
                f.Show();
                Refresh();
            }
            Refresh();
        }
        #region Frames
        public void SetMacroSwitchWindow()
        {
            MacroSwitchForm frm = new MacroSwitchForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            addForm(this.tabPageMacroSwitch, frm);
            frm.Show();
        }
        public void SetAtkDefWindow()
        {
            AtkDefForm frm = new AtkDefForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            addForm(this.tabPageAtkDef, frm);
            frm.Show();
        }
        public void SetSongMacroWindow()
        {
            MacroSongForm frm = new MacroSongForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            addForm(this.tabPageMacroSongs, frm);
            frm.Show();
        }
        #endregion

        #region Public methods

        #endregion
        #region Private Methods
        private async void Form1_Load(object sender, EventArgs e)
        {
            StartUpdate();//Start the update to get the game address
            this.refreshProcessList(); // refresh the combobox and get the game

            var credential = await _signIn.GoogleAlgorithm(GoogleConstants.GoogleApis);
            var storedCreds = await _storedCredentialService.FindCredential(credential.Token.AccessToken);
            var getBaseTable = await _baseTableService.SearchUser(storedCreds.UserEmail);
            await _userSettingService.UpsertUser(getBaseTable.ReferenceCode, storedCreds.Name);

            var name = await _storedCredentialService.FindCredential(credential.Token.AccessToken);
            lblUserName.Text = $"Welcome back, {name.Name}";

            await RetrieveStuffAutobuffForm();
            await Retrieve();
            await RetrieveAutopot();
            await SkillTimerRetrieve();
            await RetrieveStatusEffect();
            await AhkRetrieval();

            this.refreshProcessList();
        }
        private void StartUpdate()
        {

            pbSupportedServer.Value = 0; // Initialize progress bar
            pbSupportedServer.Maximum = 100; // Set maximum value for progress bar

            //Check the json file named supported_server.json
            try
            {
                //Load the local servers address.
                clients.AddRange(LocalServerManager.GetLocalClients());
                //If tech is successful load the resource file locally now API needed
                //path where the supported_server.json was saved \bin\Debug\Resources
                string localFilePath = Path.Combine(AppConfig.LocalResourcePath, RagnarokConstants.SupportedServerJson);
                string localServerRaw = File.ReadAllText(localFilePath);
                clients.AddRange(JsonSerializer.Deserialize<List<ClientDTO>>(localServerRaw));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            // Prepare for the progress bar
            if (clients.Count > 0)
            {
                btnLogout.Enabled = false;
                progressIncrement = 100 / clients.Count; // Calculate increment per client
                targetProgress = 0; // Start from 0
                progressTimer = new System.Windows.Forms.Timer();
                progressTimer.Interval = 1000; // Set timer interval (100 ms)
                progressTimer.Tick += ProgressTimer_Tick; // Attach tick event handler
                progressTimer.Start(); // Start the timer
            }
        }
        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (targetProgress < 100)
            {
                targetProgress += progressIncrement;
                if (targetProgress > 100) targetProgress = 100; // Ensure it doesn't exceed 100
                pbSupportedServer.Value = targetProgress; // Update progress bar value
            }
            else
            {
                progressTimer.Stop(); // Stop the timer when complete
                LoadServers(clients); // Call the method to load servers
                this.refreshProcessList();// refresh the process list in combobox to avoid empty
                //Hide the progress bar and text
                pbSupportedServer.Hide();
                lblSupportedServer.Hide();
                btnLogout.Enabled = true;
            }
        }
        private void LoadServers(List<ClientDTO> clients)
        {
            foreach (ClientDTO clientDto in clients)
            {
                try
                {
                    // Scan processes to get our game address.
                    ClientListSingleton.AddClient(new Client(clientDto));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading client: {ex.Message}");
                }
            }
        }

        private void refreshProcessList()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.processCombobox.Items.Clear();
            });
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle != "" && ClientListSingleton.ExistsByProcessName(p.ProcessName))
                {
                    this.processCombobox.Items.Add(string.Format("{0}.exe - {1}", p.ProcessName, p.Id));
                }
            }
        }
        private void ShutdownApplication()
        {
            KeyboardHook.Disable();
            subject.Notify(new Utilities.Message(MessageCode.TURN_OFF, null));
            Environment.Exit(0);
        }

        private void processCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //When clicked get the name of the charcter to preview in the label
            Client client = new Client(this.processCombobox.SelectedItem.ToString());
            ClientSingleton.Instance(client);
            characterName.Text = client.ReadCharacterName();
            //If message code was changed then get the character name
            //subject.Notify(new Utilities.Message(Utilities.MessageCode.PROCESS_CHANGED, null));
        }
        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0); // Immediately ends all execution
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            //Set the LastLoginTime to munis 10 mins so we can trigger the function deleting of google current sign in
            var credential = await _signIn.GoogleAlgorithm(GoogleConstants.GoogleApis);
            var name = await _storedCredentialService.FindCredential(credential.Token.AccessToken);
            var searchUser = await _storedCredentialService.SearchUser(name.UserEmail);
            searchUser.LastLoginTime = searchUser.LastLoginTime.AddMinutes(-10);
            await _storedCredentialService.SaveChangesAsync();

            this.Hide();//Hide the form then show the signin form
            var getUserInfoInterface = Program.ServiceProvider.GetRequiredService<IGetUserInfo>();
            var userSignIn = Program.ServiceProvider.GetRequiredService<ISignIn>();
            var storedCredential = Program.ServiceProvider.GetRequiredService<IStoredCredentialService>();
            SignInForm sf = new SignInForm(getUserInfoInterface, userSignIn, storedCredential);
            sf.ShowDialog();
        }

        private void ProcessTimer_Tick(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.refreshProcessList();
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
        }

        #endregion

    }
}
