using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class AHKForm : Form, IObserver
    {
        public AHKForm(Subject subject)
        {
            InitializeComponent();
            InitializeCheckAsThreeState();
            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:

                    RemoveHandlers();
                    FormUtils.ResetForm(this);
                    SetLegendDefaultValues();
                    InitializeCheckAsThreeState();

                    RadioButton rdAhkMode = (RadioButton)this.groupAhkConfig.Controls[ProfileSingleton.GetCurrent().AHK.ahkMode];
                    if (rdAhkMode != null) { rdAhkMode.Checked = true; };
                    this.txtSpammerDelay.Text = ProfileSingleton.GetCurrent().AHK.AhkDelay.ToString();
                    this.chkNoShift.Checked = ProfileSingleton.GetCurrent().AHK.noShift;
                    this.chkMouseFlick.Checked = ProfileSingleton.GetCurrent().AHK.mouseFlick;
                    this.DisableControlsIfSpeedBoost();

                    Dictionary<string, KeyConfig> ahkClones = new Dictionary<string, KeyConfig>(ProfileSingleton.GetCurrent().AHK.AhkEntries);

                    foreach (KeyValuePair<string, KeyConfig> config in ahkClones)
                    {
                        ToggleCheckboxByName(config.Key, config.Value.ClickActive);
                    }

                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().AHK.Start();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AHK.Stop();
                    break;
            }
        }
        #region private methods
        private void ToggleCheckboxByName(string Name, bool state)
        {
            try
            {
                CheckBox checkBox = (CheckBox)this.Controls.Find(Name, true)[0];
                checkBox.CheckState = state ? CheckState.Checked : CheckState.Indeterminate;
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AHK);
            }
            catch { }
        }
        private void DisableControlsIfSpeedBoost()
        {
            if (ProfileSingleton.GetCurrent().AHK.ahkMode == AHK.SPEED_BOOST)
            {
                this.chkMouseFlick.Enabled = false;
                this.chkNoShift.Enabled = false;
            }
            else
            {
                this.chkMouseFlick.Enabled = true;
                this.chkNoShift.Enabled = true;
            }
        }
        private void SetLegendDefaultValues()
        {
            //No mouse click cbWithNoClick
            this.cbWithNoClick.ThreeState = true;
            this.cbWithNoClick.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.cbWithNoClick.AutoCheck = false;
            //With mouse click cbWithClick
            this.cbWithClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWithClick.ThreeState = true;
            this.cbWithClick.AutoCheck = false;
        }
        private void RemoveHandlers()
        {
            foreach(Control c in this.Controls)
            {
                if(c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    check.CheckStateChanged -= onCheckChange;
                }
            }
        }
        private void onCheckChange(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;

            Key key = (Key)new KeyConverter().ConvertFromString(checkbox.Text);
            bool haveMouseClick = checkbox.CheckState == CheckState.Checked ? true : false;

            //If there is a checked key in checkbox then register this to the profile that was selected.
            if (checkbox.CheckState == CheckState.Checked || checkbox.CheckState == CheckState.Indeterminate)
                ProfileSingleton.GetCurrent().AHK.AddAHKEntry(checkbox.Name, new KeyConfig(key, haveMouseClick));
            else
                //Uncheck remove the current checked
                ProfileSingleton.GetCurrent().AHK.RemoveAHKEntry(checkbox.Name);
            //get current once the profile was loaded
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AHK);
        }
        private void InitializeCheckAsThreeState()
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    if ((check.Name.Split(new[] { "chk" }, StringSplitOptions.None).Length == 2))
                    {
                        check.ThreeState = true;
                    };

                    if (check.Enabled)
                        check.CheckStateChanged += onCheckChange;
                }
        }
        #endregion
    }
}
