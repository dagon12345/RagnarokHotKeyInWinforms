using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Factory
{
    public interface IDbContextFactory<TContext> where TContext : DbContext
    {
        TContext CreateDbContext();
    }
}
