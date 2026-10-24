using Game.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems

{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    [BurstCompile]
    public partial struct GameTimeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float frameDeltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (gameContext, gameTime) 
                     in SystemAPI.Query<RefRO<GameContext>, RefRW<GameTime>>())
            {
                gameTime.ValueRW.DeltaTime = gameContext.ValueRO.Result == GameResult.None
                    ? frameDeltaTime
                    : 0f;
            }
        }
    }
}