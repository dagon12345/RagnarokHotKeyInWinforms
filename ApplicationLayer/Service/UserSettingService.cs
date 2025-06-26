using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.ErrorMessages;
using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using Infrastructure.Service;
using Newtonsoft.Json;
using RagnarokHotKeyInWinforms;
using System;
using System.IO;
using System.Threading;
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
            try
            {
                var searchedUser = await _userSettingRepository.FindUserReferenceCode(referenceCode);
                return searchedUser;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                return null;
            }

        }

        public async Task UpsertUser(Guid referenceCode, string Name)
        {
            try
            {
                //This File is located into directory bin/Debug/Profile/Default.json
                string json = File.ReadAllText(AppConfig.ProfileFolder + RagnarokConstants.DefaultJson);
                dynamic jsonData = JsonConvert.DeserializeObject<dynamic>(json);

                var userSettings = await _userSettingRepository.FindUserReferenceCode(referenceCode);
                //If none is search then create a new user setting
                if (userSettings == null)
                {
                    userSettings = new UserSettings
                    {
                        ReferenceCode = referenceCode,
                        Name = Name,
                        UserPreferences = JsonConvert.SerializeObject(profileConfiguration.UserPreferences),
                        Ahk = JsonConvert.SerializeObject(profileConfiguration.AHK),
                        Autopot = JsonConvert.SerializeObject(profileConfiguration.Autopot),
                        AutopotYgg = JsonConvert.SerializeObject(profileConfiguration.AutopotYgg),
                        StatusRecovery = JsonConvert.SerializeObject(profileConfiguration.StatusRecovery),
                        AutoRefreshSpammer = JsonConvert.SerializeObject(profileConfiguration.AutoRefreshSpammer),
                        Autobuff = JsonConvert.SerializeObject(profileConfiguration.AutoBuff),
                        SongMacro = JsonConvert.SerializeObject(profileConfiguration.SongMacro),
                        MacroSwitch = JsonConvert.SerializeObject(profileConfiguration.MacroSwitch),
                        AtkDefMode = JsonConvert.SerializeObject(profileConfiguration.AtkDefMode),
                    };
                    _userSettingRepository.Add(userSettings);
                    await _userSettingRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
            }
           
        }

        public async Task<UserSettings> SelectUserPreference(Guid referenceCode)
        {
            try
            {
                var userPreference = await _userSettingRepository.SelectUserPreference(referenceCode);
                return userPreference;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                return null;
            }

        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _userSettingRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
            }
        }
    }
}
