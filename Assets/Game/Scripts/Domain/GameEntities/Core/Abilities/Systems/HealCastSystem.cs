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
    public partial struct HealCastSystem : ISystem
    {
        private EntityQuery _query;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();

            _query = SystemAPI.QueryBuilder()
                .WithAllRW<AbilityUseRequest>()
                .WithAll<Health>()
                .WithPresentRW<AbilityWindup, AbilityCooldown>()
                .WithPresentRW<
                    AbilityStartedEvent,
                    AbilityExecutedEvent>()
                .Build();

            state.RequireForUpdate(_query);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter commands = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged)
                .AsParallelWriter();

            state.Dependency = new HealCastJob
            {
                Commands = commands,
                EntityHandle = SystemAPI.GetEntityTypeHandle(),
                RequestHandle = SystemAPI.GetComponentTypeHandle<AbilityUseRequest>(),
                WindupHandle = SystemAPI.GetComponentTypeHandle<AbilityWindup>(),
                CooldownHandle = SystemAPI.GetComponentTypeHandle<AbilityCooldown>(),
                StartedEventHandle = SystemAPI.GetComponentTypeHandle<AbilityStartedEvent>(),
                ExecutedEventHandle = SystemAPI.GetComponentTypeHandle<AbilityExecutedEvent>(),
                HealthHandle = SystemAPI.GetComponentTypeHandle<Health>(true),
                ManaHandle = SystemAPI.GetComponentTypeHandle<Mana>(),
                AmountsLookup = SystemAPI.GetComponentLookup<HealAmount>(true),
                DefinitionsLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true),
                CostsLookup = SystemAPI.GetComponentLookup<ManaCost>(true),
                HealthLookup = SystemAPI.GetComponentLookup<Health>(true),
                MaxHealthLookup = SystemAPI.GetComponentLookup<MaxHealth>(true),
                WorldTransformsLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                HealRequestsLookup = SystemAPI.GetBufferLookup<HealRequest>(true)
            }.ScheduleParallel(_query, state.Dependency);
        }

        [BurstCompile]
        private struct HealCastJob : IJobChunk
        {
            [ReadOnly] public EntityTypeHandle EntityHandle;
            [ReadOnly] public ComponentTypeHandle<Health> HealthHandle;

            public ComponentTypeHandle<AbilityUseRequest> RequestHandle;
            public ComponentTypeHandle<AbilityWindup> WindupHandle;
            public ComponentTypeHandle<AbilityCooldown> CooldownHandle;
            public ComponentTypeHandle<AbilityStartedEvent> StartedEventHandle;
            public ComponentTypeHandle<AbilityExecutedEvent> ExecutedEventHandle;

            public ComponentTypeHandle<Mana> ManaHandle;

            [ReadOnly] public ComponentLookup<HealAmount> AmountsLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionsLookup;
            [ReadOnly] public ComponentLookup<ManaCost> CostsLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<MaxHealth> MaxHealthLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public BufferLookup<HealRequest> HealRequestsLookup;

            public EntityCommandBuffer.ParallelWriter Commands;

            public void Execute(
                in ArchetypeChunk chunk,
                int unfilteredChunkIndex,
                bool useEnabledMask,
                in v128 chunkEnabledMask)
            {
                NativeArray<Entity> actors = chunk.GetNativeArray(EntityHandle);
                NativeArray<Health> healths = chunk.GetNativeArray(ref HealthHandle);
                NativeArray<AbilityUseRequest> requests = chunk.GetNativeArray(ref RequestHandle);
                NativeArray<AbilityWindup> windups = chunk.GetNativeArray(ref WindupHandle);
                NativeArray<AbilityCooldown> cooldowns = chunk.GetNativeArray(ref CooldownHandle);
                NativeArray<AbilityStartedEvent> startedEvents = chunk.GetNativeArray(ref StartedEventHandle);
                NativeArray<AbilityExecutedEvent> executedEvents = chunk.GetNativeArray(ref ExecutedEventHandle);

                EnabledMask requestMask = chunk.GetEnabledMask(ref RequestHandle);
                EnabledMask windupMask = chunk.GetEnabledMask(ref WindupHandle);
                EnabledMask cooldownMask = chunk.GetEnabledMask(ref CooldownHandle);
                EnabledMask startedMask = chunk.GetEnabledMask(ref StartedEventHandle);
                EnabledMask executedMask = chunk.GetEnabledMask(ref ExecutedEventHandle);

                bool hasMana = chunk.Has(ref ManaHandle);

                NativeArray<Mana> balances = hasMana
                    ? chunk.GetNativeArray(ref ManaHandle)
                    : default;

                var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
                
                while (enumerator.NextEntityIndex(out int index))
                {
                    RefRW<AbilityUseRequest> requestRef = new RefRW<AbilityUseRequest>(requests, index);
                    RefRW<AbilityWindup> windupRef = new RefRW<AbilityWindup>(windups, index);
                    RefRW<AbilityCooldown> cooldownRef = new RefRW<AbilityCooldown>(cooldowns, index);
                    RefRW<AbilityStartedEvent> startedEventRef = new RefRW<AbilityStartedEvent>(startedEvents, index);
                    RefRW<AbilityExecutedEvent> executeEventRef = new RefRW<AbilityExecutedEvent>(executedEvents, index);
                    RefRW<Health> healthRef = new RefRW<Health>(healths, index);

                    Mana mana = hasMana
                        ? balances[index]
                        : default;

                    ProcessEntity(
                        unfilteredChunkIndex,
                        actors[index],
                        ref requestRef.ValueRW, requestMask.GetEnabledRefRW<AbilityUseRequest>(index),
                        ref windupRef.ValueRW, windupMask.GetEnabledRefRW<AbilityWindup>(index),
                        ref cooldownRef.ValueRW, cooldownMask.GetEnabledRefRW<AbilityCooldown>(index),
                        ref startedEventRef.ValueRW, startedMask.GetEnabledRefRW<AbilityStartedEvent>(index),
                        ref executeEventRef.ValueRW, executedMask.GetEnabledRefRW<AbilityExecutedEvent>(index),
                        in healthRef.ValueRW, hasMana,
                        ref mana);

                    if (hasMana)
                        balances[index] = mana;
                }
            }

            private void ProcessEntity(
                int sortKey,
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
                in Health health,
                bool hasMana,
                ref Mana mana)
            {
                Entity definitionEntity = request.Definition;

                if (AmountsLookup.TryGetRefRO(
                        definitionEntity,
                        out RefRO<HealAmount> amount) == false)
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

                float cost = CostsLookup.TryGetRefRO(
                    definitionEntity,
                    out RefRO<ManaCost> manaCost)
                    ? manaCost.ValueRO.Value
                    : 0f;

                if (request.Preparing == false)
                {
                    bool enoughMana = cost == 0f
                                      || (cost > 0f
                                          && hasMana
                                          && mana.Value >= cost);

                    if (enoughMana == false
                        || (amount.ValueRO.Value > 0f) == false
                        || AbilityUseCase.CanStartTargeted(
                            actor,
                            target,
                            definition.ValueRO.Range,
                            cooldownEnabled.ValueRO,
                            ref HealthLookup,
                            ref WorldTransformsLookup) == false
                        || IsWounded(target) == false
                        || HealRequestsLookup.HasBuffer(target) == false)
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

                if (IsWounded(target) == false
                    || HealRequestsLookup.HasBuffer(target) == false
                    || cost > 0f && hasMana == false)
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
                }

                Commands.AppendToBuffer(sortKey, target, new HealRequest
                {
                    Value = amount.ValueRO.Value
                });

                if (cost > 0f)
                    mana.Value -= cost;

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

            private bool IsWounded(Entity target)
            {
                return HealthLookup.TryGetRefRO(target, out RefRO<Health> health)
                       && MaxHealthLookup.TryGetRefRO(target, out RefRO<MaxHealth> maxHealth)
                       && health.ValueRO.IsWounded(maxHealth.ValueRO);
            }
        }
    }
}