using Infrastructure.Helpers;
using Infrastructure.Repositories.Interface;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public class LoginService
    {
        private readonly IStoredCredentialRepository _credentialRepository;
        public LoginService(IStoredCredentialRepository storedCredentialRepository)
        {
            _credentialRepository = storedCredentialRepository;
        }
        public async Task<bool> Login(string email, string password)
        {
            var user = await _credentialRepository.GetByEmail(email);
            if (user == null || !user.IsEmailConfirmed) 
                return false;

            var hash = SecurityHelper.HashPassword(password, user.Salt);
            return hash == user.PasswordHash;
        }
    }
}
