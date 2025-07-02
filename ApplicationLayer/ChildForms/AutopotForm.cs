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
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
namespace ApplicationLayer.ChildForms
{
    public partial class AutopotForm : Form, IObserverService
    {
        private readonly IUserSettingService _userSettingService;
        private readonly IBaseTableService _baseTableService;
        private readonly SubjectService _subjectService;
        private ThreadUtility ThreadUtility;
        public string email;// get the email of the user from login
        public AutopotForm(IUserSettingService userSettingService, IBaseTableService baseTableService, SubjectService subjectService)
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            _subjectService = subjectService;
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _subjectService.Attach(this);
            _ = LoadAsync();
        }
        private async Task LoadAsync()
        {
            try
            {
                await RetrieveAutopot();
                await SkillTimerRetrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
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
            var jsonObjectAutoPot = await GetDeserializedObject<Autopot>(async () => (await ReturnToggleKey()).Autopot);
            ThreadUtility = new ThreadUtility(_ =>
            {
                jsonObjectAutoPot?.AutopotThreadExecution(client, 0);
                Task.Delay(50).Wait(); // Safe exit
            });
        }
        private void TriggerStopActions()
        {
            ThreadUtility?.Stop();
        }
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }

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
            txtHpKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            txtHpKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            txtHpKey.TextChanged += async (sender, e) => await onHpTextChange(sender, e);

            //HPPercent Controls
            txtHPpct.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            txtHPpct.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            txtHPpct.TextChanged += async (sender, e) => await txtHPpctTextChanged(sender, e);

            //SPKey controls
            txtSpKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            txtSpKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            txtSpKey.TextChanged += async (sender, e) => await onSpTextChange(sender, e);

            //SpPercent Controls
            txtSPpct.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            txtSPpct.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            txtSPpct.TextChanged += async (sender, e) => await txtSPpctTextChanged(sender, e);


            //Delay
            txtAutopotDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            txtAutopotDelay.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
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
                await _userSettingService.SaveChangesAsync(userToggleState);
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
                await _userSettingService.SaveChangesAsync(userToggleState);
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
                await _userSettingService.SaveChangesAsync(userToggleState);
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
                await _userSettingService.SaveChangesAsync(userToggleState);
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
                await _userSettingService.SaveChangesAsync(userToggleState);
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
            this.txtSkillTimerKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            this.txtSkillTimerKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
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
                await _userSettingService.SaveChangesAsync(userToggleState);
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
                await _userSettingService.SaveChangesAsync(userToggleState);
            }
            else
            {
                return;
            }
        }

        #endregion SkillTimer

    }
}
