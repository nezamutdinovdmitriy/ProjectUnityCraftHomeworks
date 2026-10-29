using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial struct ProjectileSystem : ISystem
    {
        private const float HitRadius = 0.25f;
        private EntityQuery _query;


        public void OnCreate(ref SystemState state)
        {
            _query = SystemAPI.QueryBuilder()
                .WithAll<Projectile, TargetEntity, LocalTransform>()
                .WithDisabledRW<DeathCooldown>()
                .WithPresentRW<MovementRequest, RotationRequest>()
                .WithPresentRW<DeathEvent>()
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

            state.Dependency = new UpdateProjectilesJob
            {
                Commands = commands,

                TargetHandle = SystemAPI.GetComponentTypeHandle<TargetEntity>(true),
                TransformHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>(true),
                MovementHandle = SystemAPI.GetComponentTypeHandle<MovementRequest>(),
                RotationHandle = SystemAPI.GetComponentTypeHandle<RotationRequest>(),
                DeathCooldownHandle = SystemAPI.GetComponentTypeHandle<DeathCooldown>(),
                DeathEventHandle = SystemAPI.GetComponentTypeHandle<DeathEvent>(),
                DamageHandle = SystemAPI.GetComponentTypeHandle<Damage>(true),

                HealthLookup = SystemAPI.GetComponentLookup<Health>(true),
                HitPointsLookup = SystemAPI.GetComponentLookup<TargetHitPoint>(true),
                WorldTransformsLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                DamageRequestsLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>(true)
            }.ScheduleParallel(_query, state.Dependency);
        }

        [BurstCompile]
        private struct UpdateProjectilesJob : IJobChunk
        {
            [ReadOnly] public ComponentTypeHandle<TargetEntity> TargetHandle;
            [ReadOnly] public ComponentTypeHandle<LocalTransform> TransformHandle;
            [ReadOnly] public ComponentTypeHandle<Damage> DamageHandle;

            public ComponentTypeHandle<MovementRequest> MovementHandle;
            public ComponentTypeHandle<RotationRequest> RotationHandle;
            public ComponentTypeHandle<DeathCooldown> DeathCooldownHandle;
            public ComponentTypeHandle<DeathEvent> DeathEventHandle;

            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<TargetHitPoint> HitPointsLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public BufferLookup<TakeDamageRequest> DamageRequestsLookup;

            public EntityCommandBuffer.ParallelWriter Commands;

            public void Execute(
                in ArchetypeChunk chunk,
                int unfilteredChunkIndex,
                bool useEnabledMask,
                in v128 chunkEnabledMask)
            {
                NativeArray<TargetEntity> targets = chunk.GetNativeArray(ref TargetHandle);
                NativeArray<LocalTransform> transforms = chunk.GetNativeArray(ref TransformHandle);
                NativeArray<MovementRequest> movements = chunk.GetNativeArray(ref MovementHandle);
                NativeArray<RotationRequest> rotations = chunk.GetNativeArray(ref RotationHandle);

                NativeArray<DeathCooldown> deathCooldowns = chunk.GetNativeArray(ref DeathCooldownHandle);
                EnabledMask movementMask = chunk.GetEnabledMask(ref MovementHandle);
                EnabledMask rotationMask = chunk.GetEnabledMask(ref RotationHandle);
                EnabledMask deathCooldownMask = chunk.GetEnabledMask(ref DeathCooldownHandle);
                EnabledMask deathEventMask = chunk.GetEnabledMask(ref DeathEventHandle);

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
                    RefRW<MovementRequest> movementRequestRef = new RefRW<MovementRequest>(movements, index);
                    RefRW<RotationRequest> rotationRequestRef = new RefRW<RotationRequest>(rotations, index);
                    RefRW<DeathCooldown> deathCooldownRef = new RefRW<DeathCooldown>(deathCooldowns, index);

                    float damage = hasDamage
                        ? damages[index].Value
                        : 0f;

                    ProcessProjectile(
                        unfilteredChunkIndex,
                        targets[index].Value,
                        transforms[index].Position,
                        hasDamage,
                        damage,
                        ref movementRequestRef.ValueRW,
                        movementMask.GetEnabledRefRW<MovementRequest>(index),
                        ref rotationRequestRef.ValueRW,
                        rotationMask.GetEnabledRefRW<RotationRequest>(index),
                        ref deathCooldownRef.ValueRW,
                        deathCooldownMask.GetEnabledRefRW<DeathCooldown>(index),
                        deathEventMask.GetEnabledRefRW<DeathEvent>(index));
                }
            }

            private void ProcessProjectile(
                int sortKey,
                Entity target,
                float3 position,
                bool hasDamage,
                float damage,
                ref MovementRequest movementRequest,
                EnabledRefRW<MovementRequest> movementRequestEnabled,
                ref RotationRequest rotationRequest,
                EnabledRefRW<RotationRequest> rotationRequestEnabled,
                ref DeathCooldown deathCooldown,
                EnabledRefRW<DeathCooldown> deathCooldownEnabled,
                EnabledRefRW<DeathEvent> deathEvent)
            {
                float3 direction = movementRequest.Direction;

                if (TryGetTargetPosition(target, out float3 targetPosition))
                {
                    float3 delta = targetPosition - position;

                    if (math.lengthsq(delta) <= HitRadius * HitRadius)
                    {
                        if (hasDamage && DamageRequestsLookup.HasBuffer(target))
                        {
                            Commands.AppendToBuffer(
                                sortKey,
                                target,
                                new TakeDamageRequest
                                {
                                    Damage = damage
                                });
                        }

                        movementRequestEnabled.ValueRW = false;
                        rotationRequestEnabled.ValueRW = false;

                        DeathUseCase.StartProcess(
                            ref deathCooldown,
                            deathCooldownEnabled,
                            deathEvent);

                        return;
                    }

                    direction = math.normalizesafe(
                        delta,
                        direction);
                }

                movementRequest.Direction = direction;
                movementRequestEnabled.ValueRW = true;

                rotationRequest.Direction = direction;
                rotationRequestEnabled.ValueRW = true;
            }

            private bool TryGetTargetPosition(Entity target, out float3 position)
            {
                position = default;

                if (HealthLookup.TryGetRefRO(
                        target,
                        out RefRO<Health> health) == false
                    || health.ValueRO.IsDead())
                    return false;

                if (HitPointsLookup.TryGetRefRO(
                        target,
                        out RefRO<TargetHitPoint> hitPoint)
                    && WorldTransformsLookup.TryGetRefRO(
                        hitPoint.ValueRO.Value,
                        out RefRO<LocalToWorld> hitTransform))
                {
                    position = hitTransform.ValueRO.Position;
                    return true;
                }

                if (WorldTransformsLookup.TryGetRefRO(
                        target,
                        out RefRO<LocalToWorld> targetTransform) == false)
                    return false;

                position = targetTransform.ValueRO.Position;
                return true;
            }
        }
    }
}