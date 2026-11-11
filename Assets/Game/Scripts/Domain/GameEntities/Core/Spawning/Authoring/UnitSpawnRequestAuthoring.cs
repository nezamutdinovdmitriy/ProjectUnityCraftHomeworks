using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class UnitSpawnRequestAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<UnitSpawnRequestAuthoring>
        {
            public override void Bake(UnitSpawnRequestAuthoring authoring)
            {
                AddBuffer<UnitSpawnRequest>(GetEntity(TransformUsageFlags.None));
            }
        }
    }
}