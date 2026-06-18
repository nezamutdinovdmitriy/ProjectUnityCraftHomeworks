using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Scripts.Domain.Repositories
{
    public class RemoteRepository : IRepository
    {
        private const string ContentType = "Content-Type";
        private const string JsonContentType = "application/json";

        private readonly RemoteRepositoryConfig _config;

        public RemoteRepository(RemoteRepositoryConfig config) => _config = config;
        
        public async UniTask<bool> Save(string version, JObject saveData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(
                saveData.ToString(Newtonsoft.Json.Formatting.Indented));
            
            using UnityWebRequest request = UnityWebRequest.Put(
                _config.GetSavePath(version), 
                bytes);
            
            request.SetRequestHeader(ContentType, JsonContentType);

            await request.SendWebRequest();
            
            return request.result == UnityWebRequest.Result.Success;
        }

        public async UniTask<(bool, JObject)> Load(string version)
        {
            using UnityWebRequest request = UnityWebRequest.Get(
                _config.GetLoadPath(version));
            
            await request.SendWebRequest();

            string jsonText = request.downloadHandler.text;
            
            if (request.result != UnityWebRequest.Result.Success 
                || string.IsNullOrEmpty(jsonText))
                return (false, null);

            JObject saveData = JObject.Parse(request.downloadHandler.text);

            return (true, saveData);
        }
    }
}