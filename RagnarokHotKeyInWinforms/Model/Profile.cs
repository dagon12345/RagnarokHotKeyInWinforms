using Newtonsoft.Json;
using System.IO;

namespace RagnarokHotKeyInWinforms.Model
{
    public class ProfileSingleton
    {
        public static Profile profile = new Profile("Default");

        public static void Load(string profileName)
        {
            try
            {
                //This File is located into directory bin/Debug/Profile/Default.json
                string json = File.ReadAllText(AppConfig.ProfileFolder + profileName + ".json");
                dynamic rawObject = JsonConvert.DeserializeObject(json);

                if((rawObject != null))
                {
                    profile.Name = profileName;
                    profile.UserPreferences = JsonConvert.DeserializeObject<UserPreferences>(Profile.GetByAction(rawObject, profile.UserPreferences));
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        //Constructor for Profile
        public class Profile
        {
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

            public int MyProperty { get; set; }

            public static object GetByAction(dynamic obj, Action action)
            {
                if(obj != null & obj[action.GetActionName()] != null)
                {
                    return obj[action.GetActionName()].ToString();
                }
                return action.GetConfiguration();
            }

        }
    }
}
