using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ArrowReplenishmentCooldownAuthoring : MonoBehaviour
    {
        [SerializeField] private float _cooldown;
        
        private sealed class Baker : Baker<ArrowReplenishmentCooldownAuthoring>
        {
            public override void Bake(ArrowReplenishmentCooldownAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ArrowReplenishmentCooldown
                {
                    Duration = authoring._cooldown
                });
            }
        }
    }
}