using Game.Components;
using Game.Components.Authoring;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Scripts.Domain.GameEntities.View.Heal
{
    [DisallowMultipleComponent]
    public sealed class HealVFXSettingsAuthoring : MonoBehaviour
    {
        [SerializeField] private ParticleVFXAuthoring _prefab;
        [SerializeField] private Vector3 _offset;
        
        private sealed class Baker : Baker<HealVFXSettingsAuthoring>
        {
            public override void Bake(HealVFXSettingsAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new HealVFXSettings
                {
                    Prefab = GetEntity(authoring._prefab, TransformUsageFlags.Dynamic),
                    Offset = new float3(
                        authoring._offset.x,
                        authoring._offset.y,
                        authoring._offset.z)
                });
            }
        }
    }
}