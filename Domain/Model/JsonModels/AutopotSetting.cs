using System.Windows.Input;

namespace Domain.Model.JsonModels
{
    public class AutopotSetting
    {
        public Key hpKey { get; set; }
        public int hpPercent { get; set; }
        public Key spKey { get; set; }
        public int spPercent { get; set; }
        public int delay { get; set; }
        public int delayYgg { get; set; }
        public string actionName { get; set; } = "Autopot";
    }
}
