using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.View.Components
{
    public sealed class TeamColorPropertyAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<TeamColorPropertyAuthoring>
        {
            public override void Bake(TeamColorPropertyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Renderable);

                Color defaultColor = Color.magenta;
                
                AddComponent(entity, new TeamColorProperty
                {
                    Value = new float4(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a)
                });
            }
        }
    }
}