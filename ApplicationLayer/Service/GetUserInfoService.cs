using ApplicationLayer.Interface;
using Domain.Constants;
using Domain.ErrorMessages;
using Domain.Model;
using Infrastructure.Service;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    internal class GetUserInfoService : IGetUserInfo
    {
        public async Task<UserInfo> GetUserInfo(string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                try
                { 
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var response = await httpClient.GetStringAsync(GoogleConstants.GoogleInfoUrl);
                    return JsonConvert.DeserializeObject<UserInfo>(response);
                }
                catch (Exception ex)
                {

                    LoggerService.LogError(ex, $"{ErrorCodes.ProcessFailed}");
                    return null;
                }
            }
        }
    }
}
