using Domain.Model.DataModels;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interface
{
    public interface IUserSettingRepository
    {
        void Add<T>(T table) where T : class;
        void Update<T>(T table) where T : class;
        Task SaveChangesAsync();
        Task<UserSettings> FindUserReferenceCode(Guid referenceCode);
        Task<UserSettings> SelectUserPreference(Guid referenceCode);
    }
}
