using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zenject;

namespace Game.Scripts.Domain
{
    public class SaveManager
    {
        private readonly ISaveSerializer[] _serializers;
        private readonly IRepository _repository;

        private int _version;

        public SaveManager(
            ISaveSerializer[] serializers,
            IRepository repository)
        {
            _serializers = serializers;
            _repository = repository;
        }

        public async UniTask<(bool success, int version)> SaveAsync()
        {
            JObject saveData = new();

            int nextVersion = _version + 1;

            foreach (ISaveSerializer serializer in _serializers)
                saveData.Add(serializer.Key, serializer.Serialize());

            bool success = await _repository.Save(nextVersion.ToString(), saveData);

            if (success)
                _version = nextVersion;

            return (success, nextVersion);
        }

        public async UniTask<(bool success, int version)> LoadAsync(string version)
        {
            (bool success, JObject saveData) = await _repository.Load(version.ToString());

            if (success)
                foreach (ISaveSerializer serializer in _serializers)
                    if (saveData.TryGetValue(serializer.Key, out JToken data))
                        serializer.Deserialize(data);

            int v = int.TryParse(version, out var result) ? result : -1;

            return (success, v);
        }
    }
}