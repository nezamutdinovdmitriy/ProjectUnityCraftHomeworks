using System.Collections.Generic;
using Modules.Entities;
using UnityEngine;
using Zenject;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ProductionOrder : MonoBehaviour, IComponentSavable
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

        public void Accept(IComponentVisitor visitor) => visitor.Visit(this);
    }
}