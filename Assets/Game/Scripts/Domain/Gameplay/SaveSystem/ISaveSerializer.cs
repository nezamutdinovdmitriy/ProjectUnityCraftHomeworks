using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain
{
    public interface ISaveSerializer
    {
        public string Key { get; }
        
        public JToken Serialize();
        
        public void Deserialize(JToken data);
    }

    public interface ISaveSerializer<T> : ISaveSerializer
    {
        JToken ISaveSerializer.Serialize() => JToken.FromObject(this.Serialize());
        
        void ISaveSerializer.Deserialize(JToken data) => this.Deserialize(data.ToObject<T>());
        
        string ISaveSerializer.Key => typeof(T).Name;
        
        public new T Serialize();
        
        public void Deserialize(T data);
    }
}