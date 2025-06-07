using System.Windows.Input;

namespace Domain.Model.JsonModels
{
    public class AutoRefereshSpammerSetting
    {
        public int refreshDelay { get; set; }
        public Key refreshKey { get; set; }
    }
}
