using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Properties;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Drawing;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class ToggleApplicationStateForm : Form, IObserver
    {
        //This 3 properties is from our Utilities from RObserver
        private Subject subject;
        private ContextMenu contextMenu;
        private MenuItem menuItem;
        //Store key used for the last profile - necessarily to clean when change profile
        private Keys lastKey;

        public ToggleApplicationStateForm(Subject subject)
        {
            InitializeComponent();


            subject.Attach(this);
            this.subject = subject;
            KeyboardHook.Enable();
            //TODO: Retrieve the profile settings of the user
            //this.txtStatusToggleKey.Text = ProfileSingleton.GetCurrent().UserPreferences.toggleStateKey;
            txtStatusToggleKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            this.txtStatusToggleKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtStatusToggleKey.TextChanged += new EventHandler(this.onStatusToggleKeyChange);
            InitializeContextualMenu();
        }

        //Not used yet
        public void Update(ISubject subject)
        {
            if ((subject as Subject).Message.code == MessageCode.PROFILE_CHANGED)
            {
                //Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), ProfileSingleton.GetCurrent().UserPreferences.toggleStateKey);
                //KeyboardHook.Remove(lastKey); //Remove last key hook to prevent toggle with last profile key used.

                //this.txtStatusToggleKey.Text = currentToggleKey.ToString();
                //KeyboardHook.Add(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));
                //lastKey = currentToggleKey;
            }
        }

        #region private methods
        private void InitializeContextualMenu()
        {
            this.contextMenu = new ContextMenu();
            this.menuItem = new MenuItem();

            this.contextMenu.MenuItems.AddRange(
                new MenuItem[] { this.menuItem });

            this.menuItem.Index = 0;
            this.menuItem.Text = "Close";
            //this.menuItem.Click += new EventHandler(this.notifyShutdownApplication);

            this.notifyIconTray.ContextMenu = this.contextMenu;

        }
        private void onStatusToggleKeyChange(object sender, EventArgs e)
        {
            //Get last key from profile before update it in json
            Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), this.txtStatusToggleKey.Text);
            KeyboardHook.Remove(lastKey);
            KeyboardHook.Add(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));
           // ProfileSingleton.GetCurrent().UserPreferences.toggleStateKey = currentToggleKey.ToString(); //Update profile key
            //ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);

            lastKey = currentToggleKey; //Refresh lastKey to update 
        }

        private bool toggleStatus()
        {
            bool isOn = this.btnStatusToggle.Text == "On";
            if(isOn)
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
                if(client != null)
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

        //private void notifyIconDoubleClick(object sender, MouseEventArgs e)
        //{
        //    this.subject.Notify(new Utils.Message(MessageCode.CLICK_ICON_TRAY, null));
        //}

        //private void notifyShutdownApplication(object Sender, EventArgs e)
        //{
        //    // Close the form, which closes the application.
        //    this.subject.Notify(new Utils.Message(MessageCode.SHUTDOWN_APPLICATION, null));
        //}
        #endregion

        private void btnStatusToggle_Click(object sender, EventArgs e)
        {
            this.toggleStatus();
        }
    }
}
