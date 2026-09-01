using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class AimBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IVariable<Vector3> _aimDirection;
        private ICooldown _cooldown;
        private IVariable<bool> _hasAimingLastFrame;
        private IRequest _fireRequest;
        
        public void Init(IGameEntity entity)
        {
            _aimDirection = entity.GetValue(GameEntityAPI.AimDirection);
            _cooldown = entity.GetValue(GameEntityAPI.AimCooldown);
            _hasAimingLastFrame = entity.GetValue(GameEntityAPI.HasAimingLastFrame);
            _fireRequest = entity.GetValue(GameEntityAPI.FireRequest);
        }
        
        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            entity.AimingProcess(
                _aimDirection,
                _hasAimingLastFrame,
                _cooldown,
                _fireRequest,
                deltaTime);
        }
    }
}