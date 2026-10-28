using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    public partial struct DeathCooldownSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter commands = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged)
                .AsParallelWriter();
            
            state.Dependency = new UpdateDeathCooldownJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
                Commands = commands
            }.ScheduleParallel(state.Dependency);

            // float deltaTime = SystemAPI.Time.DeltaTime;
            //
            // EntityCommandBuffer ecb = SystemAPI
            //     .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
            //     .CreateCommandBuffer(state.WorldUnmanaged);
            //
            // foreach ((RefRW<DeathCooldown> cooldown, Entity entity)
            //          in SystemAPI.Query<RefRW<DeathCooldown>>().WithEntityAccess())
            // {
            //     cooldown.ValueRW.Time -= deltaTime;
            //
            //     if (cooldown.ValueRO.Time <= 0)
            //         ecb.DestroyEntity(entity);
            // }
        }
        
        [BurstCompile]
        private partial struct UpdateDeathCooldownJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter Commands;
            
            private void Execute(
                Entity entity,
                [ChunkIndexInQuery] int sortKey,
                ref DeathCooldown cooldown)
            {
                cooldown.Time -= DeltaTime;
                
                if (cooldown.Time <= 0)
                    Commands.DestroyEntity(sortKey, entity);
            }
        }
    }
}