using ApplicationLayer.Interface;
using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using System;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public class StoredCredentialService : IStoredCredentialService
    {
        private readonly IStoredCredentialRepository _storedCredentialRepository;
        public StoredCredentialService(IStoredCredentialRepository storedCredentialRepository)
        {
            _storedCredentialRepository = storedCredentialRepository;
        }
        public async Task<StoredCredential> FindCredential(string accessToken)
        {
            var findCredential = await _storedCredentialRepository.FindUserCredential(accessToken);
            return findCredential;
        }

        public async Task SaveCredentials(StoredCredential credential, string accessToken, DateTimeOffset lastTimeLogin, string Name, string Email)
        {
            credential.Name = Name;
            credential.AccessToken = accessToken;
            credential.LastLoginTime = lastTimeLogin;
            credential.UserEmail = Email;
            _storedCredentialRepository.Add(credential);
            await _storedCredentialRepository.SaveChangesAsync();
        }

        public async Task<StoredCredential> SearchUser(string userEmail)
        {
            var searchUserEmail = await _storedCredentialRepository.SearchUser(userEmail);
            return searchUserEmail;
        }

        public async Task SaveChangesAsync()
        {
            await _storedCredentialRepository.SaveChangesAsync();
        }
    }
}
