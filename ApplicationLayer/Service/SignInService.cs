using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.ErrorMessages;
using Domain.Model.DataModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Infrastructure.Repositories.Interface;
using Infrastructure.Service;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public class SignInService : ISignIn
    {
        private readonly IBaseTableRepository _baseTableRepository;
        public SignInService(IBaseTableRepository baseTableRepository)
        {
            _baseTableRepository = baseTableRepository;
        }

        public async Task CreateUser(BaseTable baseTable)
        {
            try
            {
                _baseTableRepository.Add(baseTable);
                await _baseTableRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
            }

        }

        public async Task<UserCredential> GoogleAlgorithm(string googleApisFolder)
        {
            try
            {
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = GoogleConstants.ClientId,
                    ClientSecret = GoogleConstants.ClientSecret
                },
                 new[] { "email", "profile" },
                 "user",
                 CancellationToken.None,
                 new FileDataStore(googleApisFolder));

                return credential;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                return null;
            }

        }


        public async Task<string> SearchExistingUser(string email)
        {
            try
            {
                var searchedUser = await _baseTableRepository.SearchUsers(email);
                if (searchedUser == null)
                {
                    return null;
                }
                var userEmail = searchedUser.Email;
                return userEmail;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                return null;
            }


        }

    }
}
