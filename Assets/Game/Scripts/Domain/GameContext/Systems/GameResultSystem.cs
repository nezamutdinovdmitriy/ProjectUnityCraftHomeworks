using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [BurstCompile]
    public partial struct GameResultSystem : ISystem
    {
        private EntityQuery _localPlayerQuery;
        
        public void OnCreate(ref SystemState state)
        {
            _localPlayerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, LocalPlayer, Team>()
                .Build(ref state);
            
            state.RequireForUpdate(_localPlayerQuery);
            state.RequireForUpdate<GameContext>();
            state.RequireForUpdate<GameTime>();
        }

        public void OnUpdate(ref SystemState state)
        {
            RefRW<GameContext> context = SystemAPI.GetSingletonRW<GameContext>();

            if (context.ValueRO.Result != GameResult.None)
                return;

            TeamType localTeam = _localPlayerQuery.GetSingleton<Team>().Value;

            GameResult result = GameResult.None;

            foreach (var (health, team) 
                     in SystemAPI.Query<RefRO<Health>, RefRO<Team>>()
                         .WithAll<Base>())
            {
                if(health.ValueRO.IsAlive())
                    continue;

                if (team.ValueRO.Value == localTeam)
                {
                    result = GameResult.Defeat;
                    break;
                }

                result = GameResult.Victory;
            }

            if (result == GameResult.None)
                return;

            context.ValueRW.Result = result;

            SystemAPI.GetSingletonRW<GameTime>().ValueRW.DeltaTime = 0f;
        }
    }
}