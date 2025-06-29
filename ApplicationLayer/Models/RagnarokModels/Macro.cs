using Domain.Constants;
using Domain.Interface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using Infrastructure.Utilities;
using ApplicationLayer.Singleton.RagnarokSingleton;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class Macro : IAction
    {
        public static string ACTION_NAME_SONG_MACRO = "SongMacro2.0";
        public static string ACTION_NAME_MACRO_SWITCH = "MacroSwitch";
        public string actionName { get; set; }
        private ThreadUtility ThreadUtility;
        public List<ChainConfig> chainConfigs { get; set; } = new List<ChainConfig>();

        public Macro(string macroName, int macroLanes)
        {
            this.actionName = macroName;
            for (int i = 1; i <= macroLanes; i++)
            {
                chainConfigs.Add(new ChainConfig(i, Key.None));
            }
        }
        public Macro()
        {

        }
        public void ResetMacro(int macroId)
        {
            try
            {
                chainConfigs[macroId - 1] = new ChainConfig(macroId);
            }
            catch (Exception) { }
        }

        public string GetActionName()
        {
            return this.actionName;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }
        public void MacroExecutionThread(Client roClient)
        {
            foreach (ChainConfig chainConfig in this.chainConfigs)
            {
                if (chainConfig.trigger != Key.None && Keyboard.IsKeyDown(chainConfig.trigger))
                {
                    Dictionary<string, MacroKey> macro = chainConfig.macroEntries;
                    for (int i = 1; i <= macro.Count; i++)//Ensure to execute keys in order
                    {
                        MacroKey macroKey = macro["in" + i + "mac" + chainConfig.Id];
                        if (chainConfig.instrumentKey != Key.None)
                        {
                            //Press instrumetn key if exists.
                            Keys instrumentKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.instrumentKey.ToString());
                            InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, instrumentKey, 0);
                            Thread.Sleep(30);
                        }
                        Keys thisk = (Keys)Enum.Parse(typeof(Keys), macroKey.Key.ToString());
                        Thread.Sleep(macroKey.delay);
                        InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);

                        if (chainConfig.daggerKey != Key.None)
                        {
                            //Press instrument key if exists
                            Keys daggerKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.daggerKey.ToString());
                            InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, daggerKey, 0);
                            Thread.Sleep(30);
                        }
                    }
                }
            }
            Task.Delay(50).Wait(); // Safe exit
        }

        public void Start()
        {
            //This checking the MacroSongForm thread. Stop only if the thread is not null else proceed with the function below.
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (ThreadUtility != null)
                {
                    Stop();
                }

                this.ThreadUtility = new ThreadUtility((_) => MacroExecutionThread(roClient));
            }
        }

        public void Stop()
        {
            ThreadUtility.Stop();
        }
    }
}
