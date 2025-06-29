using Domain.Model.DataModels;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interface
{
    public interface IStoredCredentialRepository
    {
        void Add<T>(T table) where T : class;
        void Update<T>(T table) where T : class;
        Task SaveChangesAsync();
        Task<StoredCredential> FindUserCredential(string accessToken);
        Task<StoredCredential> SearchUser(string userEmail);

        Task <StoredCredential> GetByEmail(string email);
        Task<StoredCredential> GetPasswordResetToken(string passwordResetToken, DateTime passwordResetTokenExpiry);
    }
}
