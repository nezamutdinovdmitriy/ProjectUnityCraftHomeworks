using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class GameContextAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<GameContextAuthoring>
        {
            public override void Bake(GameContextAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new GameContext
                {
                    Result = GameResult.None
                });
                
                AddComponent(entity, new GameTime());
            }
        }
    }
}