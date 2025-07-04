using Domain.Model.SettingModels;
using Newtonsoft.Json;
using System;
using System.IO;

namespace RagnarokHotKeyInWinforms
{
    internal class AppConfig
    {
       
        public static string LocalResourcePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        public static string Name = "RagnarokTool";
        public static string Version = "v1.0.0";
        public static string ProfileFolder = "Profile\\"; //bin/debug/profile

        //Connection Configuration
        public static SshSettings SshSettings { get; private set; }
        public static DatabaseSettings DatabaseSettings { get; private set; }

        public static void Load(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                path = Path.Combine(basePath, "appsettings.json"); // If file is in bin\Debug


            }

            if (!File.Exists(path))
                throw new FileNotFoundException("Config file not found at: " + path);

            var json = File.ReadAllText(path);
            dynamic parsed = JsonConvert.DeserializeObject(json);

            SshSettings = JsonConvert.DeserializeObject<SshSettings>(parsed.Ssh.ToString());
            DatabaseSettings = JsonConvert.DeserializeObject<DatabaseSettings>(parsed.Database.ToString());
        }
    }
}
