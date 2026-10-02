using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public sealed class SwordmanAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<SwordmanAuthoring>
        {
            public override void Bake(SwordmanAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Swordman());
            }
        }
    }
}