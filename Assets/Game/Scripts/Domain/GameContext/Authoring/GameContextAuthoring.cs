using Game.Configs;
using Game.View.Components;
using SampleGame;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class GameContextAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamCatalog _teamCatalog;
        [SerializeField] private UnitCardsCatalog _unitCardsCatalog;

        private sealed class Baker : Baker<GameContextAuthoring>
        {
            public override void Bake(GameContextAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new GameState
                {
                    Value = GameStateType.Playing
                });

                AddComponent(entity, new GameDeltaTime());
                
                AddComponent(entity, new GameOverRequest());
                SetComponentEnabled<GameOverRequest>(entity, false);
                
                BakeTeamCatalog(entity, authoring);
                BakeUnitCatalog(entity, authoring);
            }

            private void BakeUnitCatalog(Entity entity, GameContextAuthoring authoring)
            {
                UnitCardsCatalog catalog = authoring._unitCardsCatalog;
                DependsOn(catalog);

                if (catalog == null || catalog.Cards.Count == 0)
                    return;

                foreach (UnitCardConfig card in catalog.Cards)
                {
                    GameObject prefab = card?.Prefab;
                    DependsOn(prefab);

                    if (prefab == null)
                        return;
                }

                AddComponent(entity, new UnitCatalogReference
                {
                    Catalog = catalog
                });

                DynamicBuffer<UnitPrefab> prefabs = AddBuffer<UnitPrefab>(entity);
                foreach (UnitCardConfig card in catalog.Cards)
                {
                    prefabs.Add(new UnitPrefab
                    {
                        Value = GetEntity(card.Prefab, TransformUsageFlags.Dynamic)
                    });
                }
            }
            
            private void BakeTeamCatalog(Entity entity, GameContextAuthoring authoring)
            {
                TeamCatalog catalog = authoring._teamCatalog;
                DependsOn(catalog);
                
                Color red = catalog.GetTeam(TeamType.Red).Color.linear;
                Color blue = catalog.GetTeam(TeamType.Blue).Color.linear;
                
                AddComponent(entity, new TeamColors
                {
                    Blue = new float4(blue.r, blue.g, blue.b, blue.a),
                    Red = new float4(red.r, red.g, red.b, red.a)
                });
            }
        }
    }
}