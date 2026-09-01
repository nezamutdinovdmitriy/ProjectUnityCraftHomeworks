using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities.Weapon
{
    public static class WeaponEntityAPI
    {
        // Common
        public static TagKey WeaponTag = new(nameof(WeaponTag));
        public static ValueKey<IWeaponEntity, IReactiveVariable<IGameEntity>> Owner = new(nameof(Owner));
        public static ValueKey<IWeaponEntity, IReactiveVariable<int>> Ammo = new(nameof(Ammo));
        
        // Fire
        public static ValueKey<IWeaponEntity, IEvent> FireStartEvent = new(nameof(FireStartEvent));
        public static ValueKey<IWeaponEntity, IRequest> FireRequest = new(nameof(FireRequest));
        public static ValueKey<IWeaponEntity, ICommand> FireCommand = new(nameof(FireCommand));
        public static ValueKey<IWeaponEntity, ICooldown> FireCooldown = new(nameof(FireCooldown));
    }
}