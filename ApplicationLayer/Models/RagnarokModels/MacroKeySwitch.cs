using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
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
}
