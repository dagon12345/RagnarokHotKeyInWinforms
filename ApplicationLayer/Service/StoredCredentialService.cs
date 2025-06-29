using ApplicationLayer.Interface;
using Domain.ErrorMessages;
using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using Infrastructure.Service;
using System;
using System.Threading;
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
            try
            {
                var findCredential = await _storedCredentialRepository.FindUserCredential(accessToken);
                return findCredential;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                return null;
            }

        }

        public async Task SaveCredentials(StoredCredential credential)
        {
            try
            {
                _storedCredentialRepository.Add(credential);
                await _storedCredentialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
            }

        }

        public async Task<StoredCredential> SearchUser(string userEmail)
        {
            try
            {
                var searchUserEmail = await _storedCredentialRepository.SearchUser(userEmail);
                return searchUserEmail;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                return null;
            }

        }

        public async Task SaveChangesAsync(StoredCredential storedCredentials)
        {
            try
            {
                _storedCredentialRepository.Update(storedCredentials);
                await _storedCredentialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
            }

        }

    }
}
