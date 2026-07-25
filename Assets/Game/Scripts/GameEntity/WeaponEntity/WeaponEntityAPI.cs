using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game.Weapon
{
    public static class WeaponEntityAPI
    {
        // Weapon
        public static TagKey WeaponTag = new(nameof(WeaponTag));
        public static ValueKey<IGameEntity, IRequest> FireRequest = new(nameof(FireRequest));
        public static ValueKey<IGameEntity, ICommand> FireCommand = new(nameof(FireCommand));
        public static ValueKey<IGameEntity, ICooldown> FireCooldown = new(nameof(FireCooldown));
        public static ValueKey<IGameEntity, IGameEntity> CurrentWeapon = new(nameof(CurrentWeapon));
    }
}