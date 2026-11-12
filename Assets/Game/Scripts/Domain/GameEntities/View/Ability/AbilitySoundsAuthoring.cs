using Modules.AudioEvents;
using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    [DisallowMultipleComponent]
    public sealed class AbilitySoundsAuthoring : MonoBehaviour
    {
        [SerializeField] private AudioEventSerialized _started;
        [SerializeField] private AudioEventSerialized _executed;
        
        private sealed class Baker : Baker<AbilitySoundsAuthoring>
        {
            public override void Bake(AbilitySoundsAuthoring authoring)
            {
                Entity definition = GetEntity(TransformUsageFlags.None);

                AddComponent(definition, new AbilitySounds
                {
                    Started = string.IsNullOrEmpty(authoring._started.Key) 
                        ? default(AudioEventKey) : authoring._started,
                    
                    Executed = string.IsNullOrEmpty(authoring._executed.Key) 
                        ? default(AudioEventKey) : authoring._executed
                });
            }
        }
    }
}