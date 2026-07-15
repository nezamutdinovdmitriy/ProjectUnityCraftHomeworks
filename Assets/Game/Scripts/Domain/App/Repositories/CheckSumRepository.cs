using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Game.Scripts.Domain.App.Hash;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.Repositories
{
    public class CheckSumRepository : IRepository
    {
        private const string HashKey = "Hash";

        private readonly IRepository _wrappedRepository;
        private readonly IHashProvider _hashProvider;

        public CheckSumRepository(IRepository wrappedRepository, IHashProvider hashProvider)
        {
            _wrappedRepository = wrappedRepository;
            _hashProvider = hashProvider;
        }

        public async UniTask<bool> Save(string version, JObject saveData)
        {
            saveData.Remove(HashKey);

            string rawJson = saveData.ToString(Newtonsoft.Json.Formatting.Indented);
            byte[] rawDataBytes = Encoding.UTF8.GetBytes(rawJson);

            byte[] computedHashBytes = _hashProvider.GetHash(rawDataBytes);
            saveData[HashKey] = Convert.ToBase64String(computedHashBytes);

            return await _wrappedRepository.Save(version, saveData);
        }

        public async UniTask<(bool, JObject)> Load(string version)
        {
            (bool success, JObject loadedData) = await _wrappedRepository.Load(version);

            if (success == false 
                || loadedData == null 
                || loadedData.TryGetValue(HashKey, out JToken hashToken) == false)
                return (false, null);

            string fileHashBase64 = hashToken.ToString();
            byte[] fileHashBytes = Convert.FromBase64String(fileHashBase64);

            loadedData.Remove(HashKey);
            
            string rawJson = loadedData.ToString(Newtonsoft.Json.Formatting.Indented);
            byte[] rawDataBytes = Encoding.UTF8.GetBytes(rawJson);

            Debug.Log($"RAW = FILE : {_hashProvider.VerifyHash(rawDataBytes, fileHashBytes)}");
            
            if (_hashProvider.VerifyHash(rawDataBytes, fileHashBytes))
            {
                Debug.Log($"[{_wrappedRepository.GetType().Name}] Data verified successfully.");
                return (true, loadedData);
            }
            
            return (false, null);
        }
    }
}