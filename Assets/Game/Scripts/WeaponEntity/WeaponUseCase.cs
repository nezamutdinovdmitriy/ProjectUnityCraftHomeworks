using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities.Weapon
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

        public static bool IsFireCooldownCompleted(this IWeaponEntity weapon) 
            => weapon.GetValue(WeaponEntityAPI.FireCooldown).IsCompleted();

        public static void Fire(this IWeaponEntity weapon, IGameContext gameContext, Transform firePoint)
        {
            gameContext.SpawnBullet(
                firePoint.position, 
                firePoint.rotation, 
                weapon.GetValue(WeaponEntityAPI.Owner).Value);
                
            weapon.GetValue(WeaponEntityAPI.FireCooldown).ResetTime();
            weapon.GetValue(WeaponEntityAPI.Ammo).Value--;
        }
        
        public static void AttackMelee(
            this IWeaponEntity weapon, 
            Vector3 position, 
            float attackRadius, 
            Collider[] buffer, 
            float damage)
        {
            weapon.GetValue(WeaponEntityAPI.FireCooldown).ResetTime();

            IGameEntity owner = weapon.GetValue(WeaponEntityAPI.Owner).Value;

            int size = Physics.OverlapSphereNonAlloc(position, attackRadius, buffer);

            for (int i = 0; i < size; i++)
            {
                if (buffer[i].TryGetComponent(out IGameEntity entity)
                    && entity.Equals(owner) == false
                    && entity.HasTag(GameEntityAPI.CharacterTag))
                {
                    if (entity.IsDead())
                        continue;

                    bool success = entity.TryInvokeTakeDamageCommand(damage);
                    
                    if (success)
                        return;
                }
            }
        }
    }
}