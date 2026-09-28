using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class AttackCooldownAuthoring : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown;
        
        private class Baker : Baker<AttackCooldownAuthoring>
        {
            public override void Bake(AttackCooldownAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new AttackCooldown
                {
                    Duration = authoring._attackCooldown
                });
            }
        }
    }
}