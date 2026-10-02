using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class MovementAuthoring : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;

        private sealed class Baker : Baker<MovementAuthoring>
        {
            public override void Bake(MovementAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MovementSpeed {Value = authoring._movementSpeed});
                AddComponent(entity, new MovementRequest());
                AddComponent(entity, new MovementEvent());
            }
        }
    }
}