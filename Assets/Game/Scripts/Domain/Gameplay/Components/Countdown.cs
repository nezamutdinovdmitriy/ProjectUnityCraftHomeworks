using Game.Scripts.Domain.Serializers;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Countdown : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public float Current { get; set; }

        ///Const
        [field: SerializeField]
        public float Duration { get; private set; }
        
        public JToken Serialize() => JToken.FromObject(Current);

        public void Deserialize(JToken saveData) => Current = saveData.ToObject<float>();
    }
}