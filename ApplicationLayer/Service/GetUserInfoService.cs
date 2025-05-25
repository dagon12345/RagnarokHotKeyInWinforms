using ApplicationLayer.Interface;
using Domain.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    internal class GetUserInfoService : IGetUserInfo
    {
        public async Task<UserInfo> GetUserInfo(string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetStringAsync("https://www.googleapis.com/userinfo/v2/me");
                return JsonConvert.DeserializeObject<UserInfo>(response);
            }
        }
    }
}
