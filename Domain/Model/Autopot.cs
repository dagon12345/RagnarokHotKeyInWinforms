using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

//Used int AutopotForm
namespace RagnarokHotKeyInWinforms.Model
{
    public class Autopot : Action
    {
        public static string actionNameAutopot { get; set; } = "Autopot";
        public static string ACTION_NAME_AUTOPOT_YGG = "AutopotYgg";

        public Key hpKey { get; set; }
        public int hpPercent { get; set; }
        public Key spKey { get; set; }
        public int spPercent { get; set; }
        public int delay { get; set; }
        public int delayYgg { get; set; }

        public string actionName { get; set; }
        private _4RThread thread;

        public Autopot() { }
        public Autopot(string actionName)
        {
            this.actionName = actionName;
        }
        public Autopot(Key hpKey, int hpPercent, int delay, Key spKey, int spPercent)
        {
            this.delay = delay;

            //HP means LIFE
            this.hpKey = hpKey;
            this.hpPercent = hpPercent;

            //SP means MANA
            this.spKey = spKey;
            this.spPercent = spPercent;
        }


        public void Start()
        {
            //If thread is running then stop and trigger the function after this.
            if (thread != null)
            {
                Stop();// Commented this and uncomment if thread later is needed.
            }
            Client roClient = ClientSingleton.GetClient();
            if(roClient != null)
            {
                int hpPotCount = 0;
                this.thread = new _4RThread(_ => AutopotThreadExecution(roClient, hpPotCount));
                _4RThread.Start(this.thread);
            }
        }

        public void Stop()
        {
            _4RThread.Stop(this.thread);
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return this.actionName != null ? this.actionName : actionNameAutopot;
        }

        #region Private methods
        private int AutopotThreadExecution(Client roClient, int hpPotCount)
        {
            //Check HP first
            if(roClient.IsHpBelow(hpPercent))
            {
                pot(this.hpKey);
                hpPotCount++;

                if(hpPotCount == 3)
                {
                    hpPotCount = 0;
                    if(roClient.IsSpBelow(spPercent))
                    {
                        pot(this.spKey);
                    }
                }
            }
            //Check sp mana
            if (roClient.IsSpBelow(spPercent))
            {
                pot(this.spKey);
            }
            Thread.Sleep(this.delay);//Assigned to 15
            return 0;
        }

        private void pot(Key key)
        {
            Keys k = (Keys)Enum.Parse(typeof(Keys), key.ToString());
            if((k != Keys.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_HOTKEY_MSG_ID, k, 0); //Keydown
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYUP_MSG_ID, k, 0); //Keyup

            }
        }
        #endregion
    }
}
