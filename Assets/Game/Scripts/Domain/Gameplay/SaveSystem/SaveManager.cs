using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain
{
    public class SaveManager
    {
        private const string SaveVersionKey = "SaveVersion";
        
        private readonly ISaveSerializer[] _serializers;
        private readonly IRepository _repository;

        private int _version;

        public SaveManager(
            ISaveSerializer[] serializers,
            IRepository repository)
        {
            _serializers = serializers;
            _repository = repository;

            _version = PlayerPrefs.GetInt(SaveVersionKey, 0);
        }

        public async UniTask<(bool success, int version)> SaveAsync()
        {
            JObject saveData = new();

            int nextVersion = _version + 1;

            foreach (ISaveSerializer serializer in _serializers)
                saveData.Add(serializer.Key, serializer.Serialize());

            bool success = await _repository.Save(nextVersion.ToString(), saveData);

            if (success)
            {
                _version = nextVersion;
                
                PlayerPrefs.SetInt(SaveVersionKey, _version);
                PlayerPrefs.Save();
            }

            return (success, nextVersion);
        }

        public async UniTask<(bool success, int version)> LoadAsync(string version)
        {
            int parsedVersion = int.TryParse(version, out var result) ? result : -1;
            
            if (parsedVersion < 0 
                || parsedVersion > PlayerPrefs.GetInt(SaveVersionKey, 0))
                return (false, parsedVersion);
            
            (bool success, JObject saveData) = await _repository.Load(version);
            
            if (success)
                foreach (ISaveSerializer serializer in _serializers)
                    if (saveData.TryGetValue(serializer.Key, out JToken data))
                        serializer.Deserialize(data);
            
            return (success, parsedVersion);
        }
    }
}