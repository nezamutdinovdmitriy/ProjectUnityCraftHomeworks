using Game.GameEntity;
using UnityEngine;

namespace Atomic.Entities
{
    public class InteractBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private IGameEntity _interactor;

        public void Init(IGameEntity entity)
        {
            _interactor = entity;

            _interactor.GetValue(GameEntityAPI.Trigger).OnEntered += OnTriggerEntered;
        }

        public void Dispose(IGameEntity entity)
            => _interactor.GetValue(GameEntityAPI.Trigger).OnEntered -= OnTriggerEntered;

        private void OnTriggerEntered(Collider obj)
        {
            if (obj.TryGetComponent(out IGameEntity interactable))
                _interactor.Interact(interactable);
        }
    }
}