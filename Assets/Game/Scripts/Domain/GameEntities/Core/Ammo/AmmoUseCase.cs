using Game.Components;

namespace Game.UseCases
{
    public static class AmmoUseCase
    {
        public static bool IsExpired(this in AmmoRestoreCooldown cooldown)
        {
            return cooldown.Time <= 0;
        }

        public static void ResetCooldown(this ref AmmoRestoreCooldown cooldown)
        {
            cooldown.Time = cooldown.Cooldown;
        }

        public static bool IsFull(this in Ammo ammo, in MaxAmmo maxAmmo)
        {
            return maxAmmo.Value == ammo.Value;
        }
    }
}