using Domain.Model.DataModels;
using System;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IStoredCredentialService
    {
        Task SaveChanges();
        Task<StoredCredential> FindCredential(string accessToken);
        Task<StoredCredential> SearchUser(string userEmail);
        Task SaveCredentials(StoredCredential credential, string accessToken,
            DateTimeOffset lastTimeLogin, string Name, string Email);
    }
}
