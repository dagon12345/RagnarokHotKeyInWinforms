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
}
