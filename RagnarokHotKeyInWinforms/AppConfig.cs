using System;
using System.IO;

namespace RagnarokHotKeyInWinforms
{
    internal class AppConfig
    {
        public static string _4RClientsURL = "https://storage.googleapis.com/4rtools/supported_servers.json";
        public static string LocalResourcePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        public static string Name = "RagnarokTool";
        public static string Version = "v1.0.0";
    }
}
