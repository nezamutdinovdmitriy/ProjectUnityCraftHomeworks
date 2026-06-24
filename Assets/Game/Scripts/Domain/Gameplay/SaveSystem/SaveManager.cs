using Cysharp.Threading.Tasks;
using Game.Scripts.Domain.App;
using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain
{
    public class SaveManager
    {
        private readonly ISaveSerializer[] _serializers;
        private readonly IRepository _repository;
        private readonly IVersionProvider _versionProvider;

        public SaveManager(
            ISaveSerializer[] serializers,
            IRepository repository, 
            IVersionProvider versionProvider)
        {
            _serializers = serializers;
            _repository = repository;
            _versionProvider = versionProvider;
        }

        public async UniTask<(bool success, int version)> SaveAsync()
        {
            JObject saveData = new();

            foreach (ISaveSerializer serializer in _serializers)
                saveData.Add(
                    serializer.Key, 
                    serializer.Serialize());

            bool success = await _repository.Save(
                _versionProvider.GetNextVersion().ToString(),
                saveData);

            if (success)
                _versionProvider.IncreaseVersion();

            return (success, _versionProvider.GetCurrentVersion());
        }

        public async UniTask<(bool success, int version)> LoadAsync(string version)
        {
            int parsedVersion = int.TryParse(version, out var result) ? result : -1;
            
            if (_versionProvider.IsVersionValid(parsedVersion) == false)
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