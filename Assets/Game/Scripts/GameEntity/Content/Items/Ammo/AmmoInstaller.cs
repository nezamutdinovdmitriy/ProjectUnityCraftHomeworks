using System;
using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;
using UnityEngine;

namespace Game.GameEntity
{
    [Serializable]
    public class AmmoInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private InteractableInstaller _interactableInstaller;

        [SerializeField]
        private int _amountAmmo;

        [SerializeField]
        private Cooldown _destroyTimer;

        private bool _wasUsed;
        
        public override void Install(IGameEntity entity)
        {
            _interactableInstaller.Install(entity);

            entity.WhenFixedTick(deltaTime =>
            {
                if(_wasUsed)
                    _destroyTimer.Tick(deltaTime);
                
                if(_destroyTimer.IsCompleted())
                    Destroy(gameObject);
            });
            
            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor =>
                    interactor.HasTag(GameEntityAPI.InteractorTag)
                    && _wasUsed == false
                    && interactor.TryGetValue(GameEntityAPI.Weapon, out IReactiveVariable<IWeaponEntity> weapon)
                    && weapon.Value?.GetValue(WeaponEntityAPI.Ammo) != null)
                .AddAction(interactor =>
                {
                    IReactiveVariable<IWeaponEntity> weapon = interactor.GetValue(GameEntityAPI.Weapon);

                    IReactiveVariable<int> ammo = weapon.Value.GetValue(WeaponEntityAPI.Ammo);
                    ammo.Value += _amountAmmo;

                    _wasUsed = true;
                });
        }
    }
}