using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.Model.DataModels;
using Domain.Model.ModelConfigurations;
using Infrastructure.Repositories.Interface;
using Newtonsoft.Json;
using RagnarokHotKeyInWinforms;
using RagnarokHotKeyInWinforms.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using static RagnarokHotKeyInWinforms.Model.ProfileSingleton;

namespace ApplicationLayer.Service
{
    public class UserSettingService : IUserSettingService
    {
        public static Profile profileConfiguration = new Profile("Default");
        private readonly IUserSettingRepository _userSettingRepository;
        public UserSettingService(IUserSettingRepository userSettingRepository)
        {
            _userSettingRepository = userSettingRepository;
        }

        public void Load(Guid referenceCode)
        {
            try
            {

                //This File is located into directory bin/Debug/Profile/Default.json
                string json = File.ReadAllText(AppConfig.ProfileFolder + RagnarokConstants.DefaultJson);
                dynamic jsonData = JsonConvert.DeserializeObject<dynamic>(json);

                if (jsonData != null)
                {
                    // Populate Singleton profile with data from the database
                    profile.Name = jsonData.Name;
                    //profile.UserPreferences = JsonConvert.DeserializeObject<UserPreferences>(GetByAction(jsonData, profileConfiguration.UserPreferences));
                    profile.AHK = JsonConvert.DeserializeObject<AHK>(GetByAction(jsonData, profileConfiguration.AHK));
                    profile.AutopotYgg = JsonConvert.DeserializeObject<Autopot>(GetByAction(jsonData, profileConfiguration.AutopotYgg));
                    profile.StatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(GetByAction(jsonData, profileConfiguration.StatusRecovery));
                    profile.AutoRefreshSpammer = JsonConvert.DeserializeObject<AutoRefreshSpammer>(GetByAction(jsonData, profileConfiguration.AutoRefreshSpammer));
                    profile.AutoBuff = JsonConvert.DeserializeObject<AutoBuff>(GetByAction(jsonData, profileConfiguration.AutoBuff));
                    profile.SongMacro = JsonConvert.DeserializeObject<Macro>(GetByAction(jsonData, profileConfiguration.SongMacro));
                    profile.AtkDefMode = JsonConvert.DeserializeObject<ATKDefMode>(GetByAction(jsonData, profileConfiguration.AtkDefMode));
                    profile.MacroSwitch = JsonConvert.DeserializeObject<Macro>(GetByAction(jsonData, profileConfiguration.MacroSwitch));
                }
                else
                {
                    throw new Exception("Profile not found in database.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static object GetByAction(dynamic obj, RagnarokHotKeyInWinforms.Model.Action action)
        {
            if (obj != null & obj[action.GetActionName()] != null)
            {
                return obj[action.GetActionName()].ToString();
            }
            return action.GetConfiguration();
        }

        public async Task<UserSettings> SearchByReferenceCode(Guid referenceCode)
        {
            var searchedUser = await _userSettingRepository.FindUserReferenceCode(referenceCode);
            return searchedUser;
        }

        public async Task UpsertUser(Guid referenceCode, string Name)
        {
            //This File is located into directory bin/Debug/Profile/Default.json
            string json = File.ReadAllText(AppConfig.ProfileFolder + RagnarokConstants.DefaultJson);
            dynamic jsonData = JsonConvert.DeserializeObject<dynamic>(json);

            var userSettings = await _userSettingRepository.FindUserReferenceCode(referenceCode);
            //If none is search then create a new user setting
            if(userSettings == null)
            {
                userSettings = new UserSettings
                {
                    ReferenceCode = referenceCode,
                    Name = Name,
                    UserPreferences = JsonConvert.SerializeObject(profileConfiguration.UserPreferences),
                    Ahk = JsonConvert.SerializeObject(profileConfiguration.AHK),
                    AutopotYgg = JsonConvert.SerializeObject(profileConfiguration.AutopotYgg),
                    StatusRecovery = JsonConvert.SerializeObject(profileConfiguration.StatusRecovery),
                    AutoRefreshSpammer = JsonConvert.SerializeObject(profileConfiguration.AutoRefreshSpammer),
                    Autobuff = JsonConvert.SerializeObject(profileConfiguration.AutoBuff),
                    SongMacro = JsonConvert.SerializeObject(profileConfiguration.SongMacro),
                    AtkDefMode = JsonConvert.SerializeObject(profileConfiguration.AtkDefMode),
                    MacroSwitch = JsonConvert.SerializeObject(profileConfiguration.MacroSwitch)
                };
                _userSettingRepository.Add(userSettings);
            }
            //Update settings
            //else
            //{
            //    // Update existing profile
            //    //userSettings.UserPreferences = JsonConvert.SerializeObject(profileConfiguration.UserPreferences);
            //    userSettings.Ahk = JsonConvert.SerializeObject(profileConfiguration.AHK);
            //    userSettings.AutopotYgg = JsonConvert.SerializeObject(profileConfiguration.AutopotYgg);
            //    userSettings.StatusRecovery = JsonConvert.SerializeObject(profileConfiguration.StatusRecovery);
            //    userSettings.AutoRefreshSpammer = JsonConvert.SerializeObject(profileConfiguration.AutoRefreshSpammer);
            //    userSettings.Ahk = JsonConvert.SerializeObject(profileConfiguration.AutoBuff);
            //    userSettings.SongMacro = JsonConvert.SerializeObject(profileConfiguration.SongMacro);
            //    userSettings.AtkDefMode = JsonConvert.SerializeObject(profileConfiguration.AtkDefMode);
            //    userSettings.MacroSwitch = JsonConvert.SerializeObject(profileConfiguration.MacroSwitch);
            //}
            await _userSettingRepository.SaveChangesAsync();
        }

        public async Task<UserSettings> SelectUserPreference(Guid referenceCode)
        {
            var userPreference = await _userSettingRepository.SelectUserPreference(referenceCode);
            return userPreference;
        }

        public async Task SaveChangesAsync()
        {
            await _userSettingRepository.SaveChangesAsync();
        }
    }
}
