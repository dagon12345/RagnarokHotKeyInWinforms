using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Forms;
using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static RagnarokHotKeyInWinforms.Model.ProfileSingleton;

namespace RagnarokHotKeyInWinforms
{
    public partial class frm_Main : Form, IObserver
    {
        private Subject subject = new Subject();
        private string currentProfile;
        private Timer progressTimer;
        private int progressIncrement; // Increment value for each tick
        private int targetProgress; // Target progress value
        List<ClientDTO> clients = new List<ClientDTO>(); // list of clients with address initiated
        public frm_Main()
        {
      
            this.subject.Attach(this);
            InitializeComponent();

            this.Text = AppConfig.Name + " - " + AppConfig.Version; // Window title
            //Container Configuration
            this.IsMdiContainer = true;
            SetBackGroundColorOfMDIForm();
            //Paint Children forms
            SetToggleApplicationStateWindow();
            SetAutopotWindow();
            SetAutopotYggWindow();
            SetSkillTimerWindow();
            SetAutoStatusEffectWindow();
            SetAHKWindow();//Tab spammer
            SetProfileWindow();//Profile
            SetAutobuffStuffWindow();//AutoBuff Stuff
            SetAutobuffSkillWindow();//AutoBuff Skill
            SetSongMacroWindow(); // Macro Song Form



        }
        //addform used for each forms
        public void addForm(TabPage tp, Form f)
        {
            if(!tp.Controls.Contains(f))
            {
                tp.Controls.Add(f);
                f.Dock = DockStyle.Fill;
                f.Show();
                Refresh();
            }
            Refresh();
        }
        #region Frames
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
        public void SetToggleApplicationStateWindow()
        {
            ToggleApplicationStateForm frm = new ToggleApplicationStateForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            int x = 0; // if 0 then it will dock in left side. The higher the number the more it docks in right
            //the higher the number the more it is going into downward position
            int y = 0; // You can set this to any desired value
            frm.Location = new Point(x, y);
            frm.MdiParent = this;
            frm.Show();
        }
        //TabControlAutopot "TabControl"
        public void SetAutopotWindow()
        {
            AutopotForm frm = new AutopotForm(subject, false);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.MdiParent = this;
            frm.Show();
            addForm(this.tabPageAutopot, frm);
        }
        public void SetAutopotYggWindow()
        {
            AutopotForm frm = new AutopotForm(subject, true);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.MdiParent = this;
            frm.Show();
            addForm(this.tabPageYggAutopot, frm);
        }
        public void SetSkillTimerWindow()
        {
            SkillTimerForm frm = new SkillTimerForm(subject);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.MdiParent = this;
            frm.Show();
            addForm(this.tabPageSkillTimer, frm);
        }
        public void SetAutoStatusEffectWindow()
        {
            StatusEffectForm form = new StatusEffectForm(subject);
            form.FormBorderStyle = FormBorderStyle.None;

            // Calculate the position for the right edge
            int x = 0; // if 0 then it will dock in left side. The higher the number the more it docks in right
            //the higher the number the more it is going into downward position
            int y = 135; // You can set this to any desired value

            form.Location = new Point(x, y);
            form.MdiParent = this;
            form.Show();
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
        private void Form1_Load(object sender, EventArgs e)
        {
            ProfileSingleton.Create("Default");
            StartUpdate();//Start the update to get the game address
            this.refreshProcessList(); // refresh the combobox and get the game
            this.refreshProfileList(); // Get the profile list
            this.profileCb.SelectedItem = "Default";
        }
        //load the local profile
        //NOTE: This method/function was used in the form "ProfileForm"
        public void refreshProfileList()
        {

            this.Invoke((MethodInvoker)delegate ()
            {
                this.profileCb.Items.Clear();
            });
            //retrieve the profile stored in  //bin/debug/profile
            foreach(string p in Profile.ListAll())
            {
                this.profileCb.Items.Add(p);
            }
            
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
                string localFilePath = Path.Combine(AppConfig.LocalResourcePath, "supported_servers.json");
                string localServerRaw = File.ReadAllText(localFilePath);
                clients.AddRange(JsonConvert.DeserializeObject<List<ClientDTO>>(localServerRaw));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            // Prepare for the progress bar
            if (clients.Count > 0)
            {
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
        private void SetBackGroundColorOfMDIForm()
        {
            foreach (Control ctl in this.Controls)
            {
                if ((ctl) is MdiClient)
                {
                    ctl.BackColor = Color.White;
                }

            }
        }
        #endregion

        private void brnRefresh_Click(object sender, EventArgs e)
        {
            this.refreshProcessList();
        }

        private void processCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //When clicked get the name of the charcter to preview in the label
            Client client = new Client(this.processCombobox.SelectedItem.ToString());
            ClientSingleton.Instance(client);
            characterName.Text = client.ReadCharacterName();
            //If message code was changed then get the character name
            subject.Notify(new Utilities.Message(Utilities.MessageCode.PROCESS_CHANGED, null));
        }
        //Load the profile
        private void profileCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.profileCb.Text != currentProfile)
            {
                try
                {
                    ProfileSingleton.Load(this.profileCb.Text); //Load the profile
                    subject.Notify(new Utilities.Message(MessageCode.PROFILE_CHANGED, null));
                    currentProfile = this.profileCb.Text.ToString();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
