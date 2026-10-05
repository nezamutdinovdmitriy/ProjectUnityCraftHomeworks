using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class AmmoAuthoring : MonoBehaviour
    {
        [SerializeField] private int _initialAmmo;
        [SerializeField] private int _maxAmmo;

        private sealed class Baker : Baker<AmmoAuthoring>
        {
            public override void Bake(AmmoAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Ammo {Value = authoring._initialAmmo});
                AddComponent(entity, new MaxAmmo {Value = authoring._maxAmmo});
            }
        }
    }
}