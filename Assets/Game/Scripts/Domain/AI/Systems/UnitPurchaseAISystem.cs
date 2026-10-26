using System;
using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;

namespace Game.UseCases.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial struct UnitPurchaseAISystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UnitPurchaseItem>();
            state.RequireForUpdate<UnitPurchaseAI>();
        }

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