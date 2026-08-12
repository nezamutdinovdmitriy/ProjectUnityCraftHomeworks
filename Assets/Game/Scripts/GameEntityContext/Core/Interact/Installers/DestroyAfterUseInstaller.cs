using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
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