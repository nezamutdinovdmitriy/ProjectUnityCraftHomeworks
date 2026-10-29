using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    public partial struct MeleeAttackSystem : ISystem
    {
        private EntityQuery _query;

        public void OnCreate(ref SystemState state)
        {
            _query = SystemAPI.QueryBuilder()
                .WithPresentRW<AbilityWindup, AbilityCooldown>()
                .WithPresentRW<AbilityStartedEvent, AbilityExecutedEvent>()
                .WithAll<Health>()
                .WithAllRW<AbilityUseRequest>()
                .Build();

            state.RequireForUpdate(_query);
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter commands = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged)
                .AsParallelWriter();

            MeleeAttackJob job = new MeleeAttackJob
            {
                Commands = commands,

                EntityHandle = SystemAPI.GetEntityTypeHandle(),

                RequestHandle = SystemAPI.GetComponentTypeHandle<AbilityUseRequest>(),
                WindupHandle = SystemAPI.GetComponentTypeHandle<AbilityWindup>(),
                CooldownHandle = SystemAPI.GetComponentTypeHandle<AbilityCooldown>(),
                StartedEventHandle = SystemAPI.GetComponentTypeHandle<AbilityStartedEvent>(),
                ExecutedEventHandle = SystemAPI.GetComponentTypeHandle<AbilityExecutedEvent>(),
                HealthHandle = SystemAPI.GetComponentTypeHandle<Health>(true),
                DamageHandle = SystemAPI.GetComponentTypeHandle<Damage>(true),

                AbilitiesLookup = SystemAPI.GetComponentLookup<MeleeAttackAbility>(true),
                DefinitionsLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true),
                HealthLookup = SystemAPI.GetComponentLookup<Health>(true),
                WorldTransformsLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                DamageRequestsLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>(true)
            };

            state.Dependency = job.ScheduleParallelByRef(_query, state.Dependency);
        }


        [BurstCompile]
        private struct MeleeAttackJob : IJobChunk
        {
            [ReadOnly] public EntityTypeHandle EntityHandle;

            [ReadOnly] public ComponentTypeHandle<Damage> DamageHandle;
            [ReadOnly] public ComponentTypeHandle<Health> HealthHandle;

            public ComponentTypeHandle<AbilityUseRequest> RequestHandle;
            public ComponentTypeHandle<AbilityWindup> WindupHandle;
            public ComponentTypeHandle<AbilityCooldown> CooldownHandle;
            public ComponentTypeHandle<AbilityStartedEvent> StartedEventHandle;
            public ComponentTypeHandle<AbilityExecutedEvent> ExecutedEventHandle;

            [ReadOnly] public ComponentLookup<MeleeAttackAbility> AbilitiesLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionsLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public BufferLookup<TakeDamageRequest> DamageRequestsLookup;

            public EntityCommandBuffer.ParallelWriter Commands;

            public void Execute(
                in ArchetypeChunk chunk,
                int unfilteredChunkIndex,
                bool useEnabledMask,
                in v128 chunkEnabledMask)
            {
                NativeArray<Entity> entities = chunk.GetNativeArray(EntityHandle);

                NativeArray<AbilityUseRequest> requests = chunk.GetNativeArray(ref RequestHandle);
                NativeArray<AbilityWindup> windups = chunk.GetNativeArray(ref WindupHandle);
                NativeArray<AbilityCooldown> cooldowns = chunk.GetNativeArray(ref CooldownHandle);
                NativeArray<AbilityStartedEvent> startedEvents = chunk.GetNativeArray(ref StartedEventHandle);
                NativeArray<AbilityExecutedEvent> executedEvents = chunk.GetNativeArray(ref ExecutedEventHandle);
                NativeArray<Health> healths = chunk.GetNativeArray(ref HealthHandle);

                EnabledMask requestMask = chunk.GetEnabledMask(ref RequestHandle);
                EnabledMask windupMask = chunk.GetEnabledMask(ref WindupHandle);
                EnabledMask cooldownMask = chunk.GetEnabledMask(ref CooldownHandle);
                EnabledMask startedEventMask = chunk.GetEnabledMask(ref StartedEventHandle);
                EnabledMask executedEventMask = chunk.GetEnabledMask(ref ExecutedEventHandle);

                bool hasDamage = chunk.Has(ref DamageHandle);

                NativeArray<Damage> damages = hasDamage
                    ? chunk.GetNativeArray(ref DamageHandle)
                    : default;

                var enumerator = new ChunkEntityEnumerator(
                    useEnabledMask,
                    chunkEnabledMask,
                    chunk.Count);

                while (enumerator.NextEntityIndex(out int index))
                {
                    var requestRef = new RefRW<AbilityUseRequest>(requests, index);
                    var windupRef = new RefRW<AbilityWindup>(windups, index);
                    var cooldownRef = new RefRW<AbilityCooldown>(cooldowns, index);
                    var healthRef = new RefRO<Health>(healths, index);

                    var startedEventRef = new RefRW<AbilityStartedEvent>(
                        startedEvents,
                        index);

                    var executedEventRef = new RefRW<AbilityExecutedEvent>(
                        executedEvents,
                        index);

                    float actorDamage = hasDamage
                        ? damages[index].Value
                        : 0f;

                    ProcessMeleeAttack(
                        unfilteredChunkIndex,
                        entities[index],
                        actorDamage,
                        ref requestRef.ValueRW, requestMask.GetEnabledRefRW<AbilityUseRequest>(index),
                        ref windupRef.ValueRW, windupMask.GetEnabledRefRW<AbilityWindup>(index),
                        ref cooldownRef.ValueRW, cooldownMask.GetEnabledRefRW<AbilityCooldown>(index),
                        ref startedEventRef.ValueRW, startedEventMask.GetEnabledRefRW<AbilityStartedEvent>(index),
                        ref executedEventRef.ValueRW, executedEventMask.GetEnabledRefRW<AbilityExecutedEvent>(index),
                        in healthRef.ValueRO);
                }
            }

            private void ProcessMeleeAttack(
                int sortKey,
                Entity actor,
                float actorDamage,
                ref AbilityUseRequest request, EnabledRefRW<AbilityUseRequest> requestEnabled,
                ref AbilityWindup windup, EnabledRefRW<AbilityWindup> windupEnabled,
                ref AbilityCooldown cooldown, EnabledRefRW<AbilityCooldown> cooldownEnabled,
                ref AbilityStartedEvent startedEvent, EnabledRefRW<AbilityStartedEvent> startedEventEnabled,
                ref AbilityExecutedEvent executedEvent, EnabledRefRW<AbilityExecutedEvent> executedEventEnabled,
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
                        ref request,
                        requestEnabled,
                        ref windup,
                        windupEnabled);

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
                            ref request,
                            requestEnabled,
                            ref windup,
                            windupEnabled);

                        return;
                    }

                    AbilityUseCase.Begin(
                        ref request,
                        ref windup,
                        windupEnabled,
                        definition.ValueRO.Windup);

                    AbilityUseCase.NotifyStarted(
                        definitionEntity,
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
                        ref request,
                        requestEnabled,
                        ref windup,
                        windupEnabled);

                    return;
                }

                float damage = DamageUseCase.Calculate(ability.ValueRO.Damage, actorDamage);

                Commands.AppendToBuffer(sortKey, target, new TakeDamageRequest
                {
                    Damage = damage
                });

                AbilityUseCase.NotifyExecuted(
                    target,
                    definitionEntity,
                    ref executedEvent,
                    executedEventEnabled);

                AbilityUseCase.Complete(
                    ref request,
                    requestEnabled,
                    ref windup,
                    windupEnabled,
                    ref cooldown,
                    cooldownEnabled,
                    definition.ValueRO.Cooldown);
            }
        }
    }
}