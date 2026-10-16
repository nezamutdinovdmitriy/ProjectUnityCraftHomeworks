using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class MeleeAttackDefinitionAuthoring : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _damage;
        
        private sealed class Baker : Baker<MeleeAttackDefinitionAuthoring>
        {
            public override void Bake(MeleeAttackDefinitionAuthoring authoring)
            {
                Entity definition = GetEntity(TransformUsageFlags.None);
                
                AddComponent(definition, new MeleeAttackAbility
                {
                    Damage = authoring._damage
                });
            }
        }
    }
}