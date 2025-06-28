using ApplicationLayer.Interface;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Interface;
using System;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public class PasswordRecoveryService
    {
        private readonly IStoredCredentialRepository _credentialRepository;
        private readonly IEmailService _emailService;
        public PasswordRecoveryService(IStoredCredentialRepository credentialRepository, IEmailService emailService)
        {
           _credentialRepository = credentialRepository;
           _emailService = emailService;
        }

        public async void SendResetLink(string email)
        {
            var user = await _credentialRepository.GetByEmail(email);
            if (user == null) return;

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            await _credentialRepository.SaveChangesAsync();
            await _emailService.Send(email, "Reset your password", "Link sent!");
        }
        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var user = await _credentialRepository.GetPasswordResetToken(token, DateTime.UtcNow);
            if (user == null) return false;

            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(newPassword, salt);

            user.Salt = salt;
            user.PasswordHash = hash;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _credentialRepository.SaveChangesAsync();
            return true;

        }
    }
}
