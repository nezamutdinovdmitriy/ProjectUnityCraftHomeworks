using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    public class Platform : 
        MoveRequestComponent.IAction,
        IInitializable
    {
        private readonly MoveRequestComponent _moveRequestComponent;
        private readonly MoveTransformComponent _moveTransformComponent;

        public Platform(
            MoveTransformComponent transform,
            MoveRequestComponent moveRequestComponent)
        {
            _moveTransformComponent = transform;
            _moveRequestComponent = moveRequestComponent;
        }

        public void Initialize() => MovementBehaviourSetup();

        private void MovementBehaviourSetup() 
            => _moveRequestComponent.SetAction(this);

        void MoveRequestComponent.IAction.Invoke(Vector2 direction) 
            => _moveTransformComponent.Move(direction);
    }
}