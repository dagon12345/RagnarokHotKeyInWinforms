using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class StoredCredentialRepository : BaseRepository, IStoredCredentialRepository
    {
        public StoredCredentialRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<StoredCredential> FindUserCredential(string accessToken)
        {
            var findCredential = await _context.StoredCredentials
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AccessToken == accessToken);
            return findCredential;
        }
        public async Task<StoredCredential> SearchUser(string userEmail)
        {
            var searchUserEmail = await _context.StoredCredentials
                .FirstOrDefaultAsync(x => x.UserEmail.Equals(userEmail));
            return searchUserEmail;
        }

        public async Task<StoredCredential> GetByEmail(string email)
        {
           var emailRetrieved = await _context.StoredCredentials.FirstOrDefaultAsync(u => u.UserEmail == email);
            if (emailRetrieved == null) return null;
            return emailRetrieved;
        }
        public async Task<StoredCredential> GetPasswordResetToken(string passwordResetToken, DateTime passwordResetTokenExpiry)
        {
            var passwordResetTokenRetrieve = await _context.StoredCredentials
                .FirstOrDefaultAsync(x => x.PasswordResetToken == passwordResetToken
                && x.PasswordResetTokenExpiry > DateTime.UtcNow);
            return passwordResetTokenRetrieve;
        }
    }
}
