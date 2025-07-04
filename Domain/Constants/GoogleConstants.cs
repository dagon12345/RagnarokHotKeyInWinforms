using System.ComponentModel.DataAnnotations;

namespace Domain.Constants
{
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

        //Send grid constants
        public const string SendGridAPI = "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";
        public const string GoogleEmail = "lanceespina478@gmail.com";
        public const string AppName = "RagnarokHotkey";
    }

    public class YahooConstants
    {
        public const string YahooMail = "lanceandreiespina@yahoo.com";
        public const string YahooAppPassword = "gqgbcpkaugdhnyux";
        public const string YahooSmtp = "smtp.mail.yahoo.com";
    }
}
