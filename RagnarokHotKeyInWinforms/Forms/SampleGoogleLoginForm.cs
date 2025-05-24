using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class SampleGoogleLoginForm : Form
    {
        public const string ClientId = "451216570289-vq70l4q1jeis4igljsr9ila3i4bq2mrt.apps.googleusercontent.com";
        public const string ClientSecret = "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";

        public SampleGoogleLoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, System.EventArgs e)
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                },
                new[] { "email", "profile" },
                "user",
                CancellationToken.None,
                new FileDataStore("Google.Apis.Auth"));

            var userInfo = await GetUserInfo(credential.Token.AccessToken);

            MessageBox.Show($"Welcome, {userInfo.Name}!");
        }

        private async Task<UserInfo> GetUserInfo(string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetStringAsync("https://www.googleapis.com/userinfo/v2/me");
                return JsonConvert.DeserializeObject<UserInfo>(response);
            }
        }

        public class UserInfo
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}
