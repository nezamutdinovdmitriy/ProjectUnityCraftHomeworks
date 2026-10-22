using Modules.AudioEvents;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    [DisallowMultipleComponent]
    public sealed class UnitSoundsAuthoring : MonoBehaviour
    {
        [SerializeField] private AudioEventSerialized _takeDamage;
        [SerializeField] private AudioEventSerialized _heal;
        [SerializeField] private AudioEventSerialized _death;
        
        private sealed class Baker : Baker<UnitSoundsAuthoring>
        {
            public override void Bake(UnitSoundsAuthoring authoring)
            {
                Entity view = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(view, new UnitSounds
                {
                    TakeDamage = new FixedString128Bytes(authoring._takeDamage.Key),
                    Heal = new FixedString128Bytes(authoring._heal.Key),
                    Death = new FixedString128Bytes(authoring._death.Key)
                });
            }
        }
    }
}