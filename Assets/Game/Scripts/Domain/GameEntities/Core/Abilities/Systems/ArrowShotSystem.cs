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
    public partial struct ArrowShotSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commands = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            state.Dependency = new ArrowShotJob
            {
                Commands = commands,

                AbilitiesLookup =
                    SystemAPI.GetComponentLookup<ArrowShotAbility>(true),

                DefinitionsLookup =
                    SystemAPI.GetComponentLookup<AbilityDefinition>(true),

                CostsLookup =
                    SystemAPI.GetComponentLookup<AmmoCost>(true),

                AmmoLookup =
                    SystemAPI.GetComponentLookup<Ammo>(),

                HealthLookup =
                    SystemAPI.GetComponentLookup<Health>(true),

                WorldTransformsLookup =
                    SystemAPI.GetComponentLookup<LocalToWorld>(true),

                PrefabTransformsLookup =
                    SystemAPI.GetComponentLookup<LocalTransform>(true),

                SpawnPointsLookup =
                    SystemAPI.GetComponentLookup<ProjectileSpawnPoint>(true),

                DamageLookupLookup =
                    SystemAPI.GetComponentLookup<Damage>(true),

                StartedEventsLookup =
                    SystemAPI.GetComponentLookup<AbilityStartedEvent>(),

                ExecutedEventsLookup =
                    SystemAPI.GetComponentLookup<AbilityExecutedEvent>()
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        [WithPresent(typeof(AbilityWindup), typeof(AbilityCooldown))]
        private partial struct ArrowShotJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<ArrowShotAbility> AbilitiesLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionsLookup;
            [ReadOnly] public ComponentLookup<AmmoCost> CostsLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> PrefabTransformsLookup;
            [ReadOnly] public ComponentLookup<ProjectileSpawnPoint> SpawnPointsLookup;
            [ReadOnly] public ComponentLookup<Damage> DamageLookupLookup;

            public ComponentLookup<Ammo> AmmoLookup;

            public ComponentLookup<AbilityStartedEvent> StartedEventsLookup;
            public ComponentLookup<AbilityExecutedEvent> ExecutedEventsLookup;
            
            public EntityCommandBuffer Commands;

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
                            && AmmoLookup.TryGetRefRO(
                                actor,
                                out RefRO<Ammo> balance)
                            && balance.ValueRO.Value >= cost);

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
                        actor,
                        request.Definition,
                        ref StartedEventsLookup);
                }

                if (windupEnabled.ValueRO)
                    return;

                if (HealthLookup.TryGetRefRO(
                        request.Target,
                        out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead())
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
                }

                RefRW<Ammo> ammo = default;

                if ((cost > 0
                     && AmmoLookup.TryGetRefRW(actor, out ammo) == false)
                    || TryCreateProjectile(
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
                    actor,
                    ability.ValueRO.ProjectilePrefab,
                    projectile);

                if (cost > 0)
                    ammo.ValueRW.Value -= cost;

                AbilityUseCase.NotifyExecuted(
                    actor,
                    request.Target,
                    request.Definition,
                    ref ExecutedEventsLookup);

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
                    && WorldTransformsLookup.HasComponent(spawnPoint.ValueRO.Value)
                    && PrefabTransformsLookup.HasComponent(prefab);
            }

            private bool TryCreateProjectile(
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
                    return false;

                projectile = ProjectileUseCase.Create(
                    ref Commands,
                    prefab,
                    target,
                    in muzzle.ValueRO,
                    in aim.ValueRO,
                    in prefabTransform.ValueRO);

                return true;
            }

            private void SetDamage(Entity actor, Entity prefab, Entity projectile)
            {
                if (DamageLookupLookup.TryGetRefRO(
                        prefab,
                        out RefRO<Damage> projectileDamage) == false)
                    return;

                float damage = DamageUseCase.Calculate(
                    projectileDamage.ValueRO.Value,
                    actor,
                    ref DamageLookupLookup);

                Commands.SetComponent(projectile, new Damage
                {
                    Value = damage
                });
            }
        }
    }
}