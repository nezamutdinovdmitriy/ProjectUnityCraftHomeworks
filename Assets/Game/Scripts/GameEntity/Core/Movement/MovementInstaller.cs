using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    [Serializable]
    public class MovementInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private float _movementSpeed;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.MovementSpeed, new Const<float>(_movementSpeed));
            entity.AddValue(GameEntityAPI.IsMoving, new ReactiveVariable<bool>());
            
            entity.AddValue(GameEntityAPI.MovementRequest, new Request<Vector3>());
            entity.AddValue(GameEntityAPI.MovementCommand, new Command<MovementArgs>());
            entity.AddValue(GameEntityAPI.MovementDirection, new Variable<Vector3>());
            
            entity.AddBehaviour(new MovementBehaviour());
        }
    }
}