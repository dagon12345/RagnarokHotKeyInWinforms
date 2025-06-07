using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.Model.DataModels;
using Domain.Model.JsonModels;
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
        private AutopotSetting autopotSetting = new AutopotSetting();
        private Subject subject = new Subject();
        private Timer progressTimer;
        private int progressIncrement; // Increment value for each tick
        private int targetProgress; // Target progress value
        List<ClientDTO> clients = new List<ClientDTO>(); // list of clients with address initiated
        private Keys lastKey;

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
            //SetAutoStatusEffectWindow();
            //SetAHKWindow();//Tab spammer
            //SetProfileWindow();//Profile
            //SetAutobuffStuffWindow();//AutoBuff Stuff
            //SetAutobuffSkillWindow();//AutoBuff Skill
            //SetSongMacroWindow(); // Macro Song Form
            //SetAtkDefWindow();//AtkDef tab page
            //SetMacroSwitchWindow();
        }
        #region ToggleApplicationStateFunction
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
            var jsonObject = JsonSerializer.Deserialize<UserPreferenceSetting>(toggleStateValue.UserPreferences);
            this.txtStatusToggleKey.Text = jsonObject.toggleStateKey;
            txtStatusToggleKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
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
            var jsonObject = JsonSerializer.Deserialize<UserPreferenceSetting>(toggleStateValue.UserPreferences);
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
        #region AutopotSettings
        private async Task RetrieveAutopot()
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutopotSetting>(userToggleState.Autopot);

            txtHpKey.Text = jsonObject.hpKey.ToString() ?? "0";
            txtSpKey.Text = jsonObject.spKey.ToString() ?? "0";
            txtHPpct.Text = jsonObject.hpPercent.ToString() ?? "0";
            txtSPpct.Text = jsonObject.spPercent.ToString() ?? "0";
            txtAutopotDelay.Text = jsonObject.delay.ToString() ?? "0";

            //HPkey Controls
            txtHpKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            txtHpKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHpKey.TextChanged += async (sender, e) => await onHpTextChange(sender, e);

            //HPPercent Controls
            txtHPpct.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            txtHPpct.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHPpct.TextChanged += async (sender, e) => await txtHPpctTextChanged(sender, e);

            //SPKey controls
            txtSpKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            txtSpKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSpKey.TextChanged += async (sender, e) => await onSpTextChange(sender, e);

            //SpPercent Controls
            txtSPpct.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            txtSPpct.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSPpct.TextChanged += async (sender, e) => await txtSPpctTextChanged(sender, e);


            //Delay
            txtAutopotDelay.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            txtAutopotDelay.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtAutopotDelay.TextChanged += async (sender, e) => await txtAutopotDelayTextChanged(sender, e);
        }
        #region HpTexAndPercent
        private async Task onHpTextChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutopotSetting>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtHpKey.Text);

            if (jsonObject != null)
            {
                jsonObject.hpKey = key;
                jsonObject.actionName = autopotSetting.actionName;
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
            var jsonObject = JsonSerializer.Deserialize<AutopotSetting>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtHPpct.Text);

            if (jsonObject != null)
            {
                jsonObject.hpPercent = Convert.ToInt32(key);
                jsonObject.actionName = autopotSetting.actionName;
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
        #region SpTextAndPercent
        private async Task onSpTextChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutopotSetting>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtSpKey.Text);

            if (jsonObject != null)
            {
                jsonObject.spKey = key;
                jsonObject.actionName = autopotSetting.actionName;
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
            var jsonObject = JsonSerializer.Deserialize<AutopotSetting>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtSPpct.Text);

            if (jsonObject != null)
            {
                jsonObject.spPercent = Convert.ToInt32(key);
                jsonObject.actionName = autopotSetting.actionName;
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
            var jsonObject = JsonSerializer.Deserialize<AutopotSetting>(userToggleState.Autopot);
            Key key = (Key)Enum.Parse(typeof(Key), txtAutopotDelay.Text);

            if (jsonObject != null)
            {
                jsonObject.delay = Convert.ToInt32(key);
                jsonObject.actionName = autopotSetting.actionName;
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
        #region SkillTimer
        private async Task SkillTimerRetrieve()
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoRefereshSpammerSetting>(userToggleState.AutoRefreshSpammer);

            txtAutoRefreshDelay.Text = jsonObject.refreshDelay.ToString() ?? "0";
            txtSkillTimerKey.Text = jsonObject.refreshKey.ToString() ?? "0";

            //Default values from hotkeys
            this.txtSkillTimerKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            this.txtSkillTimerKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtSkillTimerKey.TextChanged += new EventHandler(this.onSkillTimerKeyChange);
            this.txtAutoRefreshDelay.ValueChanged += new EventHandler(this.txtAutoRefreshDelayTextChanged);
        }
        private async void onSkillTimerKeyChange(object sender, EventArgs e)
        {
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoRefereshSpammerSetting>(userToggleState.AutoRefreshSpammer);

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
            var jsonObject = JsonSerializer.Deserialize<AutoRefereshSpammerSetting>(userToggleState.AutoRefreshSpammer);

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
        #region Status Recovery Effect Form
        private void KeyPressFunction()
        {
        
        }
        private async Task RetrieveStatusEffect()
        {
            try
            {
                var userToggleState = await ReturnToggleKey();
                var jsonObject = JsonSerializer.Deserialize<StatusRecoverySetting>(userToggleState.StatusRecovery);

                if (jsonObject.buffMapping.Count > 0)
                {
                    txtStatusKey.Text = jsonObject.buffMapping[EffectStatusIDs.SILENCE].ToString() ?? "None";
                    txtNewStatusKey.Text = jsonObject.buffMapping[EffectStatusIDs.PROPERTYUNDEAD].ToString() ?? "None";
                }

                this.txtStatusKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
                this.txtStatusKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                this.txtStatusKey.TextChanged += new EventHandler(onStatusKeyChange);
                this.txtNewStatusKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
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
            var jsonObject = JsonSerializer.Deserialize<StatusRecoverySetting>(userToggleState.StatusRecovery);
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
                // Persist changes
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
            var jsonObject = JsonSerializer.Deserialize<StatusRecoverySetting>(userToggleState.StatusRecovery);
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
                // Persist changes
                await _userSettingService.SaveChangesAsync();
            }
            else
            {
                return;

            }
        }
        #endregion
        #endregion Status Effect From
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
        public void SetAutobuffSkillWindow()
        {
            SkillAutoBuffForm frm = new SkillAutoBuffForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            addForm(this.tabAutoBuffSkill, frm);
            frm.Show();
        }
        public void SetAutobuffStuffWindow()
        {
            StuffAutoBuffForm frm = new StuffAutoBuffForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            frm.Show();
            addForm(this.tabAutoBuffStuff, frm);
        }
        public void SetProfileWindow()
        {
            ProfileForm frm = new ProfileForm(this);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            frm.Show();
            addForm(this.tabPageProfiles, frm);
        }
        public void SetAHKWindow()
        {
            AHKForm frm = new AHKForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Location = new Point(0, 65);
            frm.MdiParent = this;
            frm.Show();
            addForm(this.tabPageSpammer, frm);
        }

        #endregion
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
            this.refreshProcessList();

        }
        #region Public methods
        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.TURN_ON:
                case MessageCode.PROFILE_CHANGED:
                    Client client = ClientSingleton.GetClient();
                    if (client != null)
                    {
                        characterName.Text = ClientSingleton.GetClient().ReadCharacterName();
                    }
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
        #endregion
        #region Private Methods
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
                progressTimer = new Timer();
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
        #endregion
        private void processCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //When clicked get the name of the charcter to preview in the label
            Client client = new Client(this.processCombobox.SelectedItem.ToString());
            ClientSingleton.Instance(client);
            characterName.Text = client.ReadCharacterName();
            //If message code was changed then get the character name
            subject.Notify(new Utilities.Message(Utilities.MessageCode.PROCESS_CHANGED, null));
        }
        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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
    }
}
