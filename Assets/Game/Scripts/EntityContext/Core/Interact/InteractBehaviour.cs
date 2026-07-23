using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext.Core.Interact
{
    public class InteractBehaviour : IEntityContextInit, IEntityContextDispose
    {
        private TriggerEvents _triggerEvents;
        private IEntityContext _interactor;
        
        public void Init(IEntityContext entity)
        {
            _interactor = entity;
            _triggerEvents = entity.GetValue(EntityContextAPI.Trigger);

            _triggerEvents.OnEntered += OnTriggerEntered;
        }

        public void Dispose(IEntityContext entity) 
            => _triggerEvents.OnEntered -= OnTriggerEntered;

        private void OnTriggerEntered(Collider collider)
        {
            if (collider.TryGetComponent(out IEntityContext interactable))
                _interactor.Interact(interactable);
        }
    }
}