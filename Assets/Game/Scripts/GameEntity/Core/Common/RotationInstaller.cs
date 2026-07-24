using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    [Serializable]
    public class RotationInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private Transform _rotation;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Rotation, new TransformRotationVariable(_rotation));
        }
    }
}