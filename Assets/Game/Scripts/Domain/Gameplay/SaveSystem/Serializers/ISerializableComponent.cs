using Newtonsoft.Json.Linq;

namespace SampleGame.Gameplay
{
    public interface ISerializableComponent
    {
        public JToken Serialize(IComponentSerializer serializer);
        public void Deserialize(IComponentSerializer serializer, JToken token);
    }
}