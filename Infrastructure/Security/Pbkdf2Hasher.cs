using Domain.Security;
using System;
using System.Security.Cryptography;

public class Pbkdf2Hasher : IHasher
{
    private const int Iterations = 100000;
    private const int SaltSize = 16; // bytes
    private const int KeySize = 32;  // bytes

    public string Hash(string input)
    {
        byte[] salt = new byte[SaltSize];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        byte[] hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(input, salt, Iterations))
        {
            hash = pbkdf2.GetBytes(KeySize);
        }

        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    public bool Verify(string input, string hashed)
    {
        var parts = hashed.Split('.');
        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] expectedHash = Convert.FromBase64String(parts[1]);

        byte[] actualHash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(input, salt, Iterations))
        {
            actualHash = pbkdf2.GetBytes(KeySize);
        }

        return SlowEquals(expectedHash, actualHash);
    }

    private bool SlowEquals(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        int diff = 0;
        for (int i = 0; i < a.Length; i++)
            diff |= a[i] ^ b[i];

        return diff == 0;
    }
}
