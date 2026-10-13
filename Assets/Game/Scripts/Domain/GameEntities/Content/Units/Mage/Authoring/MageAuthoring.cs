using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class MageAuthoring : MonoBehaviour
    {
        [SerializeField] private float _healManaCost;
        private sealed class Baker : Baker<MageAuthoring>
        {
            public override void Bake(MageAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Mage());
                
                AddComponent(entity, new ManaCost
                {
                    Value = authoring._healManaCost
                });
            }
        }
    }
}