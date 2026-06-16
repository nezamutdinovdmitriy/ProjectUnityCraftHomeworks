using Game.Scripts.Domain.Serializers;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }
        
        public JToken Serialize() => JToken.FromObject(Type);

        public void Deserialize(JToken saveData) => Type = saveData.ToObject<TeamType>();
    }
}