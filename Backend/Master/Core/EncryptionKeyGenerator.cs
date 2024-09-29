using System.Security.Cryptography;

namespace Master.Core
{
    public static class EncryptionKeyGenerator
    {
        public static byte[] GenerateKey(int size = 32)
        {
            byte[] key = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }
    }
}
