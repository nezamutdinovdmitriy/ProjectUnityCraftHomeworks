using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    [Serializable]
    public class AimInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private Cooldown _aimCooldown;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.AimCooldown, _aimCooldown);
            entity.AddValue(GameEntityAPI.HasAimingLastFrame, new Variable<bool>());
            entity.AddValue(GameEntityAPI.AimDirection, new Variable<Vector3>());
            
            entity.AddBehaviour(new AimBehaviour());
        }
    }
}