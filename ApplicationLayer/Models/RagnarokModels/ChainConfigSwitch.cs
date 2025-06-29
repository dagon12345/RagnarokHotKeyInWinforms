using System.Collections.Generic;
using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
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
}
