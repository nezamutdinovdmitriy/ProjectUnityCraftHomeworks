using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ProductionOrder : MonoBehaviour, ISerializableComponent
    {
        ///Variable
        [SerializeField]
        private List<EntityConfig> _queue;

        private EntityCatalog _entityCatalog;

        [Inject]
        public void Construct(EntityCatalog entityCatalog) 
            => _entityCatalog = entityCatalog;

        public IReadOnlyList<EntityConfig> Queue
        {
            get { return _queue; }
            set { _queue = new List<EntityConfig>(value); }
        }

        public JToken Serialize(IComponentSerializer serializer)
            => serializer.Serialize(this);

        public void Deserialize(IComponentSerializer serializer, JToken token)
            => serializer.Deserialize(this, token);
    }
}