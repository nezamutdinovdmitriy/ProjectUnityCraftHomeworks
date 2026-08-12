using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;

namespace Game.GameEntity.Content.Items
{
    public static class ItemsUseCase
    {
        public static void PickupAmmo(this IGameEntity entity, int value)
        {
            IReactiveVariable<IWeaponEntity> weapon = entity.GetValue(GameEntityAPI.Weapon);
            IReactiveVariable<int> ammo = weapon.Value.GetValue(WeaponEntityAPI.Ammo);
            
            ammo.Value += value;
        }
    }
}