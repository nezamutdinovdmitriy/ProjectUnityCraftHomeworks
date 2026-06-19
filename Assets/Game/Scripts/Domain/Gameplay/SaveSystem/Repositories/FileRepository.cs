using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.Repositories
{
    public class FileRepository : IRepository
    {
        private readonly string _path;

        public FileRepository(string path) => _path = path;

        public async UniTask<bool> Save(string version, JObject saveData)
        {
            string path = GetFullPathWithVersion(version);
            string directory = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(directory) == false
                && Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);
            
            string json = saveData.ToString(Newtonsoft.Json.Formatting.Indented);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            
            await File.WriteAllBytesAsync(path, bytes);
            
            return true;
        }

        public async UniTask<(bool, JObject)> Load(string version)
        {
            string path = GetFullPathWithVersion(version);
            
            if (File.Exists(path) == false)
                return (false, null);

            byte[] bytes = await File.ReadAllBytesAsync(path);
            string json = Encoding.UTF8.GetString(bytes);
            
            return (true, JObject.Parse(json));
        }

        private string GetFullPathWithVersion(string version) 
            => Path.Combine(_path, $"save_{version}.json");
    }
}