using Domain.Model.DataModels;
using FluentResults;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IRegistrationService
    {
        Task<Result> ConfirmEmail(string token, string inputToken);
        Task<StoredCredential> Register(string email, string password, string name);
    }
}
