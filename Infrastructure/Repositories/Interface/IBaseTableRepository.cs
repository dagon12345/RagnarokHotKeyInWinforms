using Domain.Model.DataModels;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interface
{
    public interface IBaseTableRepository
    {
        Task<BaseTable> SearchUsers(string email);
        Task SaveChanges(BaseTable table);
    }
}
