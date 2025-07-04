using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Helpers
{
    public static class SecureConfig
    {
        public static void SaveEncrypted<T>(string path, T config)
        {
            var json = JsonConvert.SerializeObject(config);
            var encrypted = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(json),
                null,
                DataProtectionScope.CurrentUser);

            File.WriteAllBytes(path, encrypted);
        }

        public static T LoadEncrypted<T>(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Encrypted config not found.");

            var encrypted = File.ReadAllBytes(path);
            var decrypted = ProtectedData.Unprotect(
                encrypted,
                null,
                DataProtectionScope.CurrentUser);

            var json = Encoding.UTF8.GetString(decrypted);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

}
