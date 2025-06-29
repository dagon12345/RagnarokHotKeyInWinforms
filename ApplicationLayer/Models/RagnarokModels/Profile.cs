using Domain.Interface;
using RagnarokHotKeyInWinforms;
using System.Collections.Generic;
using System.IO;


namespace ApplicationLayer.Models.RagnarokModels
{
    public class Profile
    {
        public static int TOTAL_MACRO_LANES_FOR_SONGS = 4;
        public static int TOTAL_MACRO_LANES = 3;

        public string Name { get; set; }
        public UserPreferences UserPreferences { get; set; }
        public Ahk Ahk { get; set; }
        public Autopot Autopot { get; set; }
        public Autopot AutopotYgg { get; set; }
        public AutoRefreshSpammer AutoRefreshSpammer { get; set; }
        public AutoBuff AutoBuff { get; set; }
        public StatusRecovery StatusRecovery { get; set; }
        public Macro SongMacro { get; set; }
        public Macro MacroSwitch { get; set; }

        public AttackDefendMode AttackDefendMode { get; set; }

        //Profile means user profile stored settings/configuration of the users.
        public Profile(string name)
        {
            this.Name = name;
            this.UserPreferences = new UserPreferences();
            this.Ahk = new Ahk();
            this.Autopot = new Autopot(Autopot.actionNameAutopot);
            this.AutopotYgg = new Autopot(Autopot.ACTION_NAME_AUTOPOT_YGG);
            this.AutoRefreshSpammer = new AutoRefreshSpammer();
            this.AutoBuff = new AutoBuff();
            this.StatusRecovery = new StatusRecovery();
            this.SongMacro = new Macro(Macro.ACTION_NAME_SONG_MACRO, TOTAL_MACRO_LANES_FOR_SONGS);
            this.MacroSwitch = new Macro(Macro.ACTION_NAME_MACRO_SWITCH, TOTAL_MACRO_LANES);
            this.AttackDefendMode = new AttackDefendMode();
        }
        public static object GetByAction(dynamic obj, IAction action)
        {
            if (obj != null & obj[action.GetActionName()] != null)
            {
                return obj[action.GetActionName()].ToString();
            }
            return action.GetConfiguration();
        }
    }

}
