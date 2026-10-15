using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class AbilityDefinitionAuthoring : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _range = 8f;

        [SerializeField, Min(0f)]
        private float _windup = 0.4f;

        [SerializeField, Min(0f)]
        private float _cooldown = 1f;
        
        private sealed class Baker : Baker<AbilityDefinitionAuthoring>
        {
            public override void Bake(AbilityDefinitionAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new AbilityDefinition
                {
                    Range = authoring._range,
                    Windup = authoring._windup,
                    Cooldown = authoring._cooldown
                });
            }
        }
    }
}