using Game.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.UseCases
{
    public static class AbilityUseCase
    {
        public static void Begin(
            ref AbilityUseRequest request,
            ref AbilityWindup windup,
            EnabledRefRW<AbilityWindup> windupEnabled,
            float duration)
        {
            request.Preparing = true;
            windup.Time = math.max(0f, duration);
            windupEnabled.ValueRW = windup.Time > 0f;
        }
        
        public static void Finish(
            ref AbilityUseRequest request,
            EnabledRefRW<AbilityUseRequest> requestEnabled,
            ref AbilityWindup windup,
            EnabledRefRW<AbilityWindup> windupEnabled)
        {
            request.Preparing = false;
            request.CancelRequested = false;
            requestEnabled.ValueRW = false;

            windup.Time = 0f;
            windupEnabled.ValueRW = false;
        }
        
        public static void StartCooldown(
            ref AbilityCooldown cooldown,
            EnabledRefRW<AbilityCooldown> cooldownEnabled,
            float duration)
        {
            cooldown.Time = math.max(0f, duration);
            cooldownEnabled.ValueRW = cooldown.Time > 0f;
        }

        public static bool CanStartTargeted(
            in Entity actor,
            in Entity target,
            float range,
            bool cooldownEnabled,
            ref ComponentLookup<Health> healthLookup,
            ref ComponentLookup<LocalToWorld> worldTransformLookup)
        {
            if (cooldownEnabled
                || healthLookup.TryGetRefRO(
                    target, out RefRO<Health> targetHealth) == false
                || targetHealth.ValueRO.IsDead()
                || worldTransformLookup.TryGetRefRO(
                    actor, out RefRO<LocalToWorld> actorTransform) == false
                || worldTransformLookup.TryGetRefRO(
                    target, out RefRO<LocalToWorld> targetTransform) == false)
                return false;

            float distanceSq = math.distancesq(
                actorTransform.ValueRO.Position,
                targetTransform.ValueRO.Position);

            return distanceSq <= range * range;
        }

        public static void Complete(
            ref AbilityUseRequest request,
            EnabledRefRW<AbilityUseRequest> requestEnabled,
            ref AbilityWindup windup,
            EnabledRefRW<AbilityWindup> windupEnabled,
            ref AbilityCooldown cooldown,
            EnabledRefRW<AbilityCooldown> cooldownEnabled,
            float cooldownDuration)
        {
            Finish(
                ref request,
                requestEnabled,
                ref windup,
                windupEnabled);

            StartCooldown(
                ref cooldown,
                cooldownEnabled,
                cooldownDuration);
        }
        
        public static void NotifyStarted(
            Entity definition,
            ref AbilityStartedEvent startedEvent,
            EnabledRefRW<AbilityStartedEvent> eventEnabled)
        {
            startedEvent.Definition = definition;
            eventEnabled.ValueRW = true;
        }
        
        public static void NotifyExecuted(
            Entity target,
            Entity definition, 
            ref AbilityExecutedEvent executedEvent,
            EnabledRefRW<AbilityExecutedEvent> eventEnabled)
        {
            executedEvent.Definition = definition;
            executedEvent.Target = target;
            
            eventEnabled.ValueRW = true;
        }
    }
}