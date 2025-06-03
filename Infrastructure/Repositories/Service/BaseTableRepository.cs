using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class BaseTableRepository: BaseRepository, IBaseTableRepository
    {
        public BaseTableRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<BaseTable> SearchUsers(string email)
        {
           var searchUser = await _context.BaseTables
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.Equals(email));
           return searchUser;
        }
    }
}
