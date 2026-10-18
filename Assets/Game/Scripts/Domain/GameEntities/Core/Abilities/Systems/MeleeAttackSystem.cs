using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    [BurstCompile]
    public partial struct MeleeAttackSystem : ISystem
    {
        private ComponentLookup<MeleeAttackAbility> _abilitiesLookup;
        private ComponentLookup<AbilityDefinition> _definitionsLookup;
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<LocalToWorld> _worldTransformLookup;
        private ComponentLookup<Damage> _damageLookup;
        private BufferLookup<TakeDamageRequest> _damageRequestLookup;
        private ComponentLookup<AbilityStartedEvent> _startedEventLookup;
        private ComponentLookup<AbilityExecutedEvent> _hitEventLookup;

        public void OnCreate(ref SystemState state)
        {
            _abilitiesLookup = SystemAPI.GetComponentLookup<MeleeAttackAbility>(true);
            _definitionsLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true);
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            _worldTransformLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
            _damageLookup = SystemAPI.GetComponentLookup<Damage>(true);
            _damageRequestLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>();
            _startedEventLookup = SystemAPI.GetComponentLookup<AbilityStartedEvent>();
            _hitEventLookup = SystemAPI.GetComponentLookup<AbilityExecutedEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.CompleteDependency();

            _abilitiesLookup.Update(ref state);
            _definitionsLookup.Update(ref state);
            _healthLookup.Update(ref state);
            _worldTransformLookup.Update(ref state);
            _damageLookup.Update(ref state);
            _damageRequestLookup.Update(ref state);
            _startedEventLookup.Update(ref state);
            _hitEventLookup.Update(ref state);

            foreach (var (
                         request, requestEnabled,
                         windup, windupEnabled,
                         cooldown, cooldownEnabled,
                         health,
                         actor) in
                     SystemAPI.Query<
                             RefRW<AbilityUseRequest>, EnabledRefRW<AbilityUseRequest>,
                             RefRW<AbilityWindup>, EnabledRefRW<AbilityWindup>,
                             RefRW<AbilityCooldown>, EnabledRefRW<AbilityCooldown>,
                             RefRO<Health>>()
                         .WithPresent<AbilityWindup, AbilityCooldown>()
                         .WithEntityAccess())
            {
                Entity definitionEntity = request.ValueRO.Definition;
                
                if (_abilitiesLookup.TryGetRefRO(
                        definitionEntity,
                        out RefRO<MeleeAttackAbility> ability) == false)
                    continue;

                if (request.ValueRO.CancelRequested
                    || health.ValueRO.IsDead()
                    || _definitionsLookup.TryGetRefRO(
                        definitionEntity,
                        out RefRO<AbilityDefinition> definition) == false)
                {
                    AbilityUseCase.Finish(ref request.ValueRW, requestEnabled, ref windup.ValueRW, windupEnabled);
                    continue;
                }

                Entity target = request.ValueRO.Target;

                if (request.ValueRO.Preparing == false)
                {
                    bool canStart = AbilityUseCase.CanStartTargeted(
                        in actor, 
                        in target,
                        definition.ValueRO.Range,
                        cooldownEnabled.ValueRO,
                        ref _healthLookup,
                        ref _worldTransformLookup);
                    
                    if (cooldownEnabled.ValueRO || canStart == false)
                    {
                        AbilityUseCase.Finish(ref request.ValueRW, requestEnabled, ref windup.ValueRW, windupEnabled);
                        continue;
                    }

                    AbilityUseCase.Begin(
                        ref request.ValueRW,
                        ref windup.ValueRW,
                        windupEnabled,
                        definition.ValueRO.Windup);

                    AbilityUseCase.NotifyStarted(actor, ref _startedEventLookup);
                }

                if (windupEnabled.ValueRO)
                    continue;
                
                if (_healthLookup.TryGetRefRO(
                        target, out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead()
                    || _damageRequestLookup.TryGetBuffer(
                        target, out DynamicBuffer<TakeDamageRequest> requests) == false)
                {
                    AbilityUseCase.Finish(ref request.ValueRW, requestEnabled, ref windup.ValueRW, windupEnabled);
                    continue;
                }

                float damage = DamageUseCase.Calculate(ability.ValueRO.Damage, actor, ref _damageLookup);

                requests.Add(new TakeDamageRequest
                {
                    Damage = damage
                });
                
                AbilityUseCase.NotifyExecuted(actor, target, ref _hitEventLookup);

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
    }
}