using Atomic.Entities;

/**
 * Created by Entity Domain Generator.
 */

namespace Game.Weapon
{
    /// <summary>
    /// Represents a Unity <see cref="SceneEntity"/> implementation for <see cref="IWeaponEntity"/>.
    /// This component can be instantiated directly in a Scene and composed via the Unity Inspector.
    /// </summary>
    public sealed class WeaponEntity : SceneEntity, IWeaponEntity
    {
    }
}
