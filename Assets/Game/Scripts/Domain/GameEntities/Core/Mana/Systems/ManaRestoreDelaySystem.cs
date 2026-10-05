using Game.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct ManaRestoreDelaySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRW<ManaRestoreDelay> delay,
                         EnabledRefRW<ManaRestoreDelay> delayEnabled,
                         EnabledRefRW<ManaRestoreEvent> restoreEvent,
                         RefRW<Mana> mana,
                         RefRO<MaxMana> maxMana)
                     in SystemAPI.Query<
                             RefRW<ManaRestoreDelay>,
                             EnabledRefRW<ManaRestoreDelay>,
                             EnabledRefRW<ManaRestoreEvent>,
                             RefRW<Mana>,
                             RefRO<MaxMana>>()
                         .WithPresent<ManaRestoreEvent>())
            {
                delay.ValueRW.Time -= deltaTime;

                if (delay.ValueRO.IsExpired())
                {
                    mana.ValueRW.Value = maxMana.ValueRO.Value;
                
                    delayEnabled.ValueRW = false;
                    restoreEvent.ValueRW = true;
                }
            }
        }
    }
}