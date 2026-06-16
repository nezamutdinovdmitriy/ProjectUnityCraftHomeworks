using Game.Scripts.Domain.Serializers;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ResourceBag : MonoBehaviour, IComponentSavable
    {
        private struct SaveData
        {
            public ResourceType Type;
            public int Current;
        }
        
        ///Variable
        [field: SerializeField]
        public ResourceType Type { get; set; }
        
        ///Variable
        [field: SerializeField]
        public int Current { get; set; }
        
        ///Const
        [field: SerializeField]
        public int Capacity { get; set; }
        
        public JToken Serialize()
        {
            return JToken.FromObject(new SaveData()
            {
                Type = this.Type,
                Current = this.Current
            });
        }

        public void Deserialize(JToken saveData)
        {
            SaveData data = saveData.ToObject<SaveData>();

            Type = data.Type;
            Current = data.Current;
        }
    }
}