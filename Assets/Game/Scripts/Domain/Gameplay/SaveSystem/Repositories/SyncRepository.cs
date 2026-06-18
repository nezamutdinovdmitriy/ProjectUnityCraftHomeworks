using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Game.Scripts.Domain.Repositories
{
    public class SyncRepository : IRepository
    {
        private const string SaveTimeKey = "SaveTime";
        
        private readonly IRepository[] _repositories;

        public SyncRepository(params IRepository[] repositories) 
            => _repositories = repositories;
        
        public async UniTask<bool> Save(string version, JObject saveData)
        {
            int repositoriesCount = _repositories.Length;
            saveData[SaveTimeKey] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (repositoriesCount == 0)
                return false;

            UniTask<bool>[] saveTasks = new UniTask<bool>[repositoriesCount];
            
            for (int i = 0; i < repositoriesCount; i++)
                saveTasks[i] = _repositories[i].Save(version, saveData);

            bool[] results = await UniTask.WhenAll(saveTasks);

            return results.Any(success => success);
        }

        public async UniTask<(bool, JObject)> Load(string version)
        {
            int repositoriesCount = _repositories.Length;
            
            if (repositoriesCount == 0)
                return (false, null);
            
            UniTask<(bool, JObject)>[] loadTasks = new UniTask<(bool, JObject)>[repositoriesCount];

            for (int i = 0; i < repositoriesCount; i++)
                loadTasks[i] = _repositories[i].Load(version);

            (bool, JObject)[] results = await UniTask.WhenAll(loadTasks);

            long lastTimestamp = long.MinValue;
            JObject lastSaveData = null;

            for (int i = 0; i < repositoriesCount; i++)
            {
                (bool success, JObject saveData) = results[i];

                if (success == false || saveData == null)
                    continue;

                long timestamp = 0;

                if (saveData.TryGetValue(SaveTimeKey, out JToken token))
                    timestamp = token.Value<long>();
                
                if (timestamp > lastTimestamp)
                {
                    lastTimestamp = timestamp;
                    lastSaveData = saveData;
                }
            }

            return lastSaveData != null ? (true, lastSaveData) : (false, null);
        }
    }
}