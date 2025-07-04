
using ApplicationLayer.Models.RagnarokModels;
using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.RagnarokHotKeyInWinforms;
using System;
using System.IO;

namespace ApplicationLayer.Singleton.RagnarokSingleton
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

                if ((rawObject != null))
                {
                    //All of this is from our profilesingleton settings set its default value.
                    profile.Name = profileName;
                    profile.UserPreferences = JsonConvert.DeserializeObject<UserPreferences>(Profile.GetByAction(rawObject, profile.UserPreferences));
                    profile.Ahk = JsonConvert.DeserializeObject<Ahk>(Profile.GetByAction(rawObject, profile.Ahk));
                    profile.AutopotYgg = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.AutopotYgg));
                    profile.StatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(Profile.GetByAction(rawObject, profile.StatusRecovery));
                    profile.AutoRefreshSpammer = JsonConvert.DeserializeObject<AutoRefreshSpammer>(Profile.GetByAction(rawObject, profile.AutoRefreshSpammer));
                    profile.AutoBuff = JsonConvert.DeserializeObject<AutoBuff>(Profile.GetByAction(rawObject, profile.AutoBuff));
                    profile.SongMacro = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.SongMacro));
                    profile.AttackDefendMode = JsonConvert.DeserializeObject<AttackDefendMode>(Profile.GetByAction(rawObject, profile.AttackDefendMode));
                    profile.MacroSwitch = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.MacroSwitch));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
