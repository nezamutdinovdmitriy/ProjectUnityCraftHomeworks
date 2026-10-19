using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ParticleVFXAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<ParticleVFXAuthoring>
        {
            public override void Bake(ParticleVFXAuthoring authoring)
            {
                Entity root = GetEntity(TransformUsageFlags.Dynamic);

                ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

                Entity[] partEntities = new Entity[particles.Length];
                
                for (int i = 0; i < particles.Length; i++)
                    partEntities[i] = GetEntity(
                        particles[i].gameObject,
                        TransformUsageFlags.Dynamic);
                
                AddComponent(root, new ParticleVFX());

                DynamicBuffer<ParticleVFXPart> parts = AddBuffer<ParticleVFXPart>(root);

                foreach (Entity part in partEntities)
                {
                    parts.Add(new ParticleVFXPart
                    {
                        Value = part
                    });
                }
            }
        }
    }
}