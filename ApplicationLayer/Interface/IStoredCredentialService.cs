using Domain.Model.DataModels;
using System;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IStoredCredentialService
    {
        Task SaveChangesAsync();
        Task<StoredCredential> FindCredential(string accessToken);
        Task<StoredCredential> SearchUser(string userEmail);
        Task SaveCredentials(StoredCredential credential);
    }
}
