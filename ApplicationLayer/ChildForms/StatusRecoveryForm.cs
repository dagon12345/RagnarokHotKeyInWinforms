using ApplicationLayer.Designer;
using ApplicationLayer.Interface;
using ApplicationLayer.Interface.RagnarokInterface;
using ApplicationLayer.Models.RagnarokModels;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Singleton.RagnarokSingleton;
using ApplicationLayer.Utilities;
using Domain.Constants;
using Domain.Model.DataModels;
using Infrastructure.Utilities;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ApplicationLayer.ChildForms
{
    public partial class StatusRecoveryForm : Form, IObserverService
    {
        private readonly IBaseTableService _baseTableService;
        private readonly IUserSettingService _userSettingService;
        private readonly SubjectService _subjectService;
        private ThreadUtility ThreadUtility;
        public string email;//get the users email from login
        public StatusRecoveryForm(IBaseTableService baseTableService, IUserSettingService userSettingService, SubjectService subjectService)
        {
            
            InitializeComponent();
             DesignerService.ApplyDarkBlueTheme(this);
            _subjectService = subjectService;
            _baseTableService = baseTableService;
            _userSettingService = userSettingService;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _subjectService.Attach(this);
            _ = LoadAsync(); // Fire and forget safely

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
            var jsonObjectStatusRecovery = await GetDeserializedObject<StatusRecovery>(async () => (await ReturnToggleKey()).StatusRecovery);
            ThreadUtility = new ThreadUtility(_ =>
            {
                jsonObjectStatusRecovery?.RestoreStatusThread(client);
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
                        Key parsedKey;

                        if (!Enum.TryParse(rawText, out parsedKey) || !Enum.IsDefined(typeof(Key), parsedKey))
                        {
                            parsedKey = Key.None;
                        }

                        txtNewStatusKey.Text = parsedKey.ToString();
                        // Use `text` as needed
                    }
                    else
                    {
                        // Fallback if the key isn’t present
                        txtNewStatusKey.Text = Key.None.ToString();
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
            Key key = (Key)Enum.Parse(typeof(Key), txtStatusKey.Text.ToString());

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
            Key key = (Key)Enum.Parse(typeof(Key), txtNewStatusKey.Text.ToString());

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
