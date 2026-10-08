using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class RotationSpeedAuthoring : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        private sealed class Baker : Baker<RotationSpeedAuthoring>
        {
            public override void Bake(RotationSpeedAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new RotationSpeed {Value = authoring._rotationSpeed});
            }
        }
    }
}