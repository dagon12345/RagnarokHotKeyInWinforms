using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.Model.DataModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Infrastructure.Repositories.Interface;
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
            _baseTableRepository.Add(baseTable);
            await _baseTableRepository.SaveChangesAsync();
        }

        public async Task<UserCredential> GoogleAlgorithm(string googleApisFolder)
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

        public async Task<string> SearchExistingUser(string email)
        {
            var searchedUser = await _baseTableRepository.SearchUsers(email);
            if (searchedUser == null)
            {
                return null;
            }
            var userEmail = searchedUser.Email;
            return userEmail;
        }

    }
}
