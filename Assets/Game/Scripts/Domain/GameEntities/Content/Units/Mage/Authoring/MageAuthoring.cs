using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class MageAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<MageAuthoring>
        {
            public override void Bake(MageAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Mage());
            }
        }
    }
}