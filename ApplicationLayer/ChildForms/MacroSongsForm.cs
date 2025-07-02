using ApplicationLayer.Models.RagnarokModels;
using Infrastructure.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows.Forms;
using Domain.Model.DataModels;
using System.Text.Json;
using System.Windows.Input;
using ApplicationLayer.Interface;
using ApplicationLayer.Designer;
using ApplicationLayer.Utilities;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Interface.RagnarokInterface;
using ApplicationLayer.Singleton.RagnarokSingleton;
using Domain.Constants;

namespace ApplicationLayer.ChildForms
{
    public partial class MacroSongsForm : Form, IObserverService
    {
        public string email;
        private readonly IUserSettingService _userSettingService;
        private readonly IBaseTableService _baseTableService;
        private readonly SubjectService _subjectService;
        private ThreadUtility ThreadUtility;
        public MacroSongsForm(IUserSettingService userSettingService, IBaseTableService baseTableService, SubjectService subjectService)
        {
            InitializeComponent();
            DesignerService.ApplyDarkBlueTheme(this);
            _subjectService = subjectService;
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;


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
            var jsonObjectMacroSong = await GetDeserializedObject<Macro>(async () => (await ReturnToggleKey()).SongMacro);
            ThreadUtility = new ThreadUtility(_ =>
            {
                jsonObjectMacroSong?.MacroExecutionThread(client);
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
                await GetMacroSongsForm();
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
                await _userSettingService.SaveChangesAsync(userToggleState);
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
            await _userSettingService.SaveChangesAsync(userToggleState);
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
                        chainConfig.macroEntries[macroEntryKey].Key = Key.None;
                    }
                }
            }

            // **Save changes to database BEFORE updating UI**
            userToggleState.SongMacro = JsonSerializer.Serialize(jsonObject);
            await _userSettingService.SaveChangesAsync(userToggleState);

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
        private async Task GetMacroSongsForm()
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
                        textBox.Text = chainConfig.macroEntries[cbName].Key.ToString();
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
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtilities.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtilities.OnKeyPress);
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

        private void panelMacro2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
