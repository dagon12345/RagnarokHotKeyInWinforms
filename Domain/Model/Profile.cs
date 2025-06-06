using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RagnarokHotKeyInWinforms.Model
{
    public class ProfileSingleton
    {
        public static Profile profile = new Profile("Default");
        public static int TOTAL_MACRO_LANES_FOR_SONGS = 4;
        public static int TOTAL_MACRO_LANES = 3;
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
                    profile.AHK = JsonConvert.DeserializeObject<AHK>(Profile.GetByAction(rawObject, profile.AHK));
                    profile.AutopotYgg = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.AutopotYgg));
                    profile.StatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(Profile.GetByAction(rawObject, profile.StatusRecovery));
                    profile.AutoRefreshSpammer = JsonConvert.DeserializeObject<AutoRefreshSpammer>(Profile.GetByAction(rawObject, profile.AutoRefreshSpammer));
                    profile.AutoBuff = JsonConvert.DeserializeObject<AutoBuff>(Profile.GetByAction(rawObject, profile.AutoBuff));
                    profile.SongMacro = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.SongMacro));
                    profile.AtkDefMode = JsonConvert.DeserializeObject<ATKDefMode>(Profile.GetByAction(rawObject, profile.AtkDefMode));
                    profile.MacroSwitch = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.MacroSwitch));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Create new configuration
        public static void Create(string profileName)
        {
            //Stored in //bin/debug/profile
            string jsonFileName = AppConfig.ProfileFolder + profileName + ".json";
            if (!File.Exists(jsonFileName))
            {
                //If the directory is not existed or the file then create a new one.
                if (!Directory.Exists(AppConfig.ProfileFolder)) { Directory.CreateDirectory(AppConfig.ProfileFolder); }
                FileStream fs = File.Create(jsonFileName);
                fs.Close();
                Profile profile = new Profile(profileName);
                string output = JsonConvert.SerializeObject(profile, Formatting.Indented);
                File.WriteAllText(jsonFileName, output);
            }
            //Load if existed
            ProfileSingleton.Load(profileName);
        }
        //Delete the current configuration
        public static void Delete(string profileName)
        {
            try
            {
                if(profileName != "Default")
                {
                    File.Delete(AppConfig.ProfileFolder + profileName + ".json");
                }
            }
            catch
            {
            }
        }
        // set the default configuration set by the user.
        public static void SetConfiguration(Action action)
        {
            if (profile != null)
            {
                string jsonData = File.ReadAllText(AppConfig.ProfileFolder + profile.Name + ".json");
                dynamic jsonObj = JsonConvert.DeserializeObject(jsonData);
                jsonObj[action.GetActionName()] = action.GetConfiguration();
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(AppConfig.ProfileFolder + profile.Name + ".json", output);
            }
        }
        //get the current set by the user locally
        public static Profile GetCurrent()
        {
            return profile;
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

            public ATKDefMode AtkDefMode { get; set; }

            //Profile means user profile stored settings/configuration of the users.
            public Profile(string name)
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
            public static object GetByAction(dynamic obj, Action action)
            {
                if (obj != null & obj[action.GetActionName()] != null)
                {
                    return obj[action.GetActionName()].ToString();
                }
                return action.GetConfiguration();
            }

            public static List<string> ListAll()
            {
                List<string> profiles = new List<string>();
                try
                {
                    //Bin/debug/profile - We store the user settings in this folder.
                    string[] files = Directory.GetFiles(AppConfig.ProfileFolder);
                    foreach (string fileName in files)
                    {
                        string[] len = fileName.Split('\\');
                        string profileName = len[len.Length - 1].Split('.')[0];
                        profiles.Add(profileName);
                    }
                }
                catch { }
                return profiles;

            }

        }
    }
}
