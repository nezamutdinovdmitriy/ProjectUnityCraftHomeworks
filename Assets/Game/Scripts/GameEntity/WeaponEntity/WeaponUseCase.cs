using Atomic.Entities;

namespace Game.Weapon.Content
{
    public static class WeaponUseCase
    {
        public static bool HasAmmo(this IWeaponEntity weapon)
        {
            return weapon.GetValue(WeaponEntityAPI.Ammo) != null;
        }
    }
}