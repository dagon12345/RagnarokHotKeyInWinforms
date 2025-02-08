using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms
{
    public partial class frm_Main : Form, IObserver
    {
        private Subject subject = new Subject();
        private Timer progressTimer;
        private int progressIncrement; // Increment value for each tick
        private int targetProgress; // Target progress value
        List<ClientDTO> clients = new List<ClientDTO>();
        public frm_Main()
        {
      
            this.subject.Attach(this);
            InitializeComponent();

            this.Text = AppConfig.Name + " - " + AppConfig.Version; // Window title
            //Container Configuration
            this.IsMdiContainer = true;
            SetBackGroundColorOfMDIForm();
        

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartUpdate();
            this.refreshProcessList(); // refresh the combobox and get the game
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
                progressTimer.Interval = 100; // Set timer interval (100 ms)
                progressTimer.Tick += ProgressTimer_Tick; // Attach tick event handler
                progressTimer.Start(); // Start the timer
            }
            else
            {
                // No clients to load, close the form immediately
                //this.Close();
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
    }
}
