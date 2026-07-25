using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game.Weapon
{
    public static class WeaponEntityAPI
    {
        // Common
        public static TagKey WeaponTag = new(nameof(WeaponTag));
        public static ValueKey<IWeaponEntity, IReactiveVariable<IGameEntity>> Owner = new(nameof(Owner));
        
        // Fire
        public static ValueKey<IWeaponEntity, ICommand> FireCommand = new(nameof(FireCommand));
        public static ValueKey<IWeaponEntity, ICooldown> FireCooldown = new(nameof(FireCooldown));
    }
}