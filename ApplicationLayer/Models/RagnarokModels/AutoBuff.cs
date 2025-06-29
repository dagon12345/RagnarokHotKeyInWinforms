using ApplicationLayer.Singleton.RagnarokSingleton;
using Domain.Constants;
using Domain.Interface;
using Infrastructure.Utilities;
using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class AutoBuff : IAction
    {
        public static string ACTION_NAME_AUTOBUFF = "Autobuff";

        private ThreadUtility ThreadUtility;
        public int delay { get; set; } = 1;
        public Dictionary<EffectStatusIdEnum, Key> buffMapping { get; set; } = new Dictionary<EffectStatusIdEnum, Key>();


        public string GetActionName()
        {
            return ACTION_NAME_AUTOBUFF;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Start()
        {
            //If thread is running in the background, then trigger Stop function.
            if (ThreadUtility != null)
            {
                Stop();
            }
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                // this.thread = AutoBuffThread(roClient);
            }
        }
        public void AutoBuffThread(Client c)
        {

            bool foundQuag = false;
            Dictionary<EffectStatusIdEnum, Key> bmClone = new Dictionary<EffectStatusIdEnum, Key>(this.buffMapping);
            for (int i = 0; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
            {
                uint currentStatus = c.CurrentBuffStatusCode(i);
                EffectStatusIdEnum status = (EffectStatusIdEnum)currentStatus;

                if (status == EffectStatusIdEnum.OVERTHRUSTMAX)
                {
                    if (buffMapping.ContainsKey(EffectStatusIdEnum.OVERTHRUST))
                    {
                        bmClone.Remove(EffectStatusIdEnum.OVERTHRUST);
                    }
                }

                if (buffMapping.ContainsKey(status)) //CHECK IF STATUS EXISTS IN STATUS LIST AND DO ACTION
                {
                    bmClone.Remove(status);
                }

                if (status == EffectStatusIdEnum.QUAGMIRE) foundQuag = true;
            }

            foreach (var item in bmClone)
            {
                if (foundQuag && (item.Key == EffectStatusIdEnum.CONCENTRATION || item.Key == EffectStatusIdEnum.INC_AGI || 
                    item.Key == EffectStatusIdEnum.TRUESIGHT || item.Key == EffectStatusIdEnum.ADRENALINE))
                {
                    break;
                }
                else if (c.ReadCurrentHp() >= Constants.MINIMUM_HP_TO_RECOVER)
                {
                    this.useAutobuff(item.Value);
                    Thread.Sleep(10);
                }
            }

            Thread.Sleep(300);


        }

        public void AddKeyToBuff(EffectStatusIdEnum status, Key key)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }

            if (FormUtils.IsValidKey(key))
            {
                buffMapping.Add(status, key);
            }
        }
        public void ClearKeyMapping()
        {
            buffMapping.Clear();
        }
        public void Stop()
        {
            ThreadUtility.Stop();
        }
        private void useAutobuff(Key key)
        {
            if ((key != Key.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                InteropUtility.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
        }
    }
}
