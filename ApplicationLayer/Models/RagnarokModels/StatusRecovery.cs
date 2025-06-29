using ApplicationLayer.Singleton.RagnarokSingleton;
using Domain.Constants;
using Domain.Interface;
using Infrastructure.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class StatusRecovery : IAction
    {
        public static string ACTION_NAME_STATUS_AUTOBUFF = "StatusAutoBuff";

        private ThreadUtility ThreadUtility;
        public Dictionary<EffectStatusIdEnum, Keys> buffMapping { get; set; } = new Dictionary<EffectStatusIdEnum, Keys>();
        public int delay { get; set; } = 1;

        public void RestoreStatusThread(Client client)
        {

            for (int i = 0; i <= Constants.MAX_BUFF_LIST_INDEX_SIZE - 1; i++)
            {
                uint currentStatus = client.CurrentBuffStatusCode(i);
                EffectStatusIdEnum status = (EffectStatusIdEnum)currentStatus;
                if (buffMapping.ContainsKey((EffectStatusIdEnum)currentStatus))//IF FOR REMOVE STATUS - CHECK IF STATUS EXISTS IN STATUS LIST AND DO ACTION
                {
                    //IF CONTAINS CURRENT STATUS ON DICTIONARY
                    Keys keys = buffMapping[(EffectStatusIdEnum)currentStatus];
                    if (Enum.IsDefined(typeof(EffectStatusIdEnum), currentStatus))
                    {
                        this.useStatusRecovery(keys);
                    }
                }
            }
            Task.Delay(50).Wait(); // Safe exit

        }

        public void Start()
        {

            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {

                if (ThreadUtility != null)
                {
                    Stop();// Commented this and uncomment if thread later is needed.
                }
            }
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

        private void useStatusRecovery(Keys key)
        {
            if ((key != Keys.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                InteropUtility.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle,
                    Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
        }
        public void AddKeyToBuff(EffectStatusIdEnum status, Keys key)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }
            if (FormUtilities.IsValidKey(key))
            {
                buffMapping.Add(status, key);
            }
        }

        public string GetActionName()
        {
            return ACTION_NAME_STATUS_AUTOBUFF;
        }
    }
}
