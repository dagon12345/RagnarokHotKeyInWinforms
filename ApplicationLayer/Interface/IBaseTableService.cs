using Domain.Model.DataModels;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IBaseTableService
    {
        Task<BaseTable> SearchUser(string email);
    }
}
