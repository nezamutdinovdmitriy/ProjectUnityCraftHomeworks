using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class AmmoCostAuthoring : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private int _cost = 1;
        
        private sealed class Baker : Baker<AmmoCostAuthoring>
        {
            public override void Bake(AmmoCostAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new AmmoCost
                {
                    Value = authoring._cost
                });
            }
        }
    }
}