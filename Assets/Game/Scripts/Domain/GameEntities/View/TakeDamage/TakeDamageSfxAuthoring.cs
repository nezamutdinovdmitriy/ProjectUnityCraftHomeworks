using Modules.AudioEvents;
using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class TakeDamageSfxAuthoring : MonoBehaviour
    {
        [SerializeField] private AudioEventSerialized _audioEvent;
        
        private sealed class Baker : Baker<TakeDamageSfxAuthoring>
        {
            public override void Bake(TakeDamageSfxAuthoring authoring)
            {
                Entity view = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(view, new TakeDamageSfx
                {
                    Key = authoring._audioEvent.Key
                });
            }
        }
    }
}