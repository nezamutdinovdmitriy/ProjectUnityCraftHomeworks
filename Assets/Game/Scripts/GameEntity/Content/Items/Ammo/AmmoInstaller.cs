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

        public override void Install(IGameEntity entity)
        {
            _interactableInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor =>
                    interactor.HasTag(GameEntityAPI.InteractorTag)
                    && interactor.TryGetValue(GameEntityAPI.Weapon, out IReactiveVariable<IWeaponEntity> weapon)
                    && weapon.Value != null)
                .AddAction(interactor =>
                {
                    IReactiveVariable<IWeaponEntity> weapon = interactor.GetValue(GameEntityAPI.Weapon);

                    if (weapon.Value.TryGetValue(WeaponEntityAPI.Ammo, out IReactiveVariable<int> ammo) == false)
                        return;

                    ammo.Value += _amountAmmo;
                    Destroy(gameObject);
                });
        }
    }
}