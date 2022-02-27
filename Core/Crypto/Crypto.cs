using System.Security.Cryptography;

namespace Core.Crypto;

public static class Crypto
{
    public static byte[] GetSalt() => RandomNumberGenerator.GetBytes(32);
    
    public static string GetMasterPasswordHash(string password, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 115000, HashAlgorithmName.SHA512);

        return Convert.ToBase64String(pbkdf2.GetBytes(64));
    }
}