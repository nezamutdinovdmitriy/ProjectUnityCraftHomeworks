using System;
using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    [BurstCompile]
    public partial struct AbilityTargetRequestSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (
                         health,
                         selectedAbility,
                         target,
                         request,
                         requestEnabled)
                     in SystemAPI.Query<
                             RefRO<Health>,
                             RefRO<SelectedAbility>,
                             RefRO<TargetEntity>,
                             RefRW<AbilityUseRequest>,
                             EnabledRefRW<AbilityUseRequest>>()
                         .WithAll<AIControlled>()
                         .WithDisabled<AbilityUseRequest, AbilityCooldown>())
            {
                if(health.ValueRO.IsDead())
                    continue;

                Entity definition = selectedAbility.ValueRO.Value;
                Entity targetEntity = target.ValueRO.Value;
                
                if(definition == Entity.Null || targetEntity == Entity.Null)
                    continue;

                request.ValueRW = new AbilityUseRequest
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