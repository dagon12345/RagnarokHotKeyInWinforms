using System.ComponentModel.DataAnnotations;

namespace Domain.Constants
{
    public class RagnarokConstants
    {
        [Display(Name = "Supported Server")]
        public const string SupportedServerJson = "supported_servers.json";
        [Display(Name = "Default Profile")]
        public const string DefaultJson = "Default.json";
    }
    public class GoogleConstants
    {
        [Display(Name = "Client Id")]
        public const string ClientId = "451216570289-k7khe78035l9d7bd8c3t1oe2pdifhpe4.apps.googleusercontent.com"; // Replace with your Client ID
        [Display(Name = "Client Secret")]
        public const string ClientSecret = "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"; // Replace with your Client Secret
        [Display(Name = "File Name")]
        public const string TokenFileName = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user"; // Adjust the file name if needed
        [Display(Name = "Google Auth Folder")]
        public const string GoogleApis = "Google.Apis.Auth";
        [Display(Name = "Google API Url")]
        public const string GoogleInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";
    }
}
