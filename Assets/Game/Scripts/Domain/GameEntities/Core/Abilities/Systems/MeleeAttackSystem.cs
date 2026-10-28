using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    public partial struct MeleeAttackSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new MeleeAttackJob
            {
                AbilitiesLookup = SystemAPI
                    .GetComponentLookup<MeleeAttackAbility>(true),

                DefinitionsLookup = SystemAPI
                    .GetComponentLookup<AbilityDefinition>(true),

                HealthLookup = SystemAPI
                    .GetComponentLookup<Health>(true),

                WorldTransformsLookup = SystemAPI
                    .GetComponentLookup<LocalToWorld>(true),

                DamageLookup = SystemAPI
                    .GetComponentLookup<Damage>(true),

                DamageRequestsLookup = SystemAPI
                    .GetBufferLookup<TakeDamageRequest>(),

                StartedEventsLookup = SystemAPI
                    .GetComponentLookup<AbilityStartedEvent>(),

                ExecutedEventsLookup = SystemAPI
                    .GetComponentLookup<AbilityExecutedEvent>()
            }.Schedule(state.Dependency);
        }
        
        
        [BurstCompile]
        [WithPresent(typeof(AbilityWindup), typeof(AbilityCooldown))]
        private partial struct MeleeAttackJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<MeleeAttackAbility> AbilitiesLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionsLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public ComponentLookup<Damage> DamageLookup;

            public BufferLookup<TakeDamageRequest> DamageRequestsLookup;

            public ComponentLookup<AbilityStartedEvent> StartedEventsLookup;
            public ComponentLookup<AbilityExecutedEvent> ExecutedEventsLookup;
            
            private void Execute(
                Entity actor,
                ref AbilityUseRequest request,
                EnabledRefRW<AbilityUseRequest> requestEnabled,
                ref AbilityWindup windup,
                EnabledRefRW<AbilityWindup> windupEnabled,
                ref AbilityCooldown cooldown,
                EnabledRefRW<AbilityCooldown> cooldownEnabled,
                in Health health)
            {
                Entity definitionEntity = request.Definition;

                if (AbilitiesLookup.TryGetRefRO(
                        definitionEntity,
                        out RefRO<MeleeAttackAbility> ability) == false)
                    return;

                if (request.CancelRequested
                    || health.IsDead()
                    || DefinitionsLookup.TryGetRefRO(
                        definitionEntity,
                        out RefRO<AbilityDefinition> definition) == false)
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
                }

                Entity target = request.Target;

                if (request.Preparing == false)
                {
                    bool canStart = AbilityUseCase.CanStartTargeted(
                        actor,
                        target,
                        definition.ValueRO.Range,
                        cooldownEnabled.ValueRO,
                        ref HealthLookup,
                        ref WorldTransformsLookup);

                    if (canStart == false)
                    {
                        AbilityUseCase.Finish(
                            ref request, requestEnabled,
                            ref windup, windupEnabled);

                        return;
                    }

                    AbilityUseCase.Begin(
                        ref request,
                        ref windup, windupEnabled,
                        definition.ValueRO.Windup);

                    AbilityUseCase.NotifyStarted(
                        actor,
                        request.Definition,
                        ref StartedEventsLookup);
                }

                if (windupEnabled.ValueRO)
                    return;

                if (HealthLookup.TryGetRefRO(
                        target,
                        out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead()
                    || DamageRequestsLookup.TryGetBuffer(
                        target,
                        out DynamicBuffer<TakeDamageRequest> requests) == false)
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
                }

                float damage = DamageUseCase.Calculate(
                    ability.ValueRO.Damage,
                    actor,
                    ref DamageLookup);

                requests.Add(new TakeDamageRequest
                {
                    Damage = damage
                });

                AbilityUseCase.NotifyExecuted(
                    actor,
                    target,
                    request.Definition,
                    ref ExecutedEventsLookup);

                AbilityUseCase.Complete(
                    ref request, requestEnabled,
                    ref windup, windupEnabled,
                    ref cooldown, cooldownEnabled,
                    definition.ValueRO.Cooldown);
            }
        }
    }
}