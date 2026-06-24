using Newtonsoft.Json.Linq;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : MonoBehaviour, ISerializableComponent
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }

        public JToken Serialize(IComponentSerializer serializer)
            => serializer.Serialize(this);

        public void Deserialize(IComponentSerializer serializer, JToken token)
            => serializer.Deserialize(this, token);
    }
}