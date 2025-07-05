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
using Domain.Constants;
using Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;
using RagnarokHotKeyInWinforms.RagnarokHotKeyInWinforms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private readonly IStoredCredentialService _storedCredentialService;
        private readonly SubjectService _subjectService;
        private string _userEmail; // Get the users email then distribute it on each form.
        public frm_Main(string userEmail, IStoredCredentialService storedCredentialService, SubjectService subjectService)
        {

            InitializeComponent();

            #region Logger Configuration
            LogStore.Entries.ListChanged += (s, e) => RefreshLogList();
            RefreshLogList();
            #endregion

            #region Interfaces
            _storedCredentialService = storedCredentialService;
            _subjectService = subjectService;
            #endregion

            #region Passed DataTypes
            _userEmail = userEmail;
            #endregion
            this.Text = AppConfig.Name + " - " + GlobalConstants.Version; // get the latest version

        }

 

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
        private async void Form1_Load(object sender, EventArgs e)
        {
            #region Mdi Container design
            DesignerService.ApplyDarkBlueTheme(this);


            #region Greeting text
            lblUsername.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblUsername.ReadOnly = true;
            lblUsername.BorderStyle = BorderStyle.None;
            lblUsername.BackColor = Color.FromArgb(23, 32, 42); // Deep navy


            var returnStoredCreds = await _storedCredentialService.SearchUser(_userEmail);
            lblUsername.Text = $"Welcome back, {returnStoredCreds.Name} !";


            #endregion


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
                var macroSwitchForm = Program.ServiceProvider.GetRequiredService<MacroSwitchForm>();

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

                //autoBuffSkillsForm form
                autoBuffSkillsForm.TopLevel = false;
                autoBuffSkillsForm.email = _userEmail;
                tabAutoBuffSkill.Controls.Add(autoBuffSkillsForm);
                autoBuffSkillsForm.Show();

                //macroSongForm form
                macroSongForm.TopLevel = false;
                macroSongForm.email = _userEmail;
                tabPageMacroSongs.Controls.Add(macroSongForm);
                macroSongForm.Show();

                //MacroSwitch form
                macroSwitchForm.TopLevel = false;
                macroSwitchForm.email = _userEmail;
                tabPageMacroSwitch.Controls.Add(macroSwitchForm);
                macroSwitchForm.Show();

                //Toggle which is last
                toggleForm.MdiParent = this;
                toggleForm.Location = new Point(18, 4);
                toggleForm.Size = new Size(340, 52);
                toggleForm.email = _userEmail;
                toggleForm.Show();

                StartUpdate();
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
                clients.AddRange(LocalServerManager.GetLocalClients());

                string securePath = Path.Combine(AppConfig.SecurePath, RagnarokConstants.SupportedServerJson);

                // 🔄 Fallback if file doesn't exist in secure folder
                if (!File.Exists(securePath))
                {
                    //where our supported_server.json stored
                    string fallbackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RagnarokConstants.SupportedServerJson);
                    if (!File.Exists(fallbackPath))
                        throw new FileNotFoundException("Fallback supported_servers.json not found at: " + fallbackPath);

                    Directory.CreateDirectory(AppConfig.SecurePath);
                    File.Copy(fallbackPath, securePath);
                }

                string localServerRaw = File.ReadAllText(securePath);
                clients.AddRange(JsonSerializer.Deserialize<List<ClientDto>>(localServerRaw));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load supported servers: " + ex.Message);
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
