using _4RTools.Model;
using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.Model;
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
using System.Text.RegularExpressions;
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
        private List<BuffContainer> stuffBuffContainers = new List<BuffContainer>();
        private List<BuffContainer> skillBuffContainers = new List<BuffContainer>();
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
        #region Stuff and Skill Auto Buff (Triggered with Start() method)
        private async Task RetrieveStuffAutobuffForm()
        {

            //Load the stuff containers
            stuffBuffContainers.Add(new BuffContainer(this.PotionsGP, Buff.GetPotionsBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.ElementalsGP, Buff.GetElementalsBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.BoxesGP, Buff.GetBoxesBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.FoodsGP, Buff.GetFoodBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.ScrollBuffsGP, Buff.GetScrollBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.EtcGP, Buff.GetETCBuffs()));


            //trigger the containers and textboxes doRender()
            new BuffRenderer(stuffBuffContainers, toolTipAutoBuff).doRender();

            // Retrieve the setting
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoBuff>(userToggleState.Autobuff);

            // Return the dictionary
            var autoBuffClones = new Dictionary<EffectStatusIDs, Key>(jsonObject.buffMapping);
            // Assign key values to corresponding textboxes
            foreach (KeyValuePair<EffectStatusIDs, Key> config in autoBuffClones)
            {
                bool found = false;

                foreach (BuffContainer container in stuffBuffContainers) // Iterate over all containers
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
            foreach (BuffContainer container in stuffBuffContainers)
            {
                foreach (Control c in container.container.Controls)
                {
                    if (c is TextBox textBox)
                    {
                        textBox.TextChanged += onTextChange;
                    }
                }
            }

            //Skill Auto Buff region

            skillBuffContainers.Add(new BuffContainer(this.ArcherSkillsGP, Buff.GetArcherSkills()));
            skillBuffContainers.Add(new BuffContainer(this.SwordmanSkillGP, Buff.GetSwordmanSkill()));
            skillBuffContainers.Add(new BuffContainer(this.MageSkillGP, Buff.GetMageSkills()));
            skillBuffContainers.Add(new BuffContainer(this.MerchantSkillsGP, Buff.GetMerchantSkills()));
            skillBuffContainers.Add(new BuffContainer(this.ThiefSkillsGP, Buff.GetThiefSkills()));
            skillBuffContainers.Add(new BuffContainer(this.AcolyteSkillsGP, Buff.GetAcolyteSkills()));
            skillBuffContainers.Add(new BuffContainer(this.TKSkillGroupBox, Buff.GetTaekwonSkills()));
            skillBuffContainers.Add(new BuffContainer(this.NinjaSkillsGP, Buff.GetNinjaSkills()));
            skillBuffContainers.Add(new BuffContainer(this.GunsSkillsGP, Buff.GetGunsSkills()));

            //trigger the containers and textboxes doRender()
            new BuffRenderer(skillBuffContainers, toolTipAutoBuff).doRender();
            //The retrieving of this from database is from our Stuff Autobuff
            foreach (KeyValuePair<EffectStatusIDs, Key> config in autoBuffClones)
            {
                bool found = false;

                foreach (BuffContainer container in skillBuffContainers) // Iterate over all containers
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
            foreach (BuffContainer container in skillBuffContainers)
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
        //Updating when text changed.
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
        #endregion Stuff and Skill Auto Buff
        #region Ahk Region (Triggered with Start() method)
        private void DisableControlsIfSpeedBoost(AHK jsonObject)
        {
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
            foreach (Control c in this.groupAhkConfig.Controls)
            {
                if (c is RadioButton check)
                {
                    if (check.Enabled)
                        check.CheckedChanged -= ahkCompatibility_CheckedChanged; // Remove event handler before modification
                }
            }

            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AHK>(userToggleState.Ahk);
            txtSpammerDelay.Text = jsonObject.AhkDelay.ToString() ?? "0";
            DisableControlsIfSpeedBoost(jsonObject);
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
        #region MacroSongForm (Triggered with Start() method)
        private async Task<Macro> ReturnMacro()
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Macro>(userToggleState.SongMacro);
            return jsonObject;
        }
        private async void DelayMac(object sender, EventArgs e)
        {
            var jsonObject = await ReturnMacro();
            var userToggleState = await ReturnToggleKey();
            if (jsonObject != null)
            {
                NumericUpDown numeric = (NumericUpDown)sender;
                int delayValue = (int)numeric.Value; // Get updated delay from NumericUpDown

                // Extract Macro ID from NumericUpDown name
                string[] parts = numeric.Name.Split(new[] { "Mac" }, StringSplitOptions.None);
                if (parts.Length < 2)
                {
                    Console.WriteLine($"Invalid control name format: {numeric.Name}");
                    return;
                }
                int macroID = int.Parse(parts[1]);


                // Find matching macro config
                ChainConfig chainConfig = jsonObject.chainConfigs.Find(config => config.Id == macroID);

                if (chainConfig == null)
                {
                    Console.WriteLine($"Macro ID {macroID} not found in JSON!");
                    return;
                }

                // **Update only the delay field in chainConfig**
                chainConfig.delay = delayValue;
                Console.WriteLine($"Updated delay for Macro ID {macroID} to {delayValue}");

                // Update JSON and persist changes
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.SongMacro = updatedJson;
                await _userSettingService.SaveChangesAsync();
            }
        }

        private async void onTextChangeMacroSong(object sender, EventArgs e)
        {
            // Retrieve macro from database with the column SongMacro
            var jsonObject = await ReturnMacro();
            var userToggleState = await ReturnToggleKey();

            TextBox textBox = (TextBox)sender;
            Key key = (Key)Enum.Parse(typeof(Key), textBox.Text.ToString());

            if (textBox.Tag != null)
            {
                // Could be Trigger, Dagger or Instrument input
                string[] inputTag = textBox.Tag.ToString().Split(new[] { ":" }, StringSplitOptions.None);
                int macroid = short.Parse(inputTag[0]);
                string type = inputTag[1];
                ChainConfig chainConfig = jsonObject.chainConfigs.Find(config => config.Id == macroid);

                switch (type)
                {
                    case "Dagger":
                        chainConfig.daggerKey = key;
                        break;
                    case "Instrument":
                        chainConfig.instrumentKey = key;
                        break;
                    case "Trigger":
                        chainConfig.trigger = key;
                        break;
                }
            }
            else
            {
                int macroID = int.Parse(textBox.Name.Split(new[] { "mac" }, StringSplitOptions.None)[1]);
                ChainConfig chainConfig = jsonObject.chainConfigs.Find(songMacro => songMacro.Id == macroID);
                chainConfig.macroEntries[textBox.Name] = new MacroKey(key, chainConfig.delay);
            }


            var updatedJson = JsonSerializer.Serialize(jsonObject);
            userToggleState.SongMacro = updatedJson;
            //Persist changes add to the object array in database
            await _userSettingService.SaveChangesAsync();
        }


        private async void onReset(object sender, EventArgs e)
        {
            Button delayInput = (Button)sender;
            int btnResetID = Int16.Parse(delayInput.Name.Split(new[] { "btnResMac" }, StringSplitOptions.None)[1]);

            // Retrieve current settings from the database
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Macro>(userToggleState.SongMacro);

            Panel p = (Panel)this.Controls.Find("panelMacro" + btnResetID, true)[0];

            // **Update data model first (before UI updates)**
            foreach (Control c in p.Controls)
            {
                if (c is TextBox textBox)
                {
                    string macroEntryKey = textBox.Name; // Extract macro entry key
                    ChainConfig chainConfig = jsonObject.chainConfigs.Find(config => config.Id == btnResetID);

                    if (chainConfig != null && chainConfig.macroEntries.ContainsKey(macroEntryKey))
                    {
                        chainConfig.macroEntries[macroEntryKey].key = Key.None;
                    }
                }
            }

            // **Save changes to database BEFORE updating UI**
            userToggleState.SongMacro = JsonSerializer.Serialize(jsonObject);
            await _userSettingService.SaveChangesAsync();

            // **Update UI AFTER saving**
            foreach (Control c in p.Controls)
            {
                if (c is TextBox textBox)
                {
                    // ✅ Skip if the TextBox name is "inTriggerMacro1"
                    if (textBox.Name == "inTriggerMacro1" ||
                        textBox.Name == "inTriggerMacro2" ||
                        textBox.Name == "inTriggerMacro3" ||
                        textBox.Name == "inTriggerMacro4")
                        continue;
                    // ✅ Skip if the TextBox name is "inDaggerMacro1"
                    if (textBox.Name == "inDaggerMacro1" ||
                        textBox.Name == "inDaggerMacro2" ||
                        textBox.Name == "inDaggerMacro3" ||
                        textBox.Name == "inDaggerMacro4")
                        continue;

                    // ✅ Skip if the TextBox name is "inInstrumentMacro1"
                    if (textBox.Name == "inInstrumentMacro1" ||
                        textBox.Name == "inInstrumentMacro2" ||
                        textBox.Name == "inInstrumentMacro3" ||
                        textBox.Name == "inInstrumentMacro4")
                        continue;


                    textBox.TextChanged -= onTextChangeMacroSong; // Prevent event from firing
                    textBox.Text = Key.None.ToString();
                    textBox.TextChanged += onTextChangeMacroSong; // Reattach event after update
                }
            }

        }
        private async Task updateUi()
        {
            // Retrieve macro from database with the column SongMacro
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<Macro>(userToggleState.SongMacro);
            UpdatePanelData(jsonObject);
            ConfigureMacroLanes();
        }


        private void UpdatePanelData(Macro jsonObject)
        {

            //loop through each Id not more than 4
            for (int id = 1; id <= 4; id++)
            {
                Panel p = (Panel)this.Controls.Find("panelMacro" + id, true)[0];
                ChainConfig chainConfig = new ChainConfig(jsonObject.chainConfigs[id - 1]);

                //Update Trigger Macro Value
                Control[] c = p.Controls.Find("inTriggerMacro" + chainConfig.Id, true);
                if (c.Length > 0)
                {
                    TextBox textBox = (TextBox)c[0];
                    textBox.Text = chainConfig.trigger.ToString();
                }

                //Update Dagger Value
                Control[] cDagger = p.Controls.Find("inDaggerMacro" + chainConfig.Id, true);
                if (c.Length > 0)
                {
                    TextBox textBox = (TextBox)cDagger[0];
                    textBox.Text = chainConfig.daggerKey.ToString();
                }

                //Update Instrument Value
                Control[] cInstrument = p.Controls.Find("inInstrumentMacro" + chainConfig.Id, true);
                if (c.Length > 0)
                {
                    TextBox textBox = (TextBox)cInstrument[0];
                    textBox.Text = chainConfig.instrumentKey.ToString();
                }


                List<string> names = new List<string>(chainConfig.macroEntries.Keys);
                foreach (string cbName in names)
                {
                    Control[] controls = p.Controls.Find(cbName, true);
                    if (controls.Length > 0)
                    {
                        TextBox textBox = (TextBox)controls[0];
                        textBox.Text = chainConfig.macroEntries[cbName].key.ToString();
                    }
                }

                //Update Delay Macro Value
                Control[] d = p.Controls.Find("delayMac" + chainConfig.Id, true);
                if (d.Length > 0)
                {
                    NumericUpDown delayInput = (NumericUpDown)d[0];
                    delayInput.Value = chainConfig.delay;
                }


            }


        }

        private void ConfigureMacroLanes()
        {
            //public static int TOTAL_MACRO_LANES_FOR_SONGS = 4; Set this to 4 for limit.
            for (int i = 1; i <= 4; i++)
            {
                initializeLane(i);
            }

        }

        private void initializeLane(int id)
        {
            try
            {
                Panel p = (Panel)this.Controls.Find("panelMacro" + id, true)[0];
                foreach (Control c in p.Controls)
                {
                    if (c is TextBox)
                    {
                        TextBox textBox = (TextBox)c;
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                        textBox.TextChanged += new EventHandler(this.onTextChangeMacroSong);
                    }

                    if (c is NumericUpDown)
                    {
                        NumericUpDown numeric = (NumericUpDown)c;
                        numeric.ValueChanged += new EventHandler(this.DelayMac);
                    }

                    if (c is Button)
                    {
                        Button resetButton = (Button)c;
                        resetButton.Click += new EventHandler(this.onReset);
                    }

                }
            }
            catch { }
        }


        #endregion MacroSongForm (Triggered with Start() method)
        #region MacroSwitch (Triggered with Start() method)
        private async Task DisplayMacroSwitch()
        {
            // Retrieve macro from database with the column MacroSwitch
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<MacroSwitch>(userToggleState.MacroSwitch);
            UpdatePanelDataMacroSwitch(jsonObject);
            ConfigureMacroChain();
        }
        private void UpdatePanelDataMacroSwitch(MacroSwitch jsonObject)
        {
            //Three limit of macro switch groupboxes. Loop through Id maximum 3
            for (int id = 1; id <= 3; id++)
            {
                try
                {
                    GroupBox group = (GroupBox)this.Controls.Find("chainGroup" + id, true)[0];
                    ChainConfigSwitch chainConfig = new ChainConfigSwitch(jsonObject.chainConfigs[id - 1]);

                    List<string> names = new List<string>(chainConfig.macroEntries.Keys);
                    foreach (string cbName in names)
                    {
                        Control[] controls = group.Controls.Find(cbName, true); // Keys
                        if (controls.Length > 0)
                        {
                            TextBox textBox = (TextBox)controls[0];
                            textBox.Text = chainConfig.macroEntries[cbName].key.ToString();
                        }

                        Control[] d = group.Controls.Find($"{cbName}delay", true); // Delays
                        if (d.Length > 0)
                        {
                            NumericUpDown delayInput = (NumericUpDown)d[0];
                            delayInput.Value = chainConfig.macroEntries[cbName].delay;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                };
            }
        }

        private void ConfigureMacroChain()
        {
            //public static int TOTAL_MACRO_LANES = 3; Set this to 3 for limit.
            for (int i = 1; i <= 3; i++)
            {
                initializeChain(i);
            }

        }

        private async void onTextChangeMacroSwitch(object sender, EventArgs e)
        {
            // Retrieve macro from database with the column MacroSwitch
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<MacroSwitch>(userToggleState.MacroSwitch);

            TextBox textBox = (TextBox)sender;
            Key key = (Key)Enum.Parse(typeof(Key), textBox.Text.ToString());

            int chainID = Int16.Parse(textBox.Parent.Name.Split(new[] { "chainGroup" }, StringSplitOptions.None)[1]);
            GroupBox group = (GroupBox)this.Controls.Find("chainGroup" + chainID, true)[0];
           //Done: ChainConfigSwitch
            ChainConfigSwitch chainConfig = jsonObject.chainConfigs.Find(config => config.Id == chainID); //search inside our column


            var matches = group.Controls.Find($"{textBox.Name}delay", true);
            if (matches.Length > 0 && matches[0] is NumericUpDown delayInput)
            {
                //Done: MacroKeySwitch
                chainConfig.macroEntries[textBox.Name] = new MacroKeySwitch(key, decimal.ToInt16(delayInput.Value));
            }
            else
            {
                Console.WriteLine($"Delay input not found: {textBox.Name}delay");


            }
            //The trigger key depends on the first key on textbox
            bool isFirstInput = Regex.IsMatch(textBox.Name, $"in9mac{chainID}");
            if (isFirstInput) { chainConfig.trigger = key; }

            var updatedJson = JsonSerializer.Serialize(jsonObject);
            userToggleState.MacroSwitch = updatedJson;
            //Persist changes add to the object array in database
            await _userSettingService.SaveChangesAsync();

        }
        private async void DelayMacroSwitch(object sender, EventArgs e)
        {
            // Retrieve macro from database with the column MacroSwitch
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<MacroSwitch>(userToggleState.MacroSwitch);

            NumericUpDown delayInput = (NumericUpDown)sender;
            int chainID = Int16.Parse(delayInput.Parent.Name.Split(new[] { "chainGroup" }, StringSplitOptions.None)[1]);
            ChainConfigSwitch chainConfig = jsonObject.chainConfigs.Find(config => config.Id == chainID);

            String cbName = delayInput.Name.Split(new[] { "delay" }, StringSplitOptions.None)[0];
           // chainConfig.macroEntries[cbName].delay = decimal.ToInt16(delayInput.Value);

            if (chainConfig.macroEntries.ContainsKey(cbName))
            {
                chainConfig.macroEntries[cbName].delay = decimal.ToInt16(delayInput.Value);
            }
            else
            {
                Console.WriteLine($"Key '{cbName}' not found in macroEntries.");
            }


            // Update JSON and persist changes
            var updatedJson = JsonSerializer.Serialize(jsonObject);
            userToggleState.MacroSwitch = updatedJson;
            await _userSettingService.SaveChangesAsync();

        }

        private void initializeChain(int id)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("chainGroup" + id, true)[0];
                foreach (Control c in p.Controls)
                {
                    if (c is TextBox)
                    {
                        TextBox textBox = (TextBox)c;
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                        textBox.TextChanged += new EventHandler(this.onTextChangeMacroSwitch);
                    }

                    if (c is NumericUpDown)
                    {
                        NumericUpDown numeric = (NumericUpDown)c;
                        numeric.ValueChanged += new EventHandler(this.DelayMacroSwitch);
                    }
                }
            }
            catch { }
        }
        #endregion
        #region Frames
        public void SetAtkDefWindow()
        {
            AtkDefForm frm = new AtkDefForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            addForm(this.tabPageAtkDef, frm);
            frm.Show();
        }
        #endregion
        #region Public methods
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
        #endregion
        #region Private Methods
        private async Task<T> GetDeserializedObject<T>(Func<Task<string>> getJsonData)
        {
            var jsonData = await getJsonData();
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        private async Task TriggerStartActions()
        {
            Client client = ClientSingleton.GetClient();
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
            //Done
            var jsonObjectMacroSong = await GetDeserializedObject<Macro>(async () => (await ReturnToggleKey()).SongMacro);
            //Done
            var jsonObjectMacroSwitch = await GetDeserializedObject<MacroSwitch>(async () => (await ReturnToggleKey()).MacroSwitch);
            mainThread = new _4RThread(_ =>
            {
                jsonObjectAhk?.AHKThreadExecution(client);
                jsonObjectAutopot?.AutopotThreadExecution(client, 0);
                jsonObjectAutoRefresh?.AutorefreshThreadExecution(client);
                jsonObjectStatusRecovery?.RestoreStatusThread(client);
                jsonObjectAutoBuff?.AutoBuffThread(client);
                jsonObjectMacroSong?.MacroExecutionThread(client);
                jsonObjectMacroSwitch?.MacroExecutionThreadSwitch(client);
                Task.Delay(50).Wait(); // Safe exit
            });
        }
        private void TriggerStopActions()
        {
            mainThread?.Stop();
        }

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


            await Retrieve();
            await RetrieveAutopot();
            await SkillTimerRetrieve();
            await RetrieveStatusEffect();
            await AhkRetrieval();// causing threading

            await RetrieveStuffAutobuffForm();

            await updateUi();
            await DisplayMacroSwitch();

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
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.refreshProcessList();
        }
        #endregion

        private void tabPageMacroSongs_Click(object sender, EventArgs e)
        {

        }
    }
}
