using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class BaseAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<BaseAuthoring>
        {
            public override void Bake(BaseAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Base());
            }
        }
    }
}