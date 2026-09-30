using Unity.Entities;
using UnityEngine;

namespace Game.View.Components.Authoring
{
    public sealed class TeamMaterialPartAuthoring : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer[] _renderers;
        
        private sealed class Baker : Baker<TeamMaterialPartAuthoring>
        {
            public override void Bake(TeamMaterialPartAuthoring authoring)
            {
                Entity viewEntity = GetEntity(TransformUsageFlags.None);
                
                DynamicBuffer<TeamMaterialPart> parts = AddBuffer<TeamMaterialPart>(viewEntity);

                foreach (SkinnedMeshRenderer skinnedMeshRenderer in authoring._renderers)
                {
                    parts.Add(new TeamMaterialPart
                    {
                        Value = GetEntity(skinnedMeshRenderer, TransformUsageFlags.Renderable)
                    });
                }
            }
        }
    }
}