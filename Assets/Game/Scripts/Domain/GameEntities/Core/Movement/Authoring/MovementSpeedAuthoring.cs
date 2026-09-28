using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class MovementSpeedAuthoring : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;

        private sealed class Baker : Baker<MovementSpeedAuthoring>
        {
            public override void Bake(MovementSpeedAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MovementSpeed {Value = authoring._movementSpeed});
            }
        }
    }
}