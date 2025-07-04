using ApplicationLayer.Models.RagnarokModels;
using ApplicationLayer.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Windows.Forms;
using System.Text.Json;
using System.Windows.Input;
using Domain.Model.DataModels;
using ApplicationLayer.Interface.RagnarokInterface;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Singleton.RagnarokSingleton;
using Infrastructure.Utilities;
using Domain.Constants;
using ApplicationLayer.Interface;
using ApplicationLayer.Designer;

namespace ApplicationLayer.ChildForms
{

    public partial class MacroSwitchForm : Form, IObserverService
    {
        private ThreadUtility ThreadUtility;
        public string email;
        private readonly SubjectService _subjectService;
        private readonly IBaseTableService _baseTableService;
        private readonly IUserSettingService _userSettingService;
        public MacroSwitchForm(SubjectService subjectService, IBaseTableService baseTableService, IUserSettingService userSettingService)
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
            var jsonObjectMacroSwitch = await GetDeserializedObject<MacroSwitch>(async () => (await ReturnToggleKey()).MacroSwitch);
            ThreadUtility = new ThreadUtility(_ =>
            {
                jsonObjectMacroSwitch.MacroExecutionThreadSwitch(client);
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
                await DisplayMacroSwitch();
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
                catch (Exception ex)
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
            await _userSettingService.SaveChangesAsync(userToggleState);

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
            await _userSettingService.SaveChangesAsync(userToggleState);

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
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
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
    }
}
