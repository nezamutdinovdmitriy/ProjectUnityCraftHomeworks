using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public sealed class ArcherAuthoring : MonoBehaviour
    {
        [SerializeField] private int _attackAmmoCost;
        
        private sealed class Baker : Baker<ArcherAuthoring>
        {
            public override void Bake(ArcherAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new Archer());
                
                AddComponent(entity, new AmmoCost()
                {
                    Value = authoring._attackAmmoCost
                });
            }
        }
    }
}