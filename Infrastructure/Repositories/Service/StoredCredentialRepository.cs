using Domain.Model.DataModels;
using Infrastructure.Factory;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class StoredCredentialRepository : BaseRepository, IStoredCredentialRepository
    {
        protected readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public StoredCredentialRepository(IDbContextFactory<ApplicationDbContext> context) : base(context)
        {
            _contextFactory = context;
        }

        public async Task<StoredCredential> FindUserCredential(string accessToken)
        {
            var context = _contextFactory.CreateDbContext();
            var findCredential = await context.StoredCredentials
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AccessToken == accessToken);
            return findCredential;
        }
        public async Task<StoredCredential> SearchUser(string userEmail)
        {
            var context = _contextFactory.CreateDbContext();
            var searchUserEmail = await context.StoredCredentials
                .FirstOrDefaultAsync(x => x.UserEmail.Equals(userEmail));
            return searchUserEmail;
        }

        public async Task<StoredCredential> GetByEmail(string email)
        {
            var context = _contextFactory.CreateDbContext();
            var emailRetrieved = await context.StoredCredentials.FirstOrDefaultAsync(u => u.UserEmail == email);
            if (emailRetrieved == null) return null;
            return emailRetrieved;
        }
        public async Task<StoredCredential> GetPasswordResetToken(string passwordResetToken, DateTime passwordResetTokenExpiry)
        {
            var context = _contextFactory.CreateDbContext();
            var passwordResetTokenRetrieve = await context.StoredCredentials
                .FirstOrDefaultAsync(x => x.PasswordResetToken == passwordResetToken
                && x.PasswordResetTokenExpiry > DateTime.UtcNow);
            return passwordResetTokenRetrieve;
        }
    }
}
