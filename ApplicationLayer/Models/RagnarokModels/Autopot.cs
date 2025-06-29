using Domain.Constants;
using Domain.Interface;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using Infrastructure.Utilities;
using ApplicationLayer.Singleton.RagnarokSingleton;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class Autopot : IAction
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
        private ThreadUtility ThreadUtility;

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
        }

        public void Stop()
        {
            if (ThreadUtility != null)
            {
                ThreadUtility.Stop();
                ThreadUtility = null; // Optionally set to null after stopping
            }
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
        public void AutopotThreadExecution(Client roClient, int hpPotCount)
        {
            if (roClient == null)
            {
                Console.WriteLine("Client not found. Exiting thread.");
                return;
            }

            //Check HP first
            if (roClient.IsHpBelow(hpPercent))
            {
                pot(this.hpKey);
                hpPotCount++;

                if (hpPotCount == 3)
                {
                    hpPotCount = 0;
                    if (roClient.IsSpBelow(spPercent))
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
            Task.Delay(50).Wait();

        }

        private void pot(Key key)
        {
            Keys k = (Keys)Enum.Parse(typeof(Keys), key.ToString());
            if ((k != Keys.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                InteropUtility.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_HOTKEY_MSG_ID, k, 0); //Keydown
                InteropUtility.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYUP_MSG_ID, k, 0); //Keyup

            }
        }
        #endregion
    }
}
