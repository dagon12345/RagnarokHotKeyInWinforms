using ApplicationLayer.Interface;
using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using System;
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

        public async Task CreateUser(BaseTable baseTable, string email)
        {
            baseTable.ReferenceCode = Guid.NewGuid();
            baseTable.Email = email;

            await _baseTableRepository.SaveChanges(baseTable);
        }

        public async Task<string> SearchExistingUser(string email)
        {
            var searchedUser = await _baseTableRepository.SearchUsers(email);
            if(searchedUser == null)
            {
                return null;
            }
            var userEmail = searchedUser.Email;
            return userEmail;
        }

    }
}
