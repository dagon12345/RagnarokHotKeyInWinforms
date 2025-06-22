using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domain.Model
{
    public class MacroKeySwitch
    {
        public Key key { get; set; }
        public int delay { get; set; } = 50;

        public MacroKeySwitch(Key key, int delay)
        {
            this.key = key;
            this.delay = delay;
        }
    }
    public class ChainConfigSwitch
    {
        public int Id { get; set; }
        public Key trigger { get; set; }
        public Key daggerKey { get; set; }
        public Key instrumentKey { get; set; }
        public int delay { get; set; }
        public Dictionary<string, MacroKeySwitch> macroEntries { get; set; } = new Dictionary<string, MacroKeySwitch>();

        public ChainConfigSwitch()
        {

        }
        public ChainConfigSwitch(int id)
        {
            this.Id = id;
            this.macroEntries = new Dictionary<string, MacroKeySwitch>();
        }
        public ChainConfigSwitch(ChainConfigSwitch macro)
        {
            this.Id = macro.Id;
            this.delay = macro.delay;
            this.trigger = macro.trigger;
            this.daggerKey = macro.daggerKey;
            this.instrumentKey = macro.instrumentKey;
            this.macroEntries = new Dictionary<string, MacroKeySwitch>(macro.macroEntries);
        }
        public ChainConfigSwitch(int Id, Key trigger)
        {
            this.Id = Id;
            this.trigger = trigger;
            this.macroEntries = new Dictionary<string, MacroKeySwitch>();
        }
    }
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
                            Interop.PostMessage(roClient.process.MainWindowHandle, RagnarokHotKeyInWinforms.Utilities.Constants.WM_KEYDOWN_MSG_ID, instrumentKey, 0);
                            Thread.Sleep(30);
                        }

                        Keys thisKey = (Keys)Enum.Parse(typeof(Keys), macroKey.key.ToString());
                        Thread.Sleep(macroKey.delay);
                        Interop.PostMessage(roClient.process.MainWindowHandle, RagnarokHotKeyInWinforms.Utilities.Constants.WM_KEYDOWN_MSG_ID, thisKey, 0);

                        if (chainConfig.daggerKey != Key.None)
                        {
                            Keys daggerKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.daggerKey.ToString());
                            Interop.PostMessage(roClient.process.MainWindowHandle, RagnarokHotKeyInWinforms.Utilities.Constants.WM_KEYDOWN_MSG_ID, daggerKey, 0);
                            Thread.Sleep(30);
                        }
                    }
                }
            }

            Thread.Sleep(50); // Let the thread breathe
        }



    }
}
