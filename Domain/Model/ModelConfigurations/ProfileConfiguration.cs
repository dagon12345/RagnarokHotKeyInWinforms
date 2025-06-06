using RagnarokHotKeyInWinforms.Model;

namespace Domain.Model.ModelConfigurations
{
    public class ProfileConfiguration
    {
        public static int TOTAL_MACRO_LANES_FOR_SONGS = 4;
        public static int TOTAL_MACRO_LANES = 3;

        public string Name { get; set; }
        public UserPreferences UserPreferences { get; set; }
        public AHK AHK { get; set; }
        public Autopot Autopot { get; set; }
        public Autopot AutopotYgg { get; set; }
        public AutoRefreshSpammer AutoRefreshSpammer { get; set; }
        public AutoBuff AutoBuff { get; set; }
        public StatusRecovery StatusRecovery { get; set; }
        public Macro SongMacro { get; set; }
        public Macro MacroSwitch { get; set; }
        public ATKDefMode AtkDefMode { get; set; }

        public ProfileConfiguration(string name)
        {
            this.Name = name;
            this.UserPreferences = new UserPreferences();
            this.AHK = new AHK();
            this.Autopot = new Autopot(Autopot.ACTION_NAME_AUTOPOT);
            this.AutopotYgg = new Autopot(Autopot.ACTION_NAME_AUTOPOT_YGG);
            this.AutoRefreshSpammer = new AutoRefreshSpammer();
            this.AutoBuff = new AutoBuff();
            this.StatusRecovery = new StatusRecovery();
            this.SongMacro = new Macro(Macro.ACTION_NAME_SONG_MACRO, TOTAL_MACRO_LANES_FOR_SONGS);
            this.MacroSwitch = new Macro(Macro.ACTION_NAME_MACRO_SWITCH, TOTAL_MACRO_LANES);
            this.AtkDefMode = new ATKDefMode();
        }
    }
}
