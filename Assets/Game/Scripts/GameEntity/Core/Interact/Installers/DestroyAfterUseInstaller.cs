using System;
using Atomic.Elements;
using Game.GameEntity;
using UnityEngine;

namespace Atomic.Entities
{
    [Serializable]
    public class DestroyAfterUseInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private Cooldown _destroyTimer;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.WasUsed, new Variable<bool>());
            
            entity.AddBehaviour(new DestroyAfterUseBehaviour(_destroyTimer));
        }
    }
}