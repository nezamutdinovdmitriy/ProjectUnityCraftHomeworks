using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ArmorAuthoring : MonoBehaviour
    {
        [SerializeField] private float _armorPercent;
        
        private sealed class Baker : Baker<ArmorAuthoring>
        {
            public override void Bake(ArmorAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new Armor {Value = authoring._armorPercent});
            }
        }
    }
}