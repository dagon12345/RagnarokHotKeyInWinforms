using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class BaseTableRepositoryService : IBaseTableRepository
    {
        ApplicationDbContext _context;
        public BaseTableRepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveChanges(BaseTable table)
        {
            _context.BaseTables.Add(table);
            await _context.SaveChangesAsync();
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
