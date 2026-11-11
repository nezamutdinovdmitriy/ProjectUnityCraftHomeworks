using System;
using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial struct AbilityTargetRequestSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new GenerateAbilityRequestJob()
                .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        [WithAll(typeof(AIControlled))]
        [WithDisabled(typeof(AbilityUseRequest), typeof(AbilityCooldown))]
        private partial struct GenerateAbilityRequestJob : IJobEntity
        {
            private void Execute(
                in Health health,
                in SelectedAbility selectedAbility,
                in TargetEntity target,
                ref AbilityUseRequest request,
                EnabledRefRW<AbilityUseRequest> requestEnabled)
            {
                if (health.IsDead())
                    return;

                Entity definition = selectedAbility.Value;
                Entity targetEntity = target.Value;

                if (definition == Entity.Null
                    || targetEntity == Entity.Null)
                    return;

                request = new AbilityUseRequest
                {
                    Definition = definition,
                    Target = targetEntity,
                    Preparing = false,
                    CancelRequested = false
                };

                requestEnabled.ValueRW = true;
            }
        }
    }
}