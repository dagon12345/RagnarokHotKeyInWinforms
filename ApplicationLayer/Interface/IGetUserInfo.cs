using Domain.Model;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IGetUserInfo
    {
        Task<UserInfo> GetUserInfo(string accessToken);
    }
}
