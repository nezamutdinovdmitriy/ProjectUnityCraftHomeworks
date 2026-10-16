using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class AbilityLoadoutAuthoring : MonoBehaviour
    {
        [SerializeField] private AbilityDefinitionAuthoring[] _abilities;
        [SerializeField] private int _selectedAbilityIndex;
        
        private sealed class Baker : Baker<AbilityLoadoutAuthoring>
        {
            public override void Bake(AbilityLoadoutAuthoring authoring)
            {
                Entity[] definitions = new Entity[authoring._abilities.Length];

                for (int i = 0; i < definitions.Length; i++)
                {
                    AbilityDefinitionAuthoring ability = authoring._abilities[i];
                    definitions[i] = GetEntity(ability.gameObject, TransformUsageFlags.None);
                }

                Entity actor = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(actor, new SelectedAbility
                {
                    Value = definitions[authoring._selectedAbilityIndex]
                });
                
                AddComponent(actor, new AbilityUseRequest());
                SetComponentEnabled<AbilityUseRequest>(actor, false);
                
                AddComponent(actor, new AbilityWindup());
                SetComponentEnabled<AbilityWindup>(actor, false);
                
                AddComponent(actor, new AbilityCooldown());
                SetComponentEnabled<AbilityCooldown>(actor, false);

                DynamicBuffer<AvailableAbility> available = AddBuffer<AvailableAbility>(actor);
                
                foreach (var definition in definitions)
                {
                    available.Add(new AvailableAbility
                    {
                        Definition = definition
                    });
                }
            }
        }
    }
}