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
            state.RequireForUpdate<GameContext>();
        }

        public void OnUpdate(ref SystemState state)
        {
            GameResult result = SystemAPI.GetSingleton<GameContext>().Result;

            if (result == GameResult.None)
                return;
            
            Debug.Log(result == GameResult.Victory ? "Game Win" : "Game Over");

            state.Enabled = false;
        }
    }
}