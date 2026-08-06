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
        private ConsumablePickupInstaller _consumablePickupInstaller;

        [SerializeField]
        private int _amountAmmo;

        public override void Install(IGameEntity entity)
        {
            _consumablePickupInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor =>
                    interactor.TryGetValue(GameEntityAPI.Weapon, out IReactiveVariable<IWeaponEntity> weapon)
                    && weapon.Value?.GetValue(WeaponEntityAPI.Ammo) != null)
                .AddAction(interactor =>
                {
                    IReactiveVariable<IWeaponEntity> weapon = interactor.GetValue(GameEntityAPI.Weapon);
                    IReactiveVariable<int> ammo = weapon.Value.GetValue(WeaponEntityAPI.Ammo);
                    
                    ammo.Value += _amountAmmo;
                });
        }
    }
}