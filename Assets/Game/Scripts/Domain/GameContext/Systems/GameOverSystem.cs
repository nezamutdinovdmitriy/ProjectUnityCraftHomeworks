using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [BurstCompile]
    public partial struct GameOverSystem : ISystem
    {
        private EntityQuery _localPlayerQuery;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameState>();
            state.RequireForUpdate<GameDeltaTime>();
            state.RequireForUpdate<GameOverRequest>();

            _localPlayerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, LocalPlayer, Team>()
                .Build(ref state);
        }
        
        public void OnUpdate(ref SystemState state)
        {
            GameStateType gameStateType = SystemAPI.GetSingleton<GameState>().Value;
            
            if (gameStateType != GameStateType.Playing)
                return;

            GameStateType nextState = GameStateType.Playing;
            
            TeamType localTeam = _localPlayerQuery.GetSingleton<Team>().Value;
            
            foreach (var (health, team) 
                     in SystemAPI.Query<RefRO<Health>, RefRO<Team>>()
                         .WithAll<Base>())
            {
                if(health.ValueRO.IsAlive())
                    continue;

                if (team.ValueRO.Value == localTeam)
                {
                    nextState = GameStateType.Defeat;
                    break;
                }

                nextState = GameStateType.Victory;
            }

            if (nextState == GameStateType.Playing)
                return;
            
            Entity gameContext = SystemAPI.GetSingletonEntity<GameState>();
            
            SystemAPI.SetComponent(gameContext, new GameOverRequest
            {
                Value = nextState
            });
            
            SystemAPI.SetComponentEnabled<GameOverRequest>(gameContext, true);
        }
    }
}