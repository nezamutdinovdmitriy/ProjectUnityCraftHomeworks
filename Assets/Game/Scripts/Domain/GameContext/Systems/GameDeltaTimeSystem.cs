using Game.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems

{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    [BurstCompile]
    public partial struct GameDeltaTimeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float frameDeltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (gameContext, gameTime) 
                     in SystemAPI.Query<RefRO<GameState>, RefRW<GameDeltaTime>>())
            {
                gameTime.ValueRW.DeltaTime = gameContext.ValueRO.Value == GameStateType.Playing
                    ? frameDeltaTime
                    : 0f;
            }
        }
    }
}