using Infrastructure.Factory;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class BaseRepository
    {
        protected readonly ApplicationDbContext _context;
        public BaseRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Update<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First();
                var keyValue = key.PropertyInfo.GetValue(entity);

                var trackedEntity = _context.ChangeTracker.Entries<T>()
                    .FirstOrDefault(e => key.PropertyInfo.GetValue(e.Entity).Equals(keyValue));

                if (trackedEntity != null)
                {
                    // Update the tracked entity's values
                    _context.Entry(trackedEntity.Entity).CurrentValues.SetValues(entity);
                }
                else
                {
                    _context.Attach(entity);
                    entry.State = EntityState.Modified;

                }
            }
        }
        public void Add<T>(T table) where T : class
        {
            _context.Set<T>().Add(table);
        }

    }
}
