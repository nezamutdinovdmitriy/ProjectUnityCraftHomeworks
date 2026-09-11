using Atomic.Entities;
using Game.GameEntities;
using UnityEngine;

namespace Game.Weapon
{
    public static class WeaponUseCase
    {
        public static bool HasAmmo(this IWeaponEntity weapon)
            => weapon.GetValue(WeaponEntityAPI.Ammo).Value > 0;

        public static bool HasOwner(this IWeaponEntity weapon)
        {
            IGameEntity owner = weapon.GetValue(WeaponEntityAPI.Owner).Value;

            bool hasOwner = owner != null;
            bool isOwnerAlive = owner.IsDead() == false;

            return hasOwner && isOwnerAlive;
        }

        public static IGameEntity GetOwner(this IWeaponEntity weapon)
            => weapon.GetValue(WeaponEntityAPI.Owner).Value;

        public static bool IsFireCooldownCompleted(this IWeaponEntity weapon)
            => weapon.GetValue(WeaponEntityAPI.FireCooldown).IsCompleted();

        public static void ConsumeAmmo(this IWeaponEntity weapon)
            => weapon.GetValue(WeaponEntityAPI.Ammo).Value--;

        public static void ResetCooldown(this IWeaponEntity weapon)
            => weapon.GetValue(WeaponEntityAPI.FireCooldown).ResetTime();

        public static bool TryFindFirstMeleeHit(
            this IWeaponEntity weapon,
            Vector3 position,
            float attackRadius,
            Collider[] buffer,
            out IGameEntity targetHit)
        {
            var hitsCount = Physics.OverlapSphereNonAlloc(position, attackRadius, buffer);
            IGameEntity owner = weapon.GetOwner();

            for (int i = 0; i < hitsCount; i++)
            {
                if (weapon.TryGetValidTarget(buffer[i], owner, out IGameEntity target))
                {
                    targetHit = target;
                    return true;
                }
            }
            
            targetHit = null;
            return false;
        }

        public static bool TryGetValidTarget(
            this IWeaponEntity weapon,
            Collider collider,
            IGameEntity owner,
            out IGameEntity target)
        {
            target = null;

            if (collider.TryGetComponent(out IGameEntity entity) == false
                || entity.Equals(owner)
                || entity.HasTag(GameEntityAPI.CharacterTag) == false
                || entity.IsDead())
                return false;

            target = entity;
            return true;
        }
    }
}