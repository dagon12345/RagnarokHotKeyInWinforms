using Domain.Constants;
using Domain.Model.SettingModels;
using Infrastructure.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RagnarokHotKeyInWinforms
{
    namespace RagnarokHotKeyInWinforms
    {
        public class AppConfig
        {
            public static string Name = "RagnarokTool";
            public static string Version = "v1.0.0";
            public static string ProfileFolder = "Profile\\"; //bin/debug/profile

            public static string SecurePath => Path.Combine(
             Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
             Name, "secure");
            public static string GameAddressPath => Path.Combine(SecurePath, RagnarokConstants.SupportedServerJson);

            public static SshSettings SshSettings { get; private set; }
            public static DatabaseSettings DatabaseSettings { get; private set; }
            public static GameAddressConfig GameAddress { get; private set; }

            public static void Load(string path = null)
            {
                var securePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Name, "secure");

                Directory.CreateDirectory(securePath);
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(securePath, "secrets.json");
                }


                // Optional setup logic: create encrypted config if missing
                if (!File.Exists(path))
                {
                    var fallbackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                    if (!File.Exists(fallbackPath))
                        throw new FileNotFoundException("Fallback config not found at: " + fallbackPath);

                    var json = File.ReadAllText(fallbackPath);
                    var parsed = JsonConvert.DeserializeObject(json);
                    SecureConfig.SaveEncrypted(path, parsed);
                }

                var config = SecureConfig.LoadEncrypted<dynamic>(path);
                SshSettings = JsonConvert.DeserializeObject<SshSettings>(config.Ssh.ToString());
                DatabaseSettings = JsonConvert.DeserializeObject<DatabaseSettings>(config.Database.ToString());
            }
            public static void LoadConfig()
            {
                Directory.CreateDirectory(SecurePath);

                // Load or create gameaddress.json
                if (!File.Exists(GameAddressPath))
                {
                    var fallback = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RagnarokConstants.SupportedServerJson);
                    if (File.Exists(fallback))
                    {
                        File.Copy(fallback, GameAddressPath);
                    }
                    else
                    {
                        File.WriteAllText(GameAddressPath, JsonConvert.SerializeObject(new GameAddressConfig()));
                    }
                }

                var json = File.ReadAllText(GameAddressPath);
                // ✅ Correct
                List<GameAddressConfig> gameAddresses = JsonConvert.DeserializeObject<List<GameAddressConfig>>(json);
                GameAddress = gameAddresses.FirstOrDefault(); // or use the whole list if needed

            }
        }
    }
}


