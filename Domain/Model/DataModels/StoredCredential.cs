using System;

namespace Domain.Model.DataModels
{
    public class StoredCredential
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset LastLoginTime { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
    }
}
