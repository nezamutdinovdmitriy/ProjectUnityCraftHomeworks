using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.View.Components
{
    public sealed class TeamColorAuthoring : MonoBehaviour
    {
        [SerializeField] private Color _redTeam;
        [SerializeField] private Color _blueTeam;
        
        private sealed class Baker : Baker<TeamColorAuthoring>
        {
            public override void Bake(TeamColorAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                Color red = authoring._redTeam.linear;
                Color blue = authoring._blueTeam.linear;
                
                AddComponent(entity, new TeamColors
                {
                    Red = new float4(red.r, red.g, red.b, red.a),
                    Blue = new float4(blue.r, blue.g, blue.b, blue.a)
                });
            }
        }
    }
}