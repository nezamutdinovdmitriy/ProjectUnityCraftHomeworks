using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class AttackDistanceAuthoring : MonoBehaviour
    {
        [SerializeField] private float _attackDistance;
        
        private class Baker : Baker<AttackDistanceAuthoring>
        {
            public override void Bake(AttackDistanceAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new AttackDistance {Value = authoring._attackDistance});
            }
        }
    }
}