using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class DeathAuthoring : MonoBehaviour
    {
        [SerializeField] private float _deathDelay;
        
        private sealed class Baker : Baker<DeathAuthoring>
        {
            public override void Bake(DeathAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new DeathCooldown
                {
                    Duration = authoring._deathDelay
                });
                
                SetComponentEnabled<DeathCooldown>(entity, false);
                
                AddComponent(entity, new DeathEvent());
                SetComponentEnabled<DeathEvent>(entity, false);
            }
        }
    }
}