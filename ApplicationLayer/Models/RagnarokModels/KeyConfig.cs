using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class KeyConfig
    {
        public Key key { get; set; }
        public bool ClickActive { get; set; }

        public KeyConfig(Key key, bool clickActive)
        {
            this.key = key;
            ClickActive = clickActive;
        }
    }
}
