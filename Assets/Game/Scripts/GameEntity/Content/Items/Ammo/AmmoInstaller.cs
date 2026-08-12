using System;
using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;
using Game.Weapon.Content;
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
                .AddCondition(interactor
                    => interactor.TryGetWeapon(out IReactiveVariable<IWeaponEntity> weapon)
                       && weapon.Value.HasAmmo())
                .AddAction(interactor => interactor.PickupAmmo(_amountAmmo));
        }
    }
}