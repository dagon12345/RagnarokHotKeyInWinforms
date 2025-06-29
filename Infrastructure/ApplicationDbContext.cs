using Domain.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<BaseTable> BaseTables { get; set; }
        public DbSet<StoredCredential> StoredCredentials { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
        }

    }

}
