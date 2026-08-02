using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity.Core.Target
{
    public class DetectTargetBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private TriggerEvents _triggerEvents;
        private IVariable<IGameEntity> _target;
        
        public void Init(IGameEntity entity)
        {
            _triggerEvents = entity.GetValue(GameEntityAPI.Trigger);
            _target = entity.GetValue(GameEntityAPI.Target);

            _triggerEvents.OnEntered += OnTriggerEntered;
            _triggerEvents.OnExited += OnTriggerExit;
        }

        public void Dispose(IGameEntity entity)
        {
            _triggerEvents.OnEntered -= OnTriggerEntered;
            _triggerEvents.OnExited -= OnTriggerExit;
        }
        
        private void OnTriggerEntered(Collider obj)
        {
            if (obj.TryGetComponent(out IGameEntity target) 
                && target.HasTag(GameEntityAPI.CharacterTag))
                _target.Value = target;
        }
        
        private void OnTriggerExit(Collider obj)
        {
            if (obj == null)
                return;
            
            if (obj.TryGetComponent(out IGameEntity target) 
                && _target.Value.Equals(target))
                _target.Value = null;
        }
    }
}