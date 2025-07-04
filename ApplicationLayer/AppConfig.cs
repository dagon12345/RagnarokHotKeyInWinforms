using Domain.Model.SettingModels;
using Infrastructure.Helpers;
using Newtonsoft.Json;
using System;
using System.IO;

namespace RagnarokHotKeyInWinforms
{
    namespace RagnarokHotKeyInWinforms
    {
        public class AppConfig
        {
            public static string LocalResourcePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            public static string Name = "RagnarokTool";
            public static string Version = "v1.0.0";
            public static string ProfileFolder = "Profile\\"; //bin/debug/profile

            public static SshSettings SshSettings { get; private set; }
            public static DatabaseSettings DatabaseSettings { get; private set; }

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
        }
    }
}


