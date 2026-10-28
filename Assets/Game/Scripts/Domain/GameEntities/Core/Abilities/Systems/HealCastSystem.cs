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
    public partial struct HealCastSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new HealCastJob
            {
                AmountsLookup = SystemAPI
                    .GetComponentLookup<HealAmount>(true),

                DefinitionsLookup = SystemAPI
                    .GetComponentLookup<AbilityDefinition>(true),

                CostsLookup = SystemAPI
                    .GetComponentLookup<ManaCost>(true),

                ManaLookup = SystemAPI.GetComponentLookup<Mana>(),

                HealthLookup = SystemAPI
                    .GetComponentLookup<Health>(true),

                MaxHealthLookup = SystemAPI
                    .GetComponentLookup<MaxHealth>(true),

                WorldTransformsLookup = SystemAPI
                    .GetComponentLookup<LocalToWorld>(true),

                HealRequestsLookup = SystemAPI.GetBufferLookup<HealRequest>(),

                StartedEventsLookup = SystemAPI
                    .GetComponentLookup<AbilityStartedEvent>(),

                ExecutedEventsLookup = SystemAPI
                    .GetComponentLookup<AbilityExecutedEvent>()
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        [WithPresent(typeof(AbilityWindup), typeof(AbilityCooldown))]
        private partial struct HealCastJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<HealAmount> AmountsLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionsLookup;
            [ReadOnly] public ComponentLookup<ManaCost> CostsLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<MaxHealth> MaxHealthLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;

            public ComponentLookup<Mana> ManaLookup;
            public BufferLookup<HealRequest> HealRequestsLookup;

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
                            && ManaLookup.TryGetRefRO(
                                actor,
                                out RefRO<Mana> balance)
                            && balance.ValueRO.Value >= cost);

                    if (enoughMana == false
                        || amount.ValueRO.Value > 0f == false
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
                        actor,
                        request.Definition,
                        ref StartedEventsLookup);
                }

                if (windupEnabled.ValueRO)
                    return;

                if (IsWounded(target) == false
                    || HealRequestsLookup.TryGetBuffer(
                        target,
                        out DynamicBuffer<HealRequest> requests) == false)
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
                }

                RefRW<Mana> mana = default;

                if (cost > 0f
                    && ManaLookup.TryGetRefRW(actor, out mana) == false)
                {
                    AbilityUseCase.Finish(
                        ref request, requestEnabled,
                        ref windup, windupEnabled);

                    return;
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
                    request.Definition,
                    ref ExecutedEventsLookup);

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