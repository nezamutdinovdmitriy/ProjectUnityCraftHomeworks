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
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter commands = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged)
                .AsParallelWriter();
            
            state.Dependency = new MeleeAttackJob
            {
                Commands = commands,
                AbilitiesLookup = SystemAPI.GetComponentLookup<MeleeAttackAbility>(true),
                DefinitionsLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true),
                HealthLookup = SystemAPI.GetComponentLookup<Health>(true),
                WorldTransformsLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                DamageLookup = SystemAPI.GetComponentLookup<Damage>(true),
                DamageRequestsLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>(true),
            }.ScheduleParallel(state.Dependency);
        }
        
        
        [BurstCompile]
        [WithPresent(
            typeof(AbilityWindup),
            typeof(AbilityCooldown),
            typeof(AbilityStartedEvent),
            typeof(AbilityExecutedEvent))]
        private partial struct MeleeAttackJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<MeleeAttackAbility> AbilitiesLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionsLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public ComponentLookup<Damage> DamageLookup;
            [ReadOnly] public BufferLookup<TakeDamageRequest> DamageRequestsLookup;
            
            public EntityCommandBuffer.ParallelWriter Commands;
            
            private void Execute(
                [ChunkIndexInQuery] int sortKey,
                Entity actor,
                ref AbilityUseRequest request,
                EnabledRefRW<AbilityUseRequest> requestEnabled,
                ref AbilityWindup windup,
                EnabledRefRW<AbilityWindup> windupEnabled,
                ref AbilityCooldown cooldown,
                EnabledRefRW<AbilityCooldown> cooldownEnabled,
                ref AbilityStartedEvent startedEvent,
                EnabledRefRW<AbilityStartedEvent> startedEventEnabled,
                ref AbilityExecutedEvent executedEvent,
                EnabledRefRW<AbilityExecutedEvent> executedEventEnabled,
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
                        request.Definition,
                        ref startedEvent,
                        startedEventEnabled);
                }

                if (windupEnabled.ValueRO)
                    return;

                if (HealthLookup.TryGetRefRO(
                        target,
                        out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead()
                    || DamageRequestsLookup.HasBuffer(target) == false)
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

                Commands.AppendToBuffer(sortKey, target, new TakeDamageRequest
                {
                    Damage = damage
                });

                AbilityUseCase.NotifyExecuted(
                    target,
                    request.Definition,
                    ref executedEvent,
                    executedEventEnabled);

                AbilityUseCase.Complete(
                    ref request, requestEnabled,
                    ref windup, windupEnabled,
                    ref cooldown, cooldownEnabled,
                    definition.ValueRO.Cooldown);
            }
        }
    }
}