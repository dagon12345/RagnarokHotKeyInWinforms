using System;

namespace Domain.Model.DataModels
{
    public class StoredCredential
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime LastLoginTime { get; set; } // DateTime datatype for MySql
        public string UserEmail { get; set; }
        public string Name { get; set; }
    }
}
