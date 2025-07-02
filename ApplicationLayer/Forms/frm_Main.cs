using ApplicationLayer.ChildForms;
using ApplicationLayer.Designer;
using ApplicationLayer.Dto.RagnarokDto;
using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using ApplicationLayer.Interface.RagnarokInterface;
using ApplicationLayer.Models.RagnarokModels;
using ApplicationLayer.Service;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Singleton.RagnarokSingleton;
using ApplicationLayer.Utilities;
using Domain.Constants;
using Domain.Model.DataModels;
using Infrastructure.Service;
using Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
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
using Cursors = System.Windows.Forms.Cursors;
using MessageCode = Domain.Constants.MessageCode;
namespace RagnarokHotKeyInWinforms
{
    public partial class frm_Main : Form, IObserverService
    {
       // private SubjectService subject = new SubjectService();//subject triggers the Update() method inside notify function
        private System.Windows.Forms.Timer progressTimer;
        private int progressIncrement; // Increment value for each tick
        private int targetProgress; // Target progress value
        List<ClientDto> clients = new List<ClientDto>(); // list of clients with address initiated
        private ThreadUtility ThreadUtility;

        private readonly IServiceProvider _serviceProvider;
        private readonly IStoredCredentialService _storedCredentialService;
        private readonly IUserSettingService _userSettingService;
        private readonly IBaseTableService _baseTableService;
        private readonly SubjectService _subjectService;
        private string _userEmail;
        public frm_Main(string userEmail, IServiceProvider serviceProvider, IStoredCredentialService storedCredentialService,
            IBaseTableService baseTableService, IUserSettingService userSettingService, SubjectService subjectService)
        {

            InitializeComponent();

            #region Child Forms
            _serviceProvider = serviceProvider;
            #endregion

            #region Logger Configuration
            LogStore.Entries.ListChanged += (s, e) => RefreshLogList();
            RefreshLogList();
            #endregion

            #region Interfaces
            _storedCredentialService = storedCredentialService;
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;
            _subjectService = subjectService;
            #endregion

            #region Passed DataTypes
            _userEmail = userEmail;
            #endregion
            this.Text = AppConfig.Name + " - " + AppConfig.Version; // Window title


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

        #region Public methods
        public void Update(ISubjectService subject)
        {
            switch ((subject as SubjectService).Message.code)
            {
                case MessageCode.TURN_ON:
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
                    //this.ShutdownApplication();
                    break;
            }
        }
        #endregion
        #region Private Methods
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(_userEmail);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }

