using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ArmorPercentAuthoring : MonoBehaviour
    {
        [SerializeField] private float _initialArmorPercent;
        
        private sealed class Baker : Baker<ArmorPercentAuthoring>
        {
            public override void Bake(ArmorPercentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new Armor {Value = authoring._initialArmorPercent});
            }
        }
    }
}