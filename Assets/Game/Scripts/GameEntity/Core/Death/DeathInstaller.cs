using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity.Core.Death
{
    [Serializable]
    public class DeathInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private Cooldown _deathDelay;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.DeathDelay, _deathDelay);
            entity.AddValue(GameEntityAPI.DeathAction, new CompositeAction());
            
            entity.AddBehaviour(new DeathBehaviour());
        }
    }
}