using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class LifeTimeAuthoring : MonoBehaviour
    {
        [SerializeField] private float _lifetime;

        private class Baker : Baker<LifeTimeAuthoring>
        {
            public override void Bake(LifeTimeAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Lifetime {Value = authoring._lifetime});
            }
        }
    }
}