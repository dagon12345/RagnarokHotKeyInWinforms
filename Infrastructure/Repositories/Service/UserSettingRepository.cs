using Domain.Model.DataModels;
using Infrastructure.Repositories.Interface;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Repositories.Service
{
    public class UserSettingRepository : BaseRepository, IUserSettingRepository
    {
        public UserSettingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UserSettings> FindUserReferenceCode(Guid referenceCode)
        {
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
            var selectUserPreference = await _context.UserSettings
                .Where(x => x.ReferenceCode.Equals(referenceCode))
                .FirstOrDefaultAsync();
            if (referenceCode == null)
            {
                return null;
            }
            return selectUserPreference;
        }
    }
}
