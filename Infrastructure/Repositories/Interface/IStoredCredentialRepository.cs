using Domain.Model.DataModels;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interface
{
    public interface IStoredCredentialRepository
    {
        Task CaptureAndSaveTable<T>(T table) where T : class;
        Task<StoredCredential> FindUserCredential(string accessToken);
        Task<StoredCredential> SearchUser(string userEmail);
        Task SaveChanges();
    }
}
