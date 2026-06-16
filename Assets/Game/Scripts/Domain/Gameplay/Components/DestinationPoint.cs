using Game.Scripts.Domain.Serializers;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class DestinationPoint : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public Vector3 Value { get; set; }
        
        public JToken Serialize() => JToken.FromObject((SerializedVector3)Value);

        public void Deserialize(JToken saveData) => Value = saveData.ToObject<SerializedVector3>();
    }
}