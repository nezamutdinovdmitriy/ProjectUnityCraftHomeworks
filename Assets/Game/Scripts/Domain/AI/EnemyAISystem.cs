using System;
using Game.Components;
using Game.ECS.SystemGroups;
using SampleGame;
using Unity.Burst;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Game.UseCases.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial class EnemyAISystem : SystemBase
    {
        private uint _nextSeedIndex;
        
        protected override void OnCreate()
        {
            RequireForUpdate<UnitCatalogReference>();
            RequireForUpdate<EnemyAIState>();

            _nextSeedIndex = (uint) (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % uint.MaxValue);
        }
        
        protected override void OnUpdate()
        {
            UnitCardsCatalog catalog = SystemAPI.GetSingleton<UnitCatalogReference>().Catalog.Value;

            if (catalog == null || catalog.Cards.Count == 0)
                return;
            
            int cardCount = catalog.Cards.Count;
            
            foreach (var (
                         requests,
                         energy,
                         buyer) 
                     in SystemAPI.Query<
                             DynamicBuffer<UnitPurchaseRequest>,
                             RefRO<Energy>,
                             RefRW<EnemyAIState>>()
                         .WithAll<Player>())
            {
                if(requests.IsEmpty == false)
                    continue;

                if (buyer.ValueRO.Generator.state == 0)
                {
                    buyer.ValueRW.Generator = Random.CreateFromIndex(_nextSeedIndex);
                    _nextSeedIndex++;

                    if (_nextSeedIndex == uint.MaxValue)
                        _nextSeedIndex = 0;
                }
                
                
                int itemIndex = buyer.ValueRO.SelectedItemIndex;
                if (itemIndex < 0 || itemIndex >= cardCount)
                {
                    itemIndex = buyer.ValueRW.Generator.NextInt(cardCount);
                    buyer.ValueRW.SelectedItemIndex = itemIndex;
                }

                UnitCardConfig cardConfig = catalog.GetCard(itemIndex);
                if (cardConfig == null || cardConfig.Price < 0)
                {
                    buyer.ValueRW.SelectedItemIndex = -1;
                    continue;
                }
                
                if (energy.ValueRO.Value < cardConfig.Price)
                    continue;
                
                requests.Add(new UnitPurchaseRequest
                {
                    ItemIndex = itemIndex
                });
                
                buyer.ValueRW.SelectedItemIndex = -1;
            }
        }
    }
}