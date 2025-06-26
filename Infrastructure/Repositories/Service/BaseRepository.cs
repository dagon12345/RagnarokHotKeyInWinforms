using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class BaseRepository
    {
        protected readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Add<T>(T table) where T : class
        {
            _context.Set<T>().Add(table);
        }

    }
}
