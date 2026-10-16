using Game.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.UseCases
{
    public static class AttackUseCase
    {
        public static bool InAttackDistance(in float3 delta, float attackDistance)
        {
            float lengthSq = math.lengthsq(delta);
            float attackDistanceSq = attackDistance * attackDistance;

            return lengthSq < attackDistanceSq;
        }
        
        public static bool IsExpired(this in AttackCooldown cooldown)
        {
            return cooldown.Time <= 0;
        }
        
        public static bool IsExpired(this in AttackHitDelay hitDelay)
        {
            return hitDelay.Time <= 0;
        }
        
        public static void Reset(this ref AttackHitDelay delay)
        {
            delay.Time = delay.Delay;
        }
        
        public static void Reset(this ref AttackCooldown cooldown)
        {
            cooldown.Time = cooldown.Duration;
        }

        public static float CalculateDamage(
            float baseDamage,
            Entity actor,
            ref ComponentLookup<Damage> damageLookup)
        {
            return damageLookup.TryGetRefRO(actor, out RefRO<Damage> actorDamage)
                ? baseDamage + actorDamage.ValueRO.Value
                : baseDamage;
        }

        public static void NotifyStarted(Entity actor, ref ComponentLookup<AttackStartedEvent> eventLookup)
        {
            if (eventLookup.HasComponent(actor))
                eventLookup.SetComponentEnabled(actor, true);
        }
        
        public static void NotifyHit(Entity actor, Entity target, ref ComponentLookup<AttackHitEvent> eventLookup)
        {
            if (eventLookup.TryGetRefRW(actor, out var hitEvent) == false)
                return;

            hitEvent.ValueRW.Target = target;
            eventLookup.SetComponentEnabled(actor, true);
        }
    }
}