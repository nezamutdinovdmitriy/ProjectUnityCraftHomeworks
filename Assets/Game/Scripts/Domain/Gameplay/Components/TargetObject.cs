using Game.Scripts.Domain.Serializers;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class TargetObject : MonoBehaviour, IComponentSavable
    {
        private const int EmptyTarget = -1;
        
        ///Variable
        [field: SerializeField]
        public Entity Value { get; set; }

        private EntityWorld _entityWorld;

        [Inject]
        public void Construct(EntityWorld entityWorld) 
            => _entityWorld = entityWorld;

        public JToken Serialize()
        {
            int targetId = Value != null ? Value.Id : EmptyTarget;

            return JToken.FromObject(targetId);
        }

        public void Deserialize(JToken saveData)
        {
            int targetId = saveData.ToObject<int>();

            if (targetId == EmptyTarget)
            {
                Value = null;
                return;
            }

            if (_entityWorld.TryGet(targetId, out Entity entity))
                Value = entity;
        }
    }
}