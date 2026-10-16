using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    [BurstCompile]
    public partial struct ArrowShotSystem : ISystem
    {
        private ComponentLookup<ArrowShotAbility> _abilitiesLookup;
        private ComponentLookup<AbilityDefinition> _definitionsLookup;
        private ComponentLookup<AmmoCost> _costsLookup;
        private ComponentLookup<Ammo> _ammoLookup;
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<LocalToWorld> _worldTransformLookup;
        private ComponentLookup<LocalTransform> _localTransformLookup;
        private ComponentLookup<ProjectileSpawnPoint> _spawnPointLookup;
        private ComponentLookup<Damage> _damageLookup;
        private ComponentLookup<AttackStartedEvent> _startedEventLookup;
        private ComponentLookup<AttackHitEvent> _releasedEventLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();

            _abilitiesLookup = SystemAPI.GetComponentLookup<ArrowShotAbility>(true);
            _definitionsLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true);
            _costsLookup = SystemAPI.GetComponentLookup<AmmoCost>(true);
            _ammoLookup = SystemAPI.GetComponentLookup<Ammo>();
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            _worldTransformLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
            _localTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            _spawnPointLookup = SystemAPI.GetComponentLookup<ProjectileSpawnPoint>(true);
            _damageLookup = SystemAPI.GetComponentLookup<Damage>(true);
            _startedEventLookup = SystemAPI.GetComponentLookup<AttackStartedEvent>();
            _releasedEventLookup = SystemAPI.GetComponentLookup<AttackHitEvent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.CompleteDependency();

            _damageLookup.Update(ref state);
            _spawnPointLookup.Update(ref state);
            _localTransformLookup.Update(ref state);
            _worldTransformLookup.Update(ref state);
            _healthLookup.Update(ref state);
            _ammoLookup.Update(ref state);
            _costsLookup.Update(ref state);
            _definitionsLookup.Update(ref state);
            _abilitiesLookup.Update(ref state);
            _releasedEventLookup.Update(ref state);
            _startedEventLookup.Update(ref state);

            EntityCommandBuffer commands = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (
                         request, requestEnabled,
                         windup, windupEnabled,
                         cooldown, cooldownEnabled,
                         health,
                         actor)
                     in SystemAPI.Query<
                             RefRW<AbilityUseRequest>, EnabledRefRW<AbilityUseRequest>,
                             RefRW<AbilityWindup>, EnabledRefRW<AbilityWindup>,
                             RefRW<AbilityCooldown>, EnabledRefRW<AbilityCooldown>,
                             RefRO<Health>>()
                         .WithPresent<AbilityWindup, AbilityCooldown>()
                         .WithEntityAccess())
            {
                if (_abilitiesLookup.TryGetRefRO(
                        request.ValueRO.Definition,
                        out RefRO<ArrowShotAbility> ability) == false)
                    continue;

                if (request.ValueRO.CancelRequested
                    || health.ValueRO.IsDead()
                    || _definitionsLookup.TryGetRefRO(
                        request.ValueRO.Definition,
                        out RefRO<AbilityDefinition> definition) == false)
                {
                    AbilityUseCase.Finish(ref request.ValueRW, requestEnabled, ref windup.ValueRW, windupEnabled);
                    continue;
                }

                int cost = _costsLookup.TryGetRefRO(request.ValueRO.Definition, out var ammoCost)
                    ? ammoCost.ValueRO.Value
                    : 0;

                if (request.ValueRO.Preparing == false)
                {
                    bool enoughAmmo = cost == 0
                                      || (cost > 0
                                          && _ammoLookup.TryGetRefRO(actor, out var balance)
                                          && balance.ValueRO.Value >= cost);

                    bool canStart = AbilityUseCase.CanStartTargeted(
                        in actor,
                        in request.ValueRO.Target,
                        definition.ValueRO.Range,
                        cooldownEnabled.ValueRO,
                        ref _healthLookup,
                        ref _worldTransformLookup);

                    if (cooldownEnabled.ValueRO || enoughAmmo == false || canStart == false
                        || CanCreateProjectile(actor, ability.ValueRO.ProjectilePrefab) == false)
                    {
                        AbilityUseCase.Finish(ref request.ValueRW, requestEnabled, ref windup.ValueRW, windupEnabled);
                        continue;
                    }

                    AbilityUseCase.Begin(
                        ref request.ValueRW,
                        ref windup.ValueRW,
                        windupEnabled,
                        definition.ValueRO.Windup);

                    AttackUseCase.NotifyStarted(actor, ref _startedEventLookup);
                }

                if (windupEnabled.ValueRO)
                    continue;

                if (_healthLookup.TryGetRefRO(
                        request.ValueRO.Target,
                        out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead())
                {
                    AbilityUseCase.Finish(ref request.ValueRW, requestEnabled, ref windup.ValueRW, windupEnabled);
                    continue;
                }

                RefRW<Ammo> ammo = default;

                if (cost > 0
                    && _ammoLookup.TryGetRefRW(actor, out ammo) == false
                    || TryCreateProjectile(
                        ref commands,
                        actor,
                        request.ValueRO.Target,
                        ability.ValueRO.ProjectilePrefab,
                        out Entity projectile) == false)
                {
                    AbilityUseCase.Finish(ref request.ValueRW, requestEnabled, ref windup.ValueRW, windupEnabled);
                    continue;
                }

                SetDamage(ref commands, actor, ability.ValueRO.ProjectilePrefab, projectile);

                if (cost > 0)
                    ammo.ValueRW.Value -= cost;
                
                AttackUseCase.NotifyHit(actor, request.ValueRO.Target, ref _releasedEventLookup);

                AbilityUseCase.Complete(
                    ref request.ValueRW,
                    requestEnabled,
                    ref windup.ValueRW,
                    windupEnabled,
                    ref cooldown.ValueRW,
                    cooldownEnabled,
                    definition.ValueRO.Cooldown);
            }
        }

        private bool CanCreateProjectile(Entity actor, Entity prefab)
        {
            return _spawnPointLookup.TryGetRefRO(actor, out var spawnPoint)
                   && _worldTransformLookup.HasComponent(spawnPoint.ValueRO.Value)
                   && _localTransformLookup.HasComponent(prefab);
        }

        private bool TryCreateProjectile(
            ref EntityCommandBuffer commands,
            Entity actor,
            Entity target,
            Entity prefab,
            out Entity projectile)
        {
            projectile = Entity.Null;

            if (_spawnPointLookup.TryGetRefRO(
                    actor, out RefRO<ProjectileSpawnPoint> spawnPoint) == false
                || _worldTransformLookup.TryGetRefRO(
                    spawnPoint.ValueRO.Value, out RefRO<LocalToWorld> muzzle) == false
                || _worldTransformLookup.TryGetRefRO(
                    target, out RefRO<LocalToWorld> aim) == false
                || _localTransformLookup.TryGetRefRO(
                    prefab, out RefRO<LocalTransform> prefabTransform) == false)
                return false;

            projectile = ProjectileUseCase.Create(
                ref commands,
                prefab,
                target,
                in muzzle.ValueRO,
                in aim.ValueRO,
                prefabTransform.ValueRO);

            return true;
        }

        private void SetDamage(ref EntityCommandBuffer commands, Entity actor, Entity prefab, Entity projectile)
        {
            if (_damageLookup.TryGetRefRO(prefab, out RefRO<Damage> projectileDamage) == false)
                return;

            float damage = AttackUseCase.CalculateDamage(projectileDamage.ValueRO.Value, actor, ref _damageLookup);

            commands.SetComponent(projectile, new Damage
            {
                Value = damage
            });
        }
    }
}