using Game.Scripts.Domain.Serializers;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Health : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;
        
        public JToken Serialize() => JToken.FromObject(Current);

        public void Deserialize(JToken saveData) => Current = saveData.ToObject<int>();
    }
}