using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace RagnarokHotKeyInWinforms.Model
{
    public class StatusRecovery : Action
    {
        public static string ACTION_NAME_STATUS_AUTOBUFF = "StatusAutoBuff";

        private _4RThread thread;
        public Dictionary<EffectStatusIDs, Key> buffMapping { get; set; } = new Dictionary<EffectStatusIDs, Key>();
        public int delay { get; set; } = 1;

        public string GetActionName()
        {
            return ACTION_NAME_STATUS_AUTOBUFF;
        }

        public _4RThread RestoreStatusThread(Client client)
        {
            Client roClient = ClientSingleton.GetClient();
            _4RThread statusEffectsThread = new _4RThread(_ =>
            {
                for(int i = 0; i <= Constants.MAX_BUFF_LIST_INDEX_SIZE - 1;  i++)
                {
                    uint currentStatus = client.CurrentBuffStatusCode(i);
                    EffectStatusIDs status = (EffectStatusIDs)currentStatus;
                    if(buffMapping.ContainsKey((EffectStatusIDs)currentStatus))//IF FOR REMOVE STATUS - CHECK IF STATUS EXISTS IN STATUS LIST AND DO ACTION
                    {
                        //IF CONTAINS CURRENT STATUS ON DICTIONARY
                        Key key = buffMapping[(EffectStatusIDs)currentStatus];
                        if(Enum.IsDefined(typeof(EffectStatusIDs), currentStatus))
                        {
                            this.useStatusRecovery(key);
                        }
                    }
                }
                Thread.Sleep(this.delay);
                return 0;
            });

            return statusEffectsThread;
        }

        public void Start()
        {
            if(thread != null)
            {
               Stop();// Commented this and uncomment if thread later is needed.
            }

            Client roClient = ClientSingleton.GetClient();
            if(roClient != null)
            {
                this.thread = RestoreStatusThread(roClient);
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

        private void useStatusRecovery(Key key)
        {
            if ((key != Key.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, 
                    Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
        }
        public void AddKeyToBuff(EffectStatusIDs status, Key key)
        {
            if(buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }
            if(FormUtils.IsValidKey(key))
            {
                buffMapping.Add(status, key);
            }
        }
    }
}
