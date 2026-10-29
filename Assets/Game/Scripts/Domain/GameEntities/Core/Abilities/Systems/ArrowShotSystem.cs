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
    public partial struct ArrowShotSystem : ISystem
    {
        private EntityQuery _query;

        public void OnCreate(ref SystemState state)
        {
            _query = SystemAPI.QueryBuilder()
                .WithAllRW<AbilityUseRequest>()
                .WithAll<Health>()
                .WithPresentRW<AbilityWindup, AbilityCooldown>()
                .WithPresentRW<AbilityStartedEvent, AbilityExecutedEvent>()
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

            state.Dependency = new ArrowShotJob
            {
                Commands = commands,
                EntityHandle = SystemAPI.GetEntityTypeHandle(),
                RequestHandle = SystemAPI.GetComponentTypeHandle<AbilityUseRequest>(),
                WindupHandle = SystemAPI.GetComponentTypeHandle<AbilityWindup>(),
                CooldownHandle = SystemAPI.GetComponentTypeHandle<AbilityCooldown>(),
                StartedEventHandle = SystemAPI.GetComponentTypeHandle<AbilityStartedEvent>(),
                ExecutedEventHandle = SystemAPI.GetComponentTypeHandle<AbilityExecutedEvent>(),
                HealthHandle = SystemAPI.GetComponentTypeHandle<Health>(true),
                AmmoHandle = SystemAPI.GetComponentTypeHandle<Ammo>(),
                AbilitiesLookup = SystemAPI.GetComponentLookup<ArrowShotAbility>(true),
                DefinitionsLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true),
                CostsLookup = SystemAPI.GetComponentLookup<AmmoCost>(true),
                HealthLookup = SystemAPI.GetComponentLookup<Health>(true),
                WorldTransformsLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                PrefabTransformsLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
                SpawnPointsLookup = SystemAPI.GetComponentLookup<ProjectileSpawnPoint>(true),
                DamageLookup = SystemAPI.GetComponentLookup<Damage>(true)
            }.ScheduleParallel(_query, state.Dependency);
        }

        [BurstCompile]
        private struct ArrowShotJob : IJobChunk
        {
            [ReadOnly] public EntityTypeHandle EntityHandle;
            [ReadOnly] public ComponentTypeHandle<Health> HealthHandle;

            public ComponentTypeHandle<AbilityUseRequest> RequestHandle;
            public ComponentTypeHandle<AbilityWindup> WindupHandle;
            public ComponentTypeHandle<AbilityCooldown> CooldownHandle;
            public ComponentTypeHandle<AbilityStartedEvent> StartedEventHandle;
            public ComponentTypeHandle<AbilityExecutedEvent> ExecutedEventHandle;

            public ComponentTypeHandle<Ammo> AmmoHandle;

            [ReadOnly] public ComponentLookup<ArrowShotAbility> AbilitiesLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionsLookup;
            [ReadOnly] public ComponentLookup<AmmoCost> CostsLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> PrefabTransformsLookup;
            [ReadOnly] public ComponentLookup<ProjectileSpawnPoint> SpawnPointsLookup;
            [ReadOnly] public ComponentLookup<Damage> DamageLookup;

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

                bool hasAmmo = chunk.Has(ref AmmoHandle);

                NativeArray<Ammo> balances = hasAmmo
                    ? chunk.GetNativeArray(ref AmmoHandle)
                    : default;

                var enumerator = new ChunkEntityEnumerator(
                    useEnabledMask,
                    chunkEnabledMask,
                    chunk.Count);

                while (enumerator.NextEntityIndex(out int index))
                {
                    RefRW<AbilityUseRequest> requestRef = new RefRW<AbilityUseRequest>(requests, index);
                    RefRW<AbilityWindup> windupRef = new RefRW<AbilityWindup>(windups, index);
                    RefRW<AbilityCooldown> cooldownRef = new RefRW<AbilityCooldown>(cooldowns, index);
                    RefRW<AbilityStartedEvent> startedEventRef = new RefRW<AbilityStartedEvent>(startedEvents, index);
                    RefRW<AbilityExecutedEvent> executeEventRef = new RefRW<AbilityExecutedEvent>(executedEvents, index);
                    RefRW<Health> healthRef = new RefRW<Health>(healths, index);
                    
                    Ammo ammo = hasAmmo
                        ? balances[index]
                        : default;

                    ProcessEntity(
                        unfilteredChunkIndex,
                        actors[index],
                        ref requestRef.ValueRW,
                        requestMask.GetEnabledRefRW<AbilityUseRequest>(index),
                        ref windupRef.ValueRW,
                        windupMask.GetEnabledRefRW<AbilityWindup>(index),
                        ref cooldownRef.ValueRW,
                        cooldownMask.GetEnabledRefRW<AbilityCooldown>(index),
                        ref startedEventRef.ValueRW,
                        startedMask.GetEnabledRefRW<AbilityStartedEvent>(index),
                        ref executeEventRef.ValueRW,
                        executedMask.GetEnabledRefRW<AbilityExecutedEvent>(index),
                        in healthRef.ValueRW,
                        hasAmmo,
                        ref ammo);

                    if (hasAmmo)
                        balances[index] = ammo;
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
                bool hasAmmo,
                ref Ammo ammo)
            {
                if (AbilitiesLookup.TryGetRefRO(
                        request.Definition,
                        out RefRO<ArrowShotAbility> ability) == false)
                    return;

                if (request.CancelRequested
                    || health.IsDead()
                    || DefinitionsLookup.TryGetRefRO(
                        request.Definition,
                        out RefRO<AbilityDefinition> definition) == false)
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
                }

                int cost = CostsLookup.TryGetRefRO(
                    request.Definition,
                    out RefRO<AmmoCost> ammoCost)
                    ? ammoCost.ValueRO.Value
                    : 0;

                if (request.Preparing == false)
                {
                    bool enoughAmmo = cost == 0
                                      || (cost > 0
                                          && hasAmmo
                                          && ammo.Value >= cost);

                    bool canStart = AbilityUseCase.CanStartTargeted(
                        actor,
                        request.Target,
                        definition.ValueRO.Range,
                        cooldownEnabled.ValueRO,
                        ref HealthLookup,
                        ref WorldTransformsLookup);

                    if (enoughAmmo == false
                        || canStart == false
                        || CanCreateProjectile(
                            actor,
                            ability.ValueRO.ProjectilePrefab) == false)
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
                        request.Target,
                        out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead()
                    || (cost > 0 && hasAmmo == false)
                    || TryCreateProjectile(
                        sortKey,
                        actor,
                        request.Target,
                        ability.ValueRO.ProjectilePrefab,
                        out Entity projectile) == false)
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
                }

                SetDamage(
                    sortKey,
                    actor,
                    ability.ValueRO.ProjectilePrefab,
                    projectile);

                if (cost > 0)
                    ammo.Value -= cost;

                AbilityUseCase.NotifyExecuted(
                    request.Target,
                    request.Definition,
                    ref executedEvent,
                    executedEventEnabled);

                AbilityUseCase.Complete(
                    ref request, requestEnabled,
                    ref windup, windupEnabled,
                    ref cooldown, cooldownEnabled,
                    definition.ValueRO.Cooldown);
            }

            private bool CanCreateProjectile(Entity actor, Entity prefab)
            {
                return SpawnPointsLookup.TryGetRefRO(
                           actor,
                           out RefRO<ProjectileSpawnPoint> spawnPoint)
                       && WorldTransformsLookup.HasComponent(
                           spawnPoint.ValueRO.Value)
                       && PrefabTransformsLookup.HasComponent(prefab);
            }

            private bool TryCreateProjectile(
                int sortKey,
                Entity actor,
                Entity target,
                Entity prefab,
                out Entity projectile)
            {
                projectile = Entity.Null;

                if (SpawnPointsLookup.TryGetRefRO(
                        actor,
                        out RefRO<ProjectileSpawnPoint> spawnPoint) == false
                    || WorldTransformsLookup.TryGetRefRO(
                        spawnPoint.ValueRO.Value,
                        out RefRO<LocalToWorld> muzzle) == false
                    || WorldTransformsLookup.TryGetRefRO(
                        target,
                        out RefRO<LocalToWorld> aim) == false
                    || PrefabTransformsLookup.TryGetRefRO(
                        prefab,
                        out RefRO<LocalTransform> prefabTransform) == false)
                {
                    return false;
                }

                projectile = ProjectileUseCase.Create(
                    ref Commands,
                    sortKey,
                    prefab,
                    target,
                    in muzzle.ValueRO,
                    in aim.ValueRO,
                    in prefabTransform.ValueRO);

                return true;
            }

            private void SetDamage(
                int sortKey,
                Entity actor,
                Entity prefab,
                Entity projectile)
            {
                if (DamageLookup.TryGetRefRO(
                        prefab,
                        out RefRO<Damage> projectileDamage) == false)
                    return;

                float damage = DamageUseCase.Calculate(
                    projectileDamage.ValueRO.Value,
                    actor,
                    ref DamageLookup);

                Commands.SetComponent(
                    sortKey,
                    projectile,
                    new Damage
                    {
                        Value = damage
                    });
            }
        }
    }
}