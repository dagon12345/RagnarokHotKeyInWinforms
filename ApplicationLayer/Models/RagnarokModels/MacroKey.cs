using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class MacroKey
    {
        public Key Key { get; set; }
        public int delay { get; set; } = 50;

        public MacroKey(Key key, int delay)
        {
            this.Key = key;
            this.delay = delay;
        }
    }
}
