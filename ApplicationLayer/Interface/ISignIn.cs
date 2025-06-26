using Domain.Model.DataModels;
using Google.Apis.Auth.OAuth2;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface ISignIn
    {
        Task<string> SearchExistingUser(string ReferenceCode);
        Task CreateUser(BaseTable baseTable);
        Task<UserCredential> GoogleAlgorithm(string googleApisFolder);
    }
}
