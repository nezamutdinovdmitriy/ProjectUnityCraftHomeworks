using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;

namespace Game.GameEntity
{
    public static class FireUseCase
    {
        public static bool HasWeapon(this IGameEntity entity)
        {
            if (entity.TryGetValue(
                    GameEntityAPI.Weapon, 
                    out IReactiveVariable<IWeaponEntity> weapon)
                && weapon.Value != null)
                return true;
            
            return false;
        }

        public static void InvokeFireRequest(this IGameEntity entity)
        {
            IReactiveVariable<IWeaponEntity> weapon = entity.GetValue(GameEntityAPI.Weapon);
            weapon.Value.GetValue(WeaponEntityAPI.FireRequest).Invoke();
        }
    }
}