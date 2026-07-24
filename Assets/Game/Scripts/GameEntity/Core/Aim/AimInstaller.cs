using System;
using Atomic.Elements;
using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.GameEntity.Core.Aim
{
    [Serializable]
    public class AimInstaller : IEntityInstaller<IGameEntity>
    {
        private readonly DisposableComposite _disposable = new();
        
        [SerializeField]
        private Cooldown _aimCooldown;
        
        public void Install(IGameEntity entity)
        {
            entity.WhenFixedTick(_aimCooldown.Tick).AddTo(_disposable);
            entity.WhenFixedTick(_ =>
            {
                bool isAiming = 
                    UIContext.Instance.GetValue(UIContextAPI.AimJoystick).Direction != Vector2.zero;
                
                if(isAiming && entity.GetValue(GameEntityAPI.HasAimingLastFrame).Value == false)
                    entity.GetValue(GameEntityAPI.AimCooldown).ResetTime();

                entity.GetValue(GameEntityAPI.HasAimingLastFrame).Value = isAiming;
            }).AddTo(_disposable);
            
            entity.AddValue(GameEntityAPI.AimCooldown, _aimCooldown);
            entity.AddValue(GameEntityAPI.HasAimingLastFrame, new Variable<bool>());
        }

        public void Uninstall()
        {
            _disposable.Dispose();
        }
    }
}