using Game.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [UpdateAfter(typeof(GameOverSystem))]
    [BurstCompile]
    public partial struct GameCycleSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (
                         request,
                         gameState,
                         gameDeltaTime,
                         entity)
                     in SystemAPI.Query<RefRO<GameOverRequest>,
                         RefRW<GameState>,
                         RefRW<GameDeltaTime>>()
                         .WithEntityAccess())
            {
                gameState.ValueRW.Value = request.ValueRO.Value;
                gameDeltaTime.ValueRW.DeltaTime = 0f;
                
                SystemAPI.SetComponentEnabled<GameOverRequest>(entity, false);
            }
        }
    }
}