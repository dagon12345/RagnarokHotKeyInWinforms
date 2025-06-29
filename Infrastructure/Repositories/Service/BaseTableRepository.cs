using Domain.Model.DataModels;
using Infrastructure.Factory;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class BaseTableRepository: BaseRepository, IBaseTableRepository
    {
        protected readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public BaseTableRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<BaseTable> SearchUsers(string email)
        {
            //We use this for garbagecollector
            var context = _contextFactory.CreateDbContext();

            var searchUser = await context.BaseTables
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.Equals(email));
           return searchUser;
        }
    }
}
