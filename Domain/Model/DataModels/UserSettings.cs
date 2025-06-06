using System;

namespace Domain.Model.DataModels
{
    public class UserSettings
    {
        public Guid Id { get; set; }
        public Guid ReferenceCode { get; set; }
        public string Name { get; set; }
        public string UserPreferences { get; set; }
        public string Ahk { get; set; }
        public string Autopot { get; set; }
        public string AutopotYgg { get; set; }
        public string AutoRefreshSpammer { get; set; }
        public string Autobuff { get; set; }
        public string StatusRecovery { get; set; }
        public string SongMacro { get; set; }
        public string MacroSwitch { get; set; }
        public string AtkDefMode { get; set; }
    }
}
