using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    [BurstCompile]
    public partial struct DeathCooldownSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            EntityCommandBuffer ecb = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach ((RefRW<DeathCooldown> cooldown, Entity entity)
                     in SystemAPI.Query<RefRW<DeathCooldown>>().WithEntityAccess())
            {
                cooldown.ValueRW.Time -= deltaTime;

                if (cooldown.ValueRO.Time <= 0)
                    ecb.DestroyEntity(entity);
            }
        }
    }
}