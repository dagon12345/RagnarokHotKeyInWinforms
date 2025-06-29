using Domain.Model;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using System.Linq;
using Infrastructure.Utilities;
using System.Text.RegularExpressions;
using Domain.Constants;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class MacroSwitch
    {
        public static string ACTION_NAME_SONG_MACRO = "SongMacro2.0";
        public static string ACTION_NAME_MACRO_SWITCH = "MacroSwitch";
        public string actionName { get; set; }
        public List<ChainConfigSwitch> chainConfigs { get; set; } = new List<ChainConfigSwitch>();

        public void MacroExecutionThreadSwitch(Client roClient)
        {
            foreach (ChainConfigSwitch chainConfig in this.chainConfigs)
            {
                if (chainConfig.trigger != Key.None && Keyboard.IsKeyDown(chainConfig.trigger))
                {
                    var macro = chainConfig.macroEntries;

                    // Dynamically find and sort keys like "in9mac1", "in10mac1", etc.
                    var orderedKeys = macro.Keys
                        .Where(k => k.EndsWith($"mac{chainConfig.Id}"))
                        .Select(k => new
                        {
                            Key = k,
                            Index = int.TryParse(Regex.Match(k, @"in(\d+)mac").Groups[1].Value, out int idx) ? idx : int.MaxValue
                        })
                        .OrderBy(x => x.Index)
                        .Select(x => x.Key)
                        .ToList();

                    foreach (var keyName in orderedKeys)
                    {
                        if (!macro.TryGetValue(keyName, out MacroKeySwitch macroKey))
                        {
                            Console.WriteLine($"[MacroExecutionThreadSwitch] Missing key: {keyName}");
                            continue;
                        }

                        if (chainConfig.instrumentKey != Key.None)
                        {
                            Keys instrumentKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.instrumentKey.ToString());
                            InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, instrumentKey, 0);
                            Thread.Sleep(30);
                        }

                        Keys thisKey = (Keys)Enum.Parse(typeof(Keys), macroKey.key.ToString());
                        Thread.Sleep(macroKey.delay);
                        InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisKey, 0);

                        if (chainConfig.daggerKey != Key.None)
                        {
                            Keys daggerKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.daggerKey.ToString());
                            InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, daggerKey, 0);
                            Thread.Sleep(30);
                        }
                    }
                }
            }

            Thread.Sleep(50); // Let the thread breathe
        }
    }
}
