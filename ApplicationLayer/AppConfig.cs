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

        public static void Load(string projectRoot = null)
        {
            if (string.IsNullOrEmpty(projectRoot))
            {
                // Go up three levels to reach the project root from bin/Debug
                //var projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                //path = Path.Combine(projectRoot, "ApplicationLayer" ,"appsettings.json");

                projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
               // path = Path.Combine(projectRoot, "ApplicationLayer", "appsettings.json");
            }

            if (!File.Exists(projectRoot))
                throw new FileNotFoundException("Config file not found at: " + projectRoot);

            var json = File.ReadAllText(projectRoot);
            dynamic parsed = JsonConvert.DeserializeObject(json);

            SshSettings = JsonConvert.DeserializeObject<SshSettings>(parsed.Ssh.ToString());
            DatabaseSettings = JsonConvert.DeserializeObject<DatabaseSettings>(parsed.Database.ToString());
        }
    }
}
