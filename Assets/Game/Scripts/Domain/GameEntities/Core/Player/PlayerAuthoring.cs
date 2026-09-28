using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class PlayerAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Player());
            }
        }
    }
}