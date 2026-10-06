using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public sealed class TargetEntityAuthoring : MonoBehaviour
    {
        private sealed class Baker: Baker<TargetEntityAuthoring>
        {
            public override void Bake(TargetEntityAuthoring entityAuthoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new TargetEntity());
            }
        }
    }
}