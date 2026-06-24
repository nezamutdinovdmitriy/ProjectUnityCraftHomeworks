using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Game.Scripts.Domain.Encrypt;
using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain.Repositories
{
    public class EncryptedRepository : IRepository
    {
        private const string DataKey = "data";
        
        private readonly IRepository _wrappedRepository;
        private readonly IEncryptor _encryptor;

        public EncryptedRepository(IRepository wrappedRepository, IEncryptor encryptor)
        {
            _wrappedRepository = wrappedRepository;
            _encryptor = encryptor;
        }

        public async UniTask<bool> Save(string version, JObject saveData)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(
                saveData.ToString(Newtonsoft.Json.Formatting.None));

            byte[] encryptedBytes = _encryptor.Encrypt(plainBytes);

            JObject encryptedPayload = new JObject();
            
            if (encryptedPayload == null)
                throw new ArgumentNullException(nameof(encryptedPayload));
            
            encryptedPayload[DataKey] = Convert.ToBase64String(encryptedBytes);

            return await _wrappedRepository.Save(version, encryptedPayload);
        }

        public async UniTask<(bool, JObject)> Load(string version)
        {
            (bool success, JObject encryptedPayload) = await _wrappedRepository.Load(version);

            if (success == false || encryptedPayload == null)
                return (false, null);

            string base64String = encryptedPayload[DataKey]?.ToString();

            if (string.IsNullOrEmpty(base64String))
                return (false, null);

            byte[] encryptedBytes = Convert.FromBase64String(base64String);
            byte[] decryptedBytes = _encryptor.Decrypt(encryptedBytes);

            string jsonText = Encoding.UTF8.GetString(decryptedBytes);

            JObject saveData = JObject.Parse(jsonText);

            return (true, saveData);
        }
    }
}