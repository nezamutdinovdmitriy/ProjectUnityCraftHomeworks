using System.IO;
using System.Security.Cryptography;

namespace Game.Scripts.Domain.Encrypt
{
    public class AesEncryptionService : IEncryptor
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        
        public AesEncryptionService(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
        }

        public byte[] Encrypt(byte[] plainBytes)
        {
            if (plainBytes == null || plainBytes.Length == 0) 
                return plainBytes;

            using Aes aes = Aes.Create();
            
            aes.Key = _key;
            aes.IV = _iv;

            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(
                memoryStream, 
                aes.CreateEncryptor(), 
                CryptoStreamMode.Write);
            
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            
            cryptoStream.FlushFinalBlock();

            return memoryStream.ToArray();
        }

        public byte[] Decrypt(byte[] cipherBytes)
        {
            if (cipherBytes == null || cipherBytes.Length == 0) 
                return cipherBytes;

            using Aes aes = Aes.Create();
            
            aes.Key = _key;
            aes.IV = _iv;

            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(
                memoryStream, 
                aes.CreateDecryptor(), 
                CryptoStreamMode.Write);
            
            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
            
            cryptoStream.FlushFinalBlock();

            return memoryStream.ToArray();
        }
    }
}