using Unity.Entities;
using UnityEngine;

namespace Game.Scripts.Domain.GameContext
{
    public sealed class GameContextAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<GameContextAuthoring>
        {
            public override void Bake(GameContextAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new GameContext());
            }
        }
    }
}