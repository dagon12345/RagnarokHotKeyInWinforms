using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.Factory;

namespace Infrastructure.Repositories.Service
{
    public class UserSettingRepository : BaseRepository, IUserSettingRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public UserSettingRepository(IDbContextFactory<ApplicationDbContext> context) : base(context)
        {
            _contextFactory = context;
        }

        public async Task<UserSettings> FindUserReferenceCode(Guid referenceCode)
        {
            var context = _contextFactory.CreateDbContext();
            var FindReferenceCode = await _context.UserSettings
                .FirstOrDefaultAsync(x => x.ReferenceCode.Equals(referenceCode));
            if(referenceCode == null)
            {
                return null;
            }
            return FindReferenceCode;
        }

        public async Task<UserSettings> SelectUserPreference(Guid referenceCode)
        {
            var context = _contextFactory.CreateDbContext();
            var selectUserPreference = await context.UserSettings
                .Where(x => x.ReferenceCode == referenceCode)
                .FirstOrDefaultAsync();
            if (selectUserPreference == null)
            {
                return null;
            }
            return selectUserPreference;
        }

    }
}
