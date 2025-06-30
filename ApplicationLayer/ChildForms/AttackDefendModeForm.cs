using ApplicationLayer.Designer;
using ApplicationLayer.Interface;
using ApplicationLayer.Models.RagnarokModels;
using Domain.Constants;
using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ApplicationLayer.ChildForms
{
    public partial class AttackDefendModeForm : Form
    {
        private IBaseTableService _baseTableService;
        private IUserSettingService _userSettingService;
        public string email;
        public AttackDefendModeForm(IUserSettingService userSettingService, IBaseTableService baseTableService)
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;
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
                await DisplayAttackDefendMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }
        #region Attack Defend Form (Triggered with start() method)
        private async Task DisplayAttackDefendMode()
        {

            var toggleStateValue = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AttackDefendMode>(toggleStateValue.AtkDefMode);

            this.inSpammerKey.Text = jsonObject.keySpammer.ToString();
            this.spammerDelay.Value = jsonObject.ahkDelay;
            this.switchDelay.Value = jsonObject.switchDelay;
            this.inSpammerClick.Checked = jsonObject.keySpammerWithClick;
            Dictionary<string, Key> atkKeys = new Dictionary<string, Key>(jsonObject.atkKeys);
            Dictionary<string, Key> defKeys = new Dictionary<string, Key>(jsonObject.defKeys);

            foreach (Control control in this.panelSwitch.Controls)
            {
                if (control is TextBox)
                {

                    TextBox tb = (TextBox)control;
                    if (!tb.Tag.ToString().Equals("spammerKey"))
                    {
                        AttackDefendEnum mode = (AttackDefendEnum)Int16.Parse(tb.Tag.ToString());
                        if (mode == AttackDefendEnum.DEF)
                        {
                            tb.Text = defKeys.ContainsKey(tb.Name) ? defKeys[tb.Name].ToString() : Keys.None.ToString();
                        }
                        else
                        {
                            tb.Text = atkKeys.ContainsKey(tb.Name) ? atkKeys[tb.Name].ToString() : Keys.None.ToString();
                        }
                    }

                    TextBox textBox = (TextBox)control;
                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
                    textBox.TextChanged += new EventHandler(this.AttackDefendonTextChange);

                }
            }

            foreach (Control c in this.groupBoxATKxDEFConfig.Controls)
            {
                if (c is CheckBox check)
                {
                    if (check.Enabled)
                        check.CheckStateChanged += ChkBox_CheckedChanged;
                }
            }

            //Delay
            spammerDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            spammerDelay.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            spammerDelay.TextChanged += async (sender, e) => await txtSpammerDelayTextChanged(sender, e);

            //Switch Delay
            switchDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            switchDelay.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            switchDelay.TextChanged += async (sender, e) => await txtSwitchDelayTextChanged(sender, e);
            //
            inSpammerKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            inSpammerKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            inSpammerKey.TextChanged += async (sender, e) => await txtInSpammerDelayTextChanged(sender, e);

        }
        private async Task txtInSpammerDelayTextChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AttackDefendMode>(userToggleState.AtkDefMode);
            Key key = (Key)Enum.Parse(typeof(Key), inSpammerKey.Text);

            if (jsonObject != null)
            {
                jsonObject.keySpammer = key;
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.AtkDefMode = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync(userToggleState);
            }
            else
            {
                return;
            }
        }
        private async Task txtSpammerDelayTextChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AttackDefendMode>(userToggleState.AtkDefMode);
            Key key = (Key)Enum.Parse(typeof(Key), spammerDelay.Text);

            if (jsonObject != null)
            {
                jsonObject.ahkDelay = Convert.ToInt32(key);
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.AtkDefMode = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync(userToggleState);
            }
            else
            {
                return;
            }
        }
        private async Task txtSwitchDelayTextChanged(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AttackDefendMode>(userToggleState.AtkDefMode);
            Key key = (Key)Enum.Parse(typeof(Key), switchDelay.Text);

            if (jsonObject != null)
            {
                jsonObject.switchDelay = Convert.ToInt32(key);
                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.AtkDefMode = updatedJson;
                // Persist changes
                await _userSettingService.SaveChangesAsync(userToggleState);
            }
            else
            {
                return;
            }
        }
        private async void ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            var toggleStateValue = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AttackDefendMode>(toggleStateValue.AtkDefMode);

            jsonObject.keySpammerWithClick = this.inSpammerClick.Checked;
            var updatedJson = JsonSerializer.Serialize(jsonObject);
            toggleStateValue.AtkDefMode = updatedJson;
            // Persist changes
            await _userSettingService.SaveChangesAsync(toggleStateValue);
        }
        private async void AttackDefendonTextChange(object sender, EventArgs e)
        {
            var toggleStateValue = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AttackDefendMode>(toggleStateValue.AtkDefMode);

            TextBox textBox = (TextBox)sender;
            Key key = (Key)Enum.Parse(typeof(Key), textBox.Text.ToString());

            //If it's ATK OR DEF
            if (textBox.Tag.Equals("spammerKey"))
            {
                jsonObject.keySpammer = key;
            }
            else
            {
                AttackDefendEnum mode = (AttackDefendEnum)Int16.Parse(textBox.Tag.ToString());
                jsonObject.AddSwitchItem(textBox.Name, key, mode);
            }
            var updatedJson = JsonSerializer.Serialize(jsonObject);
            toggleStateValue.AtkDefMode = updatedJson;
            // Persist changes
            await _userSettingService.SaveChangesAsync(toggleStateValue);

        }
        #endregion Attack Defend Form

    }
}
