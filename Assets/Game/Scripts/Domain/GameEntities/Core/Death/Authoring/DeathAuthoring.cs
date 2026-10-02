using Unity.Entities;
using UnityEngine;

namespace Game.Scripts.Domain.GameEntities.Core.Death.Authoring
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
                
                AddComponent(entity, new DeathEvent());
            }
        }
    }
}