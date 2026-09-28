using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class DamageAuthoring : MonoBehaviour
    {
        [SerializeField] private float _damage;

        private sealed class Baker : Baker<DamageAuthoring>
        {
            public override void Bake(DamageAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Damage {Value = authoring._damage});
            }
        }
    }
}