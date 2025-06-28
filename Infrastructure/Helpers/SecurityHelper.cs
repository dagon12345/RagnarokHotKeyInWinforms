using System;
using System.Security.Cryptography;

namespace Infrastructure.Helpers
{
    public static class SecurityHelper
    {
        public static string GenerateSalt(int size = 32)
        {
            var saltBytes = new byte[size];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        public static string HashPassword(string password, string salt, int iterations = 10000, 
            int hashSize = 32)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using(var pdkdf2 = new Rfc2898DeriveBytes(password, saltBytes, iterations))
            {
                return Convert.ToBase64String(pdkdf2.GetBytes(hashSize));
            }
        }
    }
}
