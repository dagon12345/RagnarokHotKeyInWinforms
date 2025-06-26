using ApplicationLayer.Interface;
using Domain.ErrorMessages;
using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using Infrastructure.Service;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public class BaseTableService : IBaseTableService
    {
        private readonly IBaseTableRepository _baseTableRepository;
        public BaseTableService(IBaseTableRepository baseTableRepository)
        {
            _baseTableRepository = baseTableRepository;
        }
        public async Task<BaseTable> SearchUser(string email)
        {
            try
            {
                var baseTable = await _baseTableRepository.SearchUsers(email);
                return baseTable;
            }
            catch (Exception ex)
            {

                LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                return null;
            }

        }
    }
}
