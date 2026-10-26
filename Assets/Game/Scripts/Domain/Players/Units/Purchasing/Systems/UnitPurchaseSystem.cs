using System;
using Game.Components;
using Game.UseCases;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct UnitPurchaseSystem : ISystem
    {
        private ComponentLookup<LocalToWorld> _worldLookup;
        private ComponentLookup<LocalTransform> _prefabTransformLookup;
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<DefaultTargetEntity> _defaultTargetLookup;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UnitPurchaseItem>();
            state.RequireForUpdate<UnitPurchaseRequest>();
            state.RequireForUpdate<GameTime>();
            state.RequireForUpdate<Base>();

            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            _worldLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
            _prefabTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            _teamLookup = SystemAPI.GetComponentLookup<Team>(true);
            _defaultTargetLookup = SystemAPI.GetComponentLookup<DefaultTargetEntity>(true);
        }
        
        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.GetSingleton<GameTime>().DeltaTime <= 0f)
                return;

            state.CompleteDependency();

            _worldLookup.Update(ref state);
            _prefabTransformLookup.Update(ref state);
            _teamLookup.Update(ref state);
            _defaultTargetLookup.Update(ref state);

            DynamicBuffer<UnitPurchaseItem> items = SystemAPI
                .GetSingletonBuffer<UnitPurchaseItem>(true);

            EntityCommandBuffer commands = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (
                         requests,
                         energy,
                         playerTeam) in
                     SystemAPI.Query<
                             DynamicBuffer<UnitPurchaseRequest>,
                             RefRW<Energy>,
                             RefRO<Team>>()
                         .WithAll<Player>())
            {
                if (requests.IsEmpty)
                    continue;

                Entity enemyBase = Entity.Null;

                foreach (var (
                             baseTeam,
                             health,
                             entity) 
                         in SystemAPI.Query<
                                 RefRO<Team>,
                                 RefRO<Health>>()
                             .WithAll<Base>()
                             .WithEntityAccess())
                {
                    if (health.ValueRO.IsAlive()
                        && baseTeam.ValueRO.Value != playerTeam.ValueRO.Value
                        && baseTeam.ValueRO.Value != TeamType.Neutral)
                    {
                        enemyBase = entity;
                        break;
                    }
                }

                if (enemyBase == Entity.Null)
                {
                    requests.Clear();
                    continue;
                }

                foreach (var (
                             points, 
                             random, 
                             baseTeam,
                             health) 
                         in SystemAPI.Query<
                                 DynamicBuffer<UnitSpawnPoint>,
                                 RefRW<UnitSpawnRandom>,
                                 RefRO<Team>,
                                 RefRO<Health>>()
                             .WithAll<Base>())
                {
                    if (baseTeam.ValueRO.Value != playerTeam.ValueRO.Value
                        || health.ValueRO.IsDead()
                        || points.IsEmpty)
                        continue;

                    for (int requestIndex = 0; requestIndex < requests.Length; requestIndex++)
                    {
                        int itemIndex =
                            requests[requestIndex].ItemIndex;

                        if (itemIndex < 0 || itemIndex >= items.Length)
                            continue;

                        UnitPurchaseItem item = items[itemIndex];

                        if (item.Price < 0 || energy.ValueRO.Value < item.Price)
                            continue;

                        if (SystemAPI.HasComponent<Prefab>(item.Prefab) == false
                            || _teamLookup.HasComponent(item.Prefab) == false
                            || _prefabTransformLookup.TryGetRefRO(
                                item.Prefab,
                                out RefRO<LocalTransform> prefabTransform) == false)
                            throw new InvalidOperationException(
                                $"UnitPurchase: item {itemIndex} needs an ECS prefab with Team and LocalTransform.");

                        int pointIndex = random.ValueRW.Generator.NextInt(points.Length);

                        Entity point = points[pointIndex].Value;

                        if (_worldLookup.TryGetRefRO(
                                point,
                                out RefRO<LocalToWorld> pointTransform) == false)
                        {
                            Debug.Log("UnitPurchase: a spawn point has no LocalToWorld.");

                            continue;
                        }

                        Entity defaultTarget = _defaultTargetLookup.HasComponent(item.Prefab)
                                ? enemyBase
                                : Entity.Null;

                        LocalTransform transform = UnitSpawnUseCase.GetTransform(
                            in pointTransform.ValueRO,
                            in prefabTransform.ValueRO);

                        UnitSpawnUseCase.Create(
                            ref commands,
                            item.Prefab,
                            in transform,
                            playerTeam.ValueRO.Value,
                            defaultTarget);

                        energy.ValueRW.Value -= item.Price;
                    }

                    break;
                }

                requests.Clear();
            }
        }
    }
}