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
    public partial struct HealCastSystem : ISystem
    {
        private ComponentLookup<HealAmount> _amountLookup;
        private ComponentLookup<AbilityDefinition> _definitionsLookup;
        private ComponentLookup<ManaCost> _costsLookup;
        private ComponentLookup<Mana> _manaLookup;
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<MaxHealth> _maxHealthLookup;
        private ComponentLookup<LocalToWorld> _worldTransformLookup;
        private BufferLookup<HealRequest> _healRequestLookup;
        private ComponentLookup<AbilityStartedEvent> _startedEventLookup;
        private ComponentLookup<AbilityExecutedEvent> _executedEventLookup;

        public void OnCreate(ref SystemState state)
        {
            _amountLookup = SystemAPI.GetComponentLookup<HealAmount>(true);
            _definitionsLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true);
            _costsLookup = SystemAPI.GetComponentLookup<ManaCost>(true);
            _manaLookup = SystemAPI.GetComponentLookup<Mana>();
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            _maxHealthLookup = SystemAPI.GetComponentLookup<MaxHealth>(true);
            _worldTransformLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
            _healRequestLookup = SystemAPI.GetBufferLookup<HealRequest>();
            _startedEventLookup = SystemAPI.GetComponentLookup<AbilityStartedEvent>();
            _executedEventLookup = SystemAPI.GetComponentLookup<AbilityExecutedEvent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.CompleteDependency();

            _amountLookup.Update(ref state);
            _definitionsLookup.Update(ref state);
            _costsLookup.Update(ref state);
            _manaLookup.Update(ref state);
            _healthLookup.Update(ref state);
            _maxHealthLookup.Update(ref state);
            _worldTransformLookup.Update(ref state);
            _healRequestLookup.Update(ref state);
            _startedEventLookup.Update(ref state);
            _executedEventLookup.Update(ref state);

            foreach (var (request, requestEnabled,
                         windup, windupEnabled,
                         cooldown, cooldownEnabled,
                         health,
                         actor) in SystemAPI.Query<
                             RefRW<AbilityUseRequest>, EnabledRefRW<AbilityUseRequest>,
                             RefRW<AbilityWindup>, EnabledRefRW<AbilityWindup>,
                             RefRW<AbilityCooldown>, EnabledRefRW<AbilityCooldown>,
                             RefRO<Health>>()
                         .WithPresent<AbilityWindup, AbilityCooldown>()
                         .WithEntityAccess())
            {
                Entity definitionEntity = request.ValueRO.Definition;

                if (_amountLookup.TryGetRefRO(
                        definitionEntity, out RefRO<HealAmount> amount) == false)
                    continue;

                if (request.ValueRO.CancelRequested
                    || health.ValueRO.IsDead()
                    || _definitionsLookup.TryGetRefRO(
                        definitionEntity, out RefRO<AbilityDefinition> definition) == false)
                {
                    AbilityUseCase.Finish(
                        ref request.ValueRW, requestEnabled,
                        ref windup.ValueRW, windupEnabled);
                    continue;
                }

                Entity target = request.ValueRO.Target;

                float cost = _costsLookup.TryGetRefRO(definitionEntity, out RefRO<ManaCost> manaCost)
                    ? manaCost.ValueRO.Value
                    : 0f;

                if (request.ValueRO.Preparing == false)
                {
                    bool enoughMana = cost == 0f
                                      || (cost > 0f && _manaLookup.TryGetRefRO(actor, out RefRO<Mana> balance)
                                                    && balance.ValueRO.Value >= cost);

                    if (enoughMana == false
                        || amount.ValueRO.Value > 0f == false
                        || AbilityUseCase.CanStartTargeted(
                            actor,
                            target,
                            definition.ValueRO.Range,
                            cooldownEnabled.ValueRO,
                            ref _healthLookup,
                            ref _worldTransformLookup) == false
                        || IsWounded(target) == false
                        || _healRequestLookup.HasBuffer(target) == false)
                    {
                        AbilityUseCase.Finish(
                            ref request.ValueRW, requestEnabled,
                            ref windup.ValueRW, windupEnabled);
                        continue;
                    }

                    AbilityUseCase.Begin(
                        ref request.ValueRW,
                        ref windup.ValueRW, windupEnabled,
                        definition.ValueRO.Windup);

                    AbilityUseCase.NotifyStarted(
                        actor,
                        request.ValueRO.Definition,
                        ref _startedEventLookup);
                }

                if (windupEnabled.ValueRO)
                    continue;

                if (IsWounded(target) == false
                    || _healRequestLookup.TryGetBuffer(
                        target, out DynamicBuffer<HealRequest> requests) == false)
                {
                    AbilityUseCase.Finish(
                        ref request.ValueRW, requestEnabled,
                        ref windup.ValueRW, windupEnabled);
                    continue;
                }

                RefRW<Mana> mana = default;

                if (cost > 0f
                    && _manaLookup.TryGetRefRW(actor, out mana) == false)
                {
                    AbilityUseCase.Finish(
                        ref request.ValueRW, requestEnabled,
                        ref windup.ValueRW, windupEnabled);
                    continue;
                }

                requests.Add(new HealRequest
                {
                    Value = amount.ValueRO.Value
                });

                if (cost > 0f)
                    mana.ValueRW.Value -= cost;

                AbilityUseCase.NotifyExecuted(
                    actor,
                    target,
                    request.ValueRO.Definition,
                    ref _executedEventLookup);

                AbilityUseCase.Complete(
                    ref request.ValueRW, requestEnabled,
                    ref windup.ValueRW, windupEnabled,
                    ref cooldown.ValueRW, cooldownEnabled,
                    definition.ValueRO.Cooldown);
            }
        }

        private bool IsWounded(Entity target)
        {
            return _healthLookup.TryGetRefRO(target, out RefRO<Health> health)
                   && _maxHealthLookup.TryGetRefRO(target, out RefRO<MaxHealth> maxHealth)
                   && health.ValueRO.IsWounded(maxHealth.ValueRO);
        }
    }
}