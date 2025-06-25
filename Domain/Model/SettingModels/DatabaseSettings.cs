namespace Domain.Model.SettingModels
{
    public class DatabaseSettings
    {
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
