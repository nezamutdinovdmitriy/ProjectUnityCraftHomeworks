using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Domain.Serializers;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ProductionOrder : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [SerializeField]
        private List<EntityConfig> _queue;

        private readonly EntityCatalog _entityCatalog;

        public ProductionOrder(EntityCatalog entityCatalog) 
            => _entityCatalog = entityCatalog;

        public IReadOnlyList<EntityConfig> Queue
        {
            get { return _queue; }
            set { _queue = new List<EntityConfig>(value); }
        }

        public JToken Serialize()
        {
            List<string> configNames = _queue.Select(config => config.Name).ToList();
            return JToken.FromObject(configNames);
        }

        public void Deserialize(JToken saveData)
        {
            _queue.Clear();

            List<string> configName = saveData.ToObject<List<string>>();

            foreach (string name in configName)
                if (_entityCatalog.FindConfig(name, out EntityConfig config))
                    _queue.Add(config);
        }
    }
}