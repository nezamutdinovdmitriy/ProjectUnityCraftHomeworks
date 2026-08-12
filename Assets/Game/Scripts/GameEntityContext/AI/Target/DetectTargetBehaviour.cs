using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class DetectTargetBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private readonly GameEntity[] _enemies;
        
        private TriggerEvents _triggerEvents;
        private IGameEntity _currentTarget;
        
        public DetectTargetBehaviour(GameEntity[] enemies) 
            => _enemies = enemies;

        public void Init(IGameEntity entity)
        {
            _triggerEvents = entity.GetValue(GameEntityAPI.Trigger);
            
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
            if (obj.TryGetComponent(out IGameEntity entity)
                && entity.HasTag(GameEntityAPI.CharacterTag))
            {
                _currentTarget = entity;
                TargetUseCase.SetTarget(_enemies, _currentTarget);
            }
        }
        
        private void OnTriggerExit(Collider obj)
        {
            if (obj == null)
                return;

            if (obj.TryGetComponent(out IGameEntity entity)
                && _currentTarget.Equals(entity))
            {
                _currentTarget = null;
                TargetUseCase.SetTarget(_enemies, null);
            }
        }
    }
}