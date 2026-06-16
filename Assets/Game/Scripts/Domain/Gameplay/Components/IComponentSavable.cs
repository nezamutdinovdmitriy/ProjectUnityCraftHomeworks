using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain.Serializers
{
    public interface IComponentSavable
    {
        public string Key => this.GetType().Name;
        public JToken Serialize();
        public void Deserialize(JToken saveData);
    }
}