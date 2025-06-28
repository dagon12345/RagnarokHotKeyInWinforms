using Domain.Security;
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

        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string ConfirmationToken { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        public DateTime? ConfirmationTokenExpiry { get; private set; }

        public void SetConfirmationCode(string plainToken, IHasher hasher, TimeSpan validity)
        {
            ConfirmationToken = hasher.Hash(plainToken);
            ConfirmationTokenExpiry = DateTime.UtcNow.Add(validity);
        }

        public bool IsValidConfirmationCode(string inputToken, IHasher hasher)
        {
            return ConfirmationTokenExpiry >= DateTime.UtcNow &&
                   hasher.Verify(inputToken, ConfirmationToken);
        }

        public void ConfirmEmail()
        {
            ConfirmationToken = null;
            ConfirmationTokenExpiry = null;
            IsEmailConfirmed = true;
        }
    }
}
