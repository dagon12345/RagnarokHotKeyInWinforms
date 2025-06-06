using ApplicationLayer.Interface;
using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
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
            var baseTable = await _baseTableRepository.SearchUsers(email);
            return baseTable;
        }
    }
}
