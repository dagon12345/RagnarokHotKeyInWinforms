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

namespace ApplicationLayer.ChildForms
{
    public partial class ToggleApplicationForm : Form, IObserverService
    {
       
        private Keys lastKey;
        private readonly IBaseTableService _baseTableService;
        private readonly IUserSettingService _userSettingService;
        public string email;//get the users email from login
        private ThreadUtility ThreadUtility;
        private readonly SubjectService _subjectService;
        public ToggleApplicationForm(IBaseTableService baseTableService,
            IUserSettingService userSettingService, SubjectService subjectService)
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            Designer();
            _subjectService = subjectService;
            _baseTableService = baseTableService;
            _userSettingService = userSettingService;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _subjectService.Attach(this);
            KeyboardHook.Enable();
            _ = LoadAsync(); // Fire and forget safely
        }

        public async void Update(ISubjectService subject)
        {
            // throw new NotImplementedException();
            await Retrieve();
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
            toggleStatus();
        }
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }
        #region Designer
        private void Designer()
        {
            btnStart.Click += Button_Click;
            txtNotification.ReadOnly = true;
            txtNotification.BackColor = Color.FromArgb(23, 32, 42); // Deep navy
        }
        #endregion
        #region ToggleApplicationStateFunction (No Start Method)
        private bool toggleStatus()
        {
            bool isOn = this.btnStart.Text == "On";
            if (isOn)
            {
                this.btnStart.BackColor = Color.Crimson;
                this.btnStart.Text = "Off";
                //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_off;
                _subjectService.Notify(new Utilities.Message(MessageCode.TURN_OFF, null));
                this.txtNotification.Text = "Press the button to start!";
                //new SoundPlayer(Resources._4RTools.ETCResource.Speech_Off).Play();
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if (client != null)
                {
                    this.btnStart.BackColor = Color.Green;
                    this.btnStart.Text = "On";
                    //this.notifyIconTray.Icon = Resources._4RTools.ETCResource.logo_4rtools_on;
                    _subjectService.Notify(new Utilities.Message(MessageCode.TURN_ON, null));
                    this.txtNotification.Text = "Press the button to stop!";
                    this.txtNotification.ForeColor = Color.White;
                    //new SoundPlayer(Resources._4RTools.ETCResource.Speech_On).Play();
                }
                else
                {
                    this.txtNotification.Text = "Please select a valid Ragnarok Client!";
                    this.txtNotification.ForeColor = Color.Red;
                }
            }
           return true;
        }
        private async Task<T> GetDeserializedObject<T>(Func<Task<string>> getJsonData)
        {
            var jsonData = await getJsonData();
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        private async Task TriggerStartActions()
        {
            Client client = ClientSingleton.GetClient();
            var toggle = await ReturnToggleKey(); // Single call

            //var jsonObjectAhk = await GetDeserializedObject<Ahk>(() => Task.FromResult(toggle.Ahk));
            //var jsonObjectAutopot = await GetDeserializedObject<Autopot>(() => Task.FromResult(toggle.Autopot));
            //var jsonObjectAutoRefresh = await GetDeserializedObject<AutoRefreshSpammer>(() => Task.FromResult(toggle.AutoRefreshSpammer));
            //var jsonObjectStatusRecovery = await GetDeserializedObject<StatusRecovery>(() => Task.FromResult(toggle.StatusRecovery));
            //var jsonObjectAutoBuff = await GetDeserializedObject<AutoBuff>(() => Task.FromResult(toggle.Autobuff));
            //var jsonObjectMacroSong = await GetDeserializedObject<Macro>(() => Task.FromResult(toggle.SongMacro));
            //var jsonObjectMacroSwitch = await GetDeserializedObject<MacroSwitch>(() => Task.FromResult(toggle.MacroSwitch));
            //var jsonObjectAtkDef = await GetDeserializedObject<AttackDefendMode>(() => Task.FromResult(toggle.AtkDefMode));
            ThreadUtility = new ThreadUtility(_ =>
            {
                //jsonObjectAhk?.AHKThreadExecution(client);
               // jsonObjectAutopot?.AutopotThreadExecution(client, 0);
               // jsonObjectAutoRefresh?.AutorefreshThreadExecution(client);
                //jsonObjectStatusRecovery?.RestoreStatusThread(client);
               // jsonObjectAutoBuff?.AutoBuffThread(client);
               // jsonObjectMacroSong?.MacroExecutionThread(client);
               // jsonObjectMacroSwitch?.MacroExecutionThreadSwitch(client);
                //jsonObjectAtkDef?.AttacKDefAHKThreadExecution(client);
                Task.Delay(50).Wait(); // Safe exit
            });
        }
        private void TriggerStopActions()
        {
            ThreadUtility?.Stop();
        }

        private async Task Retrieve()
        {
            var toggleStateValue = await ReturnToggleKey();
            // Parse JSON and extract toggleStateKey
            var jsonObject = JsonSerializer.Deserialize<UserPreferences>(toggleStateValue.UserPreferences);
            this.txtKey.Text = jsonObject.toggleStateKey;

            txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
            this.txtKey.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
            this.txtKey.TextChanged += async (sender, e) => await onStatusToggleKeyChange(sender, e);

        }
        private async Task onStatusToggleKeyChange(object sender, EventArgs e)
        {
            var toggleStateValue = await ReturnToggleKey();
            //Get last key from profile before update it in json
            Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), txtKey.Text);
            KeyboardHook.Remove(lastKey);
           // KeyboardHook.Add(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));


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
