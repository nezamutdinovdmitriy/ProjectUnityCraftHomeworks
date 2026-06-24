using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain
{
    public interface IRepository
    {
        public UniTask<bool> Save(string version, JObject saveData);
        public UniTask<(bool, JObject)> Load(string version);
    }
}