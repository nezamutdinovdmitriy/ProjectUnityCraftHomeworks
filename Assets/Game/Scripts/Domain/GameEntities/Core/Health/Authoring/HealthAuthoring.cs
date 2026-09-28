using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class HealthAuthoring : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;

        private class Baker : Baker<HealthAuthoring>
        {
            public override void Bake(HealthAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Health {Value = authoring._maxHealth});
                AddComponent(entity, new MaxHealth {Value = authoring._maxHealth});
            }
        }
    }
}