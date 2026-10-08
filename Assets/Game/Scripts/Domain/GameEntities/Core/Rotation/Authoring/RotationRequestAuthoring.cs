using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class RotationRequestAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<RotationRequestAuthoring>
        {
            public override void Bake(RotationRequestAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RotationRequest());
            }
        }
    }
}