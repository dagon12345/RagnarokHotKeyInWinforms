using ApplicationLayer.Designer;
using ApplicationLayer.Interface;
using ApplicationLayer.Models.RagnarokModels;
using Domain.Constants;
using Domain.Model.DataModels;
using Infrastructure.Utilities;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationLayer.ChildForms
{
    public partial class StatusRecoveryForm : Form
    {
        private readonly IBaseTableService _baseTableService;
        private readonly IUserSettingService _userSettingService;

        public string email;//get the users email from login
        public StatusRecoveryForm(IBaseTableService baseTableService, IUserSettingService userSettingService)
        {
            InitializeComponent();
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
                await RetrieveStatusEffect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        #region Status Recovery Effect Form(Triggered with Start() method)
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }
        private async Task RetrieveStatusEffect()
        {
            try
            {
                var userToggleState = await ReturnToggleKey();
                var jsonObject = JsonSerializer.Deserialize<StatusRecovery>(userToggleState.StatusRecovery);
                if (jsonObject.buffMapping.Count > 0)
                {
                    txtStatusKey.Text = jsonObject.buffMapping[EffectStatusIdEnum.SILENCE].ToString();

                    if (jsonObject.buffMapping.TryGetValue(EffectStatusIdEnum.PROPERTYUNDEAD, out var value))
                    {
                        string rawText = value.ToString();
                        Keys parsedKey;

                        if (!Enum.TryParse(rawText, out parsedKey) || !Enum.IsDefined(typeof(Keys), parsedKey))
                        {
                            parsedKey = Keys.None;
                        }

                        txtNewStatusKey.Text = parsedKey.ToString();
                        // Use `text` as needed
                    }
                    else
                    {
                        // Fallback if the key isn’t present
                        txtNewStatusKey.Text = Keys.None.ToString();
                        // Or log this scenario for debugging
                    }


                }

                this.txtStatusKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
                this.txtStatusKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
                this.txtStatusKey.TextChanged += new EventHandler(onStatusKeyChange);

                this.txtNewStatusKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
                this.txtNewStatusKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
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
        private async void onStatusKeyChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<StatusRecovery>(userToggleState.StatusRecovery);
            Keys key = (Keys)Enum.Parse(typeof(Keys), txtStatusKey.Text.ToString());

            if (jsonObject != null)
            {
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.POISON, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.SILENCE, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.BLIND, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.CONFUSION, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.HALLUCINATIONWALK, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.HALLUCINATION, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.CURSE, key);

                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.StatusRecovery = updatedJson;
                // Persist changes add to the object array in database
                await _userSettingService.SaveChangesAsync(userToggleState);
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
            Keys key = (Keys)Enum.Parse(typeof(Keys), txtNewStatusKey.Text.ToString());

            if (jsonObject != null)
            {
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.PROPERTYUNDEAD, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.BLOODING, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.MISTY_FROST, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.CRITICALWOUND, key);
                jsonObject.AddKeyToBuff(EffectStatusIdEnum.OVERHEAT, key);

                var updatedJson = JsonSerializer.Serialize(jsonObject);
                userToggleState.StatusRecovery = updatedJson;
                // Persist changes add to the object array in database
                await _userSettingService.SaveChangesAsync(userToggleState);
            }
            else
            {
                return;

            }
        }
        #endregion Status Effect From

    }

}
