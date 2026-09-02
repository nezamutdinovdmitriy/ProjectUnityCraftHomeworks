using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class EnemyTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameEntity[] _enemies;

        private IGameEntity _currentTarget;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IGameEntity entity)
                && entity.HasTag(GameEntityAPI.CharacterTag))
            {
                _currentTarget = entity;
                TargetUseCase.SetTarget(_enemies, _currentTarget);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other == null)
                return;

            if (other.TryGetComponent(out IGameEntity entity)
                && _currentTarget.Equals(entity))
            {
                _currentTarget = null;
                TargetUseCase.SetTarget(_enemies, null);
            }
        }
    }
}