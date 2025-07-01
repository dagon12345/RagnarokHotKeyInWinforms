using ApplicationLayer.Interface;
using Domain.Model.DataModels;
using Domain.Security;
using FluentResults;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Interface;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ApplicationLayer.Service
{
    public class PasswordRecoveryService
    {
        private readonly IStoredCredentialService _storedCredentialService;
        private readonly IEmailService _emailService;
        private readonly IHasher _hasHer;
        public PasswordRecoveryService(IStoredCredentialRepository credentialRepository, IEmailService emailService, 
            IStoredCredentialService storedCredentialService, IHasher hasHer)
        {
            _storedCredentialService = storedCredentialService;
            _emailService = emailService;
            _hasHer = hasHer;
        }

        public async Task<StoredCredential> SendResetLink(string email)
        {
            var searchUser = await _storedCredentialService.SearchUser(email);
            if (searchUser == null) return null;


            searchUser.PasswordResetToken = Guid.NewGuid().ToString();
            searchUser.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);

            string rawGuid = Guid.NewGuid().ToString("N"); // 32-character hex without dashes
            string confirmationCode = rawGuid.Substring(0, 6).ToUpper(); // E.g., "A2C9FD"

            searchUser.SetConfirmationCode(confirmationCode, _hasHer, TimeSpan.FromMinutes(30));

            await _storedCredentialService.SaveChangesAsync(searchUser);

            await _emailService.SendPasswordResetLinkAsync(email, confirmationCode);
            return searchUser;
        }
        public async Task<Result> ResetPassword(string email, string newPassword)
        {
            var user = await _storedCredentialService.SearchUser(email);

            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(newPassword, salt);

            user.Salt = salt;
            user.PasswordHash = hash;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _storedCredentialService.SaveChangesAsync(user);
            return Result.Ok().WithSuccess("Password updated.");

        }
        public async Task<StoredCredential> ConfirmationOfToken(string email, string inputToken)
        {
            var user = await _storedCredentialService.SearchUser(email);
            if (user == null)
            {
                MessageBox.Show("Empty list retrieved");
                return null;
            }
            if (!user.IsValidConfirmationCode(inputToken, _hasHer))
            {
                MessageBox.Show("Invalid or expired Reset Token");
                return null;
            }
            return user;
        }
    }
}
