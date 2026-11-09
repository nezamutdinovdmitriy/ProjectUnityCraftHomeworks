using Game.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct GameCompletionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            GameStateType stateType = SystemAPI.GetSingleton<GameState>().Value;

            if (stateType == GameStateType.Playing)
                return;
            
            Debug.Log(stateType == GameStateType.Victory ? "Game Win" : "Game Over");

            state.Enabled = false;
        }
    }
}