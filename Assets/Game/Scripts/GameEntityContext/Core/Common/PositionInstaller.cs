using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    [Serializable]
    public class PositionInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private Transform _position;
        
        public void Install(IGameEntity entity) 
            => entity.AddValue(GameEntityAPI.Position, new TransformPositionVariable(_position));
    }
}