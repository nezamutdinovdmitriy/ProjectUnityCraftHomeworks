using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity.Core.LifeTime
{
    [Serializable]
    public class LifeTimeInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private Cooldown _lifetime;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Lifetime, _lifetime);
            entity.AddValue(GameEntityAPI.DestroyAction, new CompositeAction());

            entity.AddBehaviour(new LifeTimeBehaviour());
        }
    }
}