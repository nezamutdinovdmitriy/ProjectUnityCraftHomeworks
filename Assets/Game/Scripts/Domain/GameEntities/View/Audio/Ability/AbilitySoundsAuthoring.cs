using Modules.AudioEvents;
using Unity.Collections;
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
                    Started = new FixedString128Bytes(
                        authoring._started.Key),

                    Executed = new FixedString128Bytes(
                        authoring._executed.Key)
                });
            }
        }
    }
}