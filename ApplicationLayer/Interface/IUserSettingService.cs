using Domain.Model.DataModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IUserSettingService
    {
        Task UpsertUser(Guid referenceCode, string Name);
        Task<UserSettings> SearchByReferenceCode(Guid referenceCode);
        Task<UserSettings> SelectUserPreference(Guid referenceCode);
        Task SaveChangesAsync();
    }
}
