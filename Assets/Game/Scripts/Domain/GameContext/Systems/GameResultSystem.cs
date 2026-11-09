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
            state.RequireForUpdate<GameState>();
            state.RequireForUpdate<GameDeltaTime>();
        }

        public void OnUpdate(ref SystemState state)
        {
            RefRW<GameState> gameState = SystemAPI.GetSingletonRW<GameState>();

            if (gameState.ValueRO.Value != GameStateType.Playing)
                return;

            TeamType localTeam = _localPlayerQuery.GetSingleton<Team>().Value;

            GameStateType stateType = GameStateType.Playing;

            foreach (var (health, team) 
                     in SystemAPI.Query<RefRO<Health>, RefRO<Team>>()
                         .WithAll<Base>())
            {
                if(health.ValueRO.IsAlive())
                    continue;

                if (team.ValueRO.Value == localTeam)
                {
                    stateType = GameStateType.Defeat;
                    break;
                }

                stateType = GameStateType.Victory;
            }

            if (stateType == GameStateType.Playing)
                return;

            gameState.ValueRW.Value = stateType;

            SystemAPI.GetSingletonRW<GameDeltaTime>().ValueRW.DeltaTime = 0f;
        }
    }
}