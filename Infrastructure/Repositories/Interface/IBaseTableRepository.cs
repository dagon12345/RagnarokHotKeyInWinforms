using Domain.Model.DataModels;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interface
{
    public interface IBaseTableRepository
    {
        void Add<T>(T table) where T : class;
        Task<BaseTable> SearchUsers(string email);
        Task SaveChangesAsync();
    }
}
