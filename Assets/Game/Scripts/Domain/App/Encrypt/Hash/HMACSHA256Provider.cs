using System.Security.Cryptography;
using System.Text;

namespace Game.Scripts.Domain.App.Hash
{
    public class HMACSHA256Provider : IHashProvider
    {
        private readonly byte[] _key;

        public HMACSHA256Provider(string key)
        {
            _key = Encoding.UTF8.GetBytes(key);
        }

        public byte[] GetHash(byte[] input)
        {
            using HMACSHA256 hmac = new(_key);
            return hmac.ComputeHash(input);
        }

        public bool VerifyHash(byte[] inputData, byte[] trueHash)
        {
            byte[] hash = GetHash(inputData);

            return CryptographicOperations.FixedTimeEquals(hash, trueHash);
        }
    }
}