        private void RefreshLogList()
        {
            logListBox.BeginUpdate();
            logListBox.Items.Clear();

            foreach (var entry in LogStore.SortedDescending)
            {
                logListBox.Items.Add(entry.ToString());
            }

            logListBox.EndUpdate();
            if (logListBox.Items.Count > 0)
                logListBox.SelectedIndex = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
     

            #region Mdi Container design
            DesignerService.ApplyDarkBlueTheme(this);
            logListBox.ForeColor = Color.Black;
            processCombobox.ForeColor = Color.Black;
            this.IsMdiContainer = true;

            foreach (Control ctrl in this.Controls)
            {

                if (ctrl is MdiClient client)
                {
                    client.BackColor = Color.FromArgb(23, 32, 42); // Deep navy
                    break;
                }
            }
            #endregion
            #region Children forms
            //Logout button
            btnLogout.Location = new Point(789, 7);
            btnLogout.Cursor = Cursors.Hand;
            try
            {

                _subjectService.Attach(this);

                var toggleForm = Program.ServiceProvider.GetRequiredService<ToggleApplicationForm>();
                var statusRecoveryForm = Program.ServiceProvider.GetRequiredService<StatusRecoveryForm>();
                var autopotForm = Program.ServiceProvider.GetRequiredService<AutopotForm>();
                var skillSpammerForm = Program.ServiceProvider.GetRequiredService<SkillSpammerForm>();
                var attackDefendModeForm = Program.ServiceProvider.GetRequiredService<AttackDefendModeForm>();
                var autoBuffStuffsForm = Program.ServiceProvider.GetRequiredService<AutoBuffStuffsForm>();
                var autoBuffSkillsForm = Program.ServiceProvider.GetRequiredService<AutoBuffSkillsForm>();
                var macroSongForm = Program.ServiceProvider.GetRequiredService<MacroSongsForm>();

                // Manually position the forms Location(left, right)Height(width, height)
                //Status recovery
                statusRecoveryForm.MdiParent = this;
                statusRecoveryForm.Location = new Point(199, 57);
                statusRecoveryForm.Size = new Size(159, 74);
                statusRecoveryForm.email = _userEmail;
                statusRecoveryForm.Show();

                //Autopot and skill timer form
                autopotForm.MdiParent = this;
                autopotForm.Location = new Point(17, 142);
                autopotForm.Size = new Size(246, 156);
                autopotForm.email = _userEmail;
                autopotForm.Show();


                //skillspammer
                skillSpammerForm.TopLevel = false;
                skillSpammerForm.email = _userEmail;
                tabPageSpammerAndAttack.Controls.Add(skillSpammerForm);
                skillSpammerForm.Show();

                //Attack defend mode form
                attackDefendModeForm.TopLevel = false;
                attackDefendModeForm.email = _userEmail;
                attackDefendModeForm.Location = new Point(424, 3);
                tabPageSpammerAndAttack.Controls.Add(attackDefendModeForm);
                attackDefendModeForm.Show();

                //Autobuff stuff form
                autoBuffStuffsForm.TopLevel = false;
                autoBuffStuffsForm.email = _userEmail;
                tabAutoBuffStuff.Controls.Add(autoBuffStuffsForm);
                autoBuffStuffsForm.Show();

                //Autobuff skills form
                autoBuffSkillsForm.TopLevel = false;
                autoBuffSkillsForm.email = _userEmail;
                tabAutoBuffSkill.Controls.Add(autoBuffSkillsForm);
                autoBuffSkillsForm.Show();

                //Autobuff skills form
                macroSongForm.TopLevel = false;
                macroSongForm.email = _userEmail;
                tabPageMacroSongs.Controls.Add(macroSongForm);
                macroSongForm.Show();


                //Toggle which is last
                toggleForm.MdiParent = this;
                toggleForm.Location = new Point(18, 4);
                toggleForm.Size = new Size(340, 52);
                toggleForm.email = _userEmail;
                toggleForm.Show();

                StartUpdate();
                refreshProcessList();
                //progressBar1.Value++;

                //var storedCreds = await _storedCredentialService.SearchUser(_userEmail);
                //progressBar1.Value++;

                //var getBaseTable = await _baseTableService.SearchUser(_userEmail);
                //progressBar1.Value++;

                ////If the user is new to the app automatically created a setting to the user.
                //await _userSettingService.UpsertUser(getBaseTable.ReferenceCode, storedCreds.Name);
                //progressBar1.Value++;

                //lblUserName.Text = $"Welcome back, {storedCreds.Name}";
                //progressBar1.Value++;

                //await Retrieve(); progressBar1.Value++;
                //await RetrieveAutopot(); progressBar1.Value++;
                //await SkillTimerRetrieve(); progressBar1.Value++;
                //await RetrieveStatusEffect(); progressBar1.Value++;
                //await AhkRetrieval(); progressBar1.Value++;
                //await RetrieveStuffAutobuffForm(); progressBar1.Value++;
                //await updateUi(); progressBar1.Value++;
                //await DisplayMacroSwitch(); progressBar1.Value++;
                //await DisplayAttackDefendMode(); progressBar1.Value++;
                refreshProcessList();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            #endregion

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
                clients.AddRange(JsonSerializer.Deserialize<List<ClientDto>>(localServerRaw));
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
        private void LoadServers(List<ClientDto> clients)
        {
            foreach (ClientDto clientDto in clients)
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
        private async Task Logout()
        {
            var searchUser = await _storedCredentialService.SearchUser(_userEmail);
            searchUser.LastLoginTime = searchUser.LastLoginTime.AddHours(-1);//Minus One hour so that it will surely go to the google auth again
            await _storedCredentialService.SaveChangesAsync(searchUser);
            this.Hide();//Hide the form then show the signin form
            var getUserInfoInterface = Program.ServiceProvider.GetRequiredService<IGetUserInfo>();
            var userSignIn = Program.ServiceProvider.GetRequiredService<ISignIn>();
            var storedCredential = Program.ServiceProvider.GetRequiredService<IStoredCredentialService>();
            var loginService = Program.ServiceProvider.GetRequiredService<LoginService>();
            var password = Program.ServiceProvider.GetRequiredService<PasswordRecoveryService>();
            var userSetting = Program.ServiceProvider.GetRequiredService<IUserSettingService>();
            var baseTable = Program.ServiceProvider.GetRequiredService<IBaseTableService>();
            SignInForm sf = new SignInForm(getUserInfoInterface, userSignIn, storedCredential, loginService, password, userSetting, baseTable);
            sf.ShowDialog();

        }
        private async void btnLogout_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to logout?","Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
            {
                await Logout();
            }
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
