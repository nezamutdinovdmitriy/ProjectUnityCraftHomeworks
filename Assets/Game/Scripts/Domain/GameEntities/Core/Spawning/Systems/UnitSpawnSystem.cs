using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct UnitSpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UnitSpawnRequest>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commands = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (
                         spawns,
                         points,
                         random,
                         team)
                     in SystemAPI.Query<DynamicBuffer<UnitSpawnRequest>,
                             DynamicBuffer<UnitSpawnPoint>,
                             RefRW<UnitSpawnRandom>,
                             RefRO<Team>>()
                         .WithAll<Base>())
            {
                if (spawns.IsEmpty)
                    continue;

                for (int i = 0; i < spawns.Length; i++)
                {
                    Entity prefab = spawns[i].Prefab;
                    int pointIndex = random.ValueRW.Generator.NextInt(points.Length);
                    Entity point = points[pointIndex].Value;

                    LocalToWorld pointTransform = SystemAPI.GetComponent<LocalToWorld>(point);
                    LocalTransform prefabTransform = SystemAPI.GetComponent<LocalTransform>(prefab);
                    LocalTransform transform = UnitSpawnUseCase.GetTransform(
                        in pointTransform, 
                        in prefabTransform);

                    UnitSpawnUseCase.Create(
                        ref commands,
                        prefab,
                        in transform,
                        team.ValueRO.Value);
                }

                spawns.Clear();
            }
        }
    }
}