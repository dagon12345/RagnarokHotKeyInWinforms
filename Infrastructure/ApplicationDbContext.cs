using Domain.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<BaseTable> BaseTables { get; set; }
        public DbSet<StoredCredential> StoredCredentials { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    }

}
