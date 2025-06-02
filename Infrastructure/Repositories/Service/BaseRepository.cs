using Infrastructure.Repositories.Interface;
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
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CaptureAndSaveTable<T>(T table) where T : class
        {
            _context.Set<T>().Add(table);
            await _context.SaveChangesAsync();
        }
    }
}
