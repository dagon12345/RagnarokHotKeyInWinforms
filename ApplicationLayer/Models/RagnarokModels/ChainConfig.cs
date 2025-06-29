using System.Collections.Generic;
using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class ChainConfig
    {
        public int Id { get; set; }
        public Key trigger { get; set; }
        public Key daggerKey { get; set; }
        public Key instrumentKey { get; set; }
        public int delay { get; set; }
        public Dictionary<string, MacroKey> macroEntries { get; set; } = new Dictionary<string, MacroKey>();

        public ChainConfig()
        {

        }
        public ChainConfig(int id)
        {
            this.Id = id;
            this.macroEntries = new Dictionary<string, MacroKey>();
        }
        public ChainConfig(ChainConfig macro)
        {
            this.Id = macro.Id;
            this.delay = macro.delay;
            this.trigger = macro.trigger;
            this.daggerKey = macro.daggerKey;
            this.instrumentKey = macro.instrumentKey;
            this.macroEntries = new Dictionary<string, MacroKey>(macro.macroEntries);
        }
        public ChainConfig(int Id, Key trigger)
        {
            this.Id = Id;
            this.trigger = trigger;
            this.macroEntries = new Dictionary<string, MacroKey>();
        }
    }
}
