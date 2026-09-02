using System;
using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;
using UnityEngine;

namespace Game.GameEntities
{
    [Serializable]
    public class WeaponInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private WeaponEntity _weapon;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Weapon, new ReactiveVariable<IWeaponEntity>(_weapon));

            IWeaponEntity weapon = entity.GetValue(GameEntityAPI.Weapon).Value;
            weapon.GetValue(WeaponEntityAPI.Owner).Value = entity;
        }
    }
}