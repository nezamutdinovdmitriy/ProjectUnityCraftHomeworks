using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using SampleGame;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial class UnitPurchaseSystem : SystemBase
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UnitCatalogReference>();
            state.RequireForUpdate<UnitPrefab>();
            state.RequireForUpdate<UnitPurchaseRequest>();
            state.RequireForUpdate<UnitSpawnRequest>();
        }

        protected override void OnUpdate()
        {
            UnitCardsCatalog catalog = SystemAPI.GetSingleton<UnitCatalogReference>().Catalog.Value;

            if (catalog == null)
                return;

            DynamicBuffer<UnitPrefab> prefabs = SystemAPI.GetSingletonBuffer<UnitPrefab>(true);

            if (prefabs.IsEmpty || prefabs.Length != catalog.Cards.Count)
                return;

            foreach (var (
                         purchaseRequests,
                         energy,
                         team)
                     in SystemAPI.Query<DynamicBuffer<UnitPurchaseRequest>,
                         RefRW<Energy>,
                         RefRO<Team>>().WithAll<Player>())
            {
                if (purchaseRequests.IsEmpty)
                    continue;

                if (TryGetSpawnRequestBuffer(
                        team.ValueRO.Value,
                        out DynamicBuffer<UnitSpawnRequest> spawns))
                {
                    for (int i = 0; i < purchaseRequests.Length; i++)
                    {
                        int itemIndex = purchaseRequests[i].ItemIndex;
                        if (itemIndex < 0 || itemIndex >= prefabs.Length)
                            continue;

                        UnitCardConfig card = catalog.GetCard(itemIndex);
                        if (card == null)
                            continue;

                        int price = card.Price;
                        if (price < 0 || energy.ValueRO.Value < price)
                            continue;

                        Entity prefab = prefabs[itemIndex].Value;

                        if (!SystemAPI.HasComponent<Prefab>(prefab) ||
                            !SystemAPI.HasComponent<Team>(prefab) ||
                            !SystemAPI.HasComponent<LocalTransform>(prefab))
                            continue;

                        spawns.Add(new UnitSpawnRequest
                        {
                            Prefab = prefab
                        });
                        
                        energy.ValueRW.Value -= price;
                    }
                }

                purchaseRequests.Clear();
            }
        }

        private bool TryGetSpawnRequestBuffer(
            TeamType team,
            out DynamicBuffer<UnitSpawnRequest> spawnRequests)
        {
            foreach (var (
                         spawns,
                         points,
                         baseTeam,
                         health)
                     in SystemAPI.Query<DynamicBuffer<UnitSpawnRequest>,
                             DynamicBuffer<UnitSpawnPoint>,
                             RefRO<Team>,
                             RefRO<Health>>()
                         .WithAll<Base, UnitSpawnRandom>())
            {
                if (baseTeam.ValueRO.Value != team ||
                    health.ValueRO.IsDead() ||
                    points.IsEmpty)
                    continue;

                if (!HasValidSpawnPoints(points))
                    continue;

                spawnRequests = spawns;
                return true;
            }

            spawnRequests = default;
            return false;
        }

        private bool HasValidSpawnPoints(DynamicBuffer<UnitSpawnPoint> points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (!SystemAPI.HasComponent<LocalToWorld>(points[i].Value))
                    return false;
            }

            return true;
        }
    }
}