using Game.Configs;
using Game.View.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class GameContextAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamCatalog _teamCatalog;

        private sealed class Baker : Baker<GameContextAuthoring>
        {
            public override void Bake(GameContextAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                TeamCatalog catalog = authoring._teamCatalog;
                DependsOn(catalog);
                
                if (catalog == null)
                    return;
                
                Color red = catalog.GetTeam(TeamType.Red).Color.linear;
                Color blue = catalog.GetTeam(TeamType.Blue).Color.linear;
                
                AddComponent(entity, new TeamColors
                {
                    Blue = new float4(blue.r, blue.g, blue.b, blue.a),
                    Red = new float4(red.r, red.g, red.b, red.a)
                });

                AddComponent(entity, new GameState
                {
                    Value = GameStateType.Playing
                });

                AddComponent(entity, new GameDeltaTime());
                
                AddComponent(entity, new GameOverRequest());
                SetComponentEnabled<GameOverRequest>(entity, false);
            }
        }
    }
}