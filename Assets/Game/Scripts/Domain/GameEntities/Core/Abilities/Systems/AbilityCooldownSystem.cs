using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    public partial struct AbilityCooldownSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateCooldownJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct UpdateCooldownJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                ref AbilityCooldown cooldown,
                EnabledRefRW<AbilityCooldown> enabled)
            {
                cooldown.Time -= DeltaTime;

                if (cooldown.Time <= 0)
                    enabled.ValueRW = false;
            }
        }
    }
}