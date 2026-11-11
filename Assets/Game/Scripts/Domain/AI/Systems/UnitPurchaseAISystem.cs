using System;
using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Game.UseCases.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial struct UnitPurchaseAISystem : ISystem
    {
        private uint _nextSeedIndex;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UnitPurchaseItem>();
            state.RequireForUpdate<UnitPurchaseAI>();

            _nextSeedIndex = (uint) (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % uint.MaxValue);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            DynamicBuffer<UnitPurchaseItem> items = SystemAPI
                .GetSingletonBuffer<UnitPurchaseItem>(true);

            if (items.IsEmpty)
                return;
            
            foreach (var (
                         requests,
                         energy,
                         buyer) 
                     in SystemAPI.Query<
                             DynamicBuffer<UnitPurchaseRequest>,
                             RefRO<Energy>,
                             RefRW<UnitPurchaseAI>>()
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
                
                if (buyer.ValueRO.SelectedItemIndex < 0)
                    buyer.ValueRW.SelectedItemIndex = buyer.ValueRW.Generator.NextInt(items.Length);

                int itemIndex = buyer.ValueRO.SelectedItemIndex;
                int price = items[itemIndex].Price;
                
                if(price < 0 || energy.ValueRO.Value < price)
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