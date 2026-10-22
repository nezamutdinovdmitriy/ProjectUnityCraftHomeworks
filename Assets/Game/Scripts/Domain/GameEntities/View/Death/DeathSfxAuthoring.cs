using Modules.AudioEvents;
using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class DeathSfxAuthoring : MonoBehaviour
    {
            [SerializeField] private AudioEventSerialized _audioEvent;
            
            private sealed class Baker : Baker<DeathSfxAuthoring>
            {
                public override void Bake(DeathSfxAuthoring authoring)
                {
                    Entity view = GetEntity(TransformUsageFlags.Dynamic);

                    AddComponent(view, new DeathSfx
                    {
                        Value = authoring._audioEvent.Key
                    });
                }
            }
    }
}