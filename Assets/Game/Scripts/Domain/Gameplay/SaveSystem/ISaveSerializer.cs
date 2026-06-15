using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain
{
    public interface ISaveSerializer
    {
        public string Key { get; }
        
        public JToken Serialize();
        
        public void Deserialize(JToken data);
    }
}