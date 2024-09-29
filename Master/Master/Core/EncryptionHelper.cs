using System.Security.Cryptography;
using System.Text;

namespace Master.Core
{

    public class EncryptionHelper
    {
        private readonly byte[] _key;

        public EncryptionHelper(byte[] key)
        {
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            {
                throw new ArgumentException("Key size must be 16, 24, or 32 bytes.");
            }
            _key = key;
        }

        public string Encrypt(string plainText)
        {
            using (var aes = new AesGcm(_key))
            {
                byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize]; 
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherText = new byte[plaintextBytes.Length];
                byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize]; 

                RandomNumberGenerator.Fill(nonce);

                aes.Encrypt(nonce, plaintextBytes, cipherText, tag);

                byte[] encryptedData = new byte[nonce.Length + tag.Length + cipherText.Length];
                nonce.CopyTo(encryptedData, 0);
                tag.CopyTo(encryptedData, nonce.Length);
                cipherText.CopyTo(encryptedData, nonce.Length + tag.Length);

                return Convert.ToBase64String(encryptedData);
            }
        }

        public string Decrypt(string cipherText)
        {
            byte[] encryptedData = Convert.FromBase64String(cipherText);

            byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
            byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];
            byte[] ciphertext = new byte[encryptedData.Length - nonce.Length - tag.Length];

            Array.Copy(encryptedData, 0, nonce, 0, nonce.Length);
            Array.Copy(encryptedData, nonce.Length, tag, 0, tag.Length);
            Array.Copy(encryptedData, nonce.Length + tag.Length, ciphertext, 0, ciphertext.Length);

            using (var aes = new AesGcm(_key))
            {
                byte[] plaintextBytes = new byte[ciphertext.Length];

                aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);

                return Encoding.UTF8.GetString(plaintextBytes);
            }
        }
    }
}
