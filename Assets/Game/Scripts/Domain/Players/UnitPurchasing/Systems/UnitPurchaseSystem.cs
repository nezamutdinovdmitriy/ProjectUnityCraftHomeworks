using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial struct UnitPurchaseSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UnitPurchaseItem>();
            state.RequireForUpdate<UnitPurchaseRequest>();
            state.RequireForUpdate<UnitSpawnRequest>();
            state.RequireForUpdate<GameState>();
        }

        public void OnUpdate(ref SystemState state)
        {
            GameState gameState = SystemAPI.GetSingleton<GameState>();

            if (gameState.Value != GameStateType.Playing)
                return;

            DynamicBuffer<UnitPurchaseItem> items = SystemAPI
                .GetSingletonBuffer<UnitPurchaseItem>(true);

            foreach (var (
                         purchaseRequests,
                         energy,
                         team)
                     in SystemAPI.Query<DynamicBuffer<UnitPurchaseRequest>,
                             RefRW<Energy>,
                             RefRO<Team>>()
                         .WithAll<Player>())
            {
                if (purchaseRequests.IsEmpty)
                    continue;

                if (TryGetSpawnRequestBuffer(
                        ref state,
                        team.ValueRO.Value,
                        out DynamicBuffer<UnitSpawnRequest> spawns))
                {
                    for (int i = 0; i < purchaseRequests.Length; i++)
                    {
                        int itemIndex = purchaseRequests[i].ItemIndex;

                        if (itemIndex < 0 || itemIndex >= items.Length)
                            continue;

                        UnitPurchaseItem item = items[itemIndex];

                        if (item.Price < 0 || energy.ValueRO.Value < item.Price)
                            continue;

                        if (!SystemAPI.HasComponent<Prefab>(item.Prefab) ||
                            !SystemAPI.HasComponent<Team>(item.Prefab) ||
                            !SystemAPI.HasComponent<LocalTransform>(item.Prefab))
                            continue;

                        spawns.Add(new UnitSpawnRequest {Prefab = item.Prefab});
                        energy.ValueRW.Value -= item.Price;
                    }
                }

                purchaseRequests.Clear();
            }
        }

        private bool TryGetSpawnRequestBuffer(
            ref SystemState state,
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

                if (!HasValidSpawnPoints(ref state, points))
                    continue;

                spawnRequests = spawns;
                return true;
            }

            spawnRequests = default;
            return false;
        }

        private bool HasValidSpawnPoints(
            ref SystemState state,
            DynamicBuffer<UnitSpawnPoint> points)
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