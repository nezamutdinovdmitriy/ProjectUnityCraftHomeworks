using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    [Serializable]
    public class RotateInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private float _rotationSpeed;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.RotationSpeed, new Const<float>(_rotationSpeed));
        }
    }
}