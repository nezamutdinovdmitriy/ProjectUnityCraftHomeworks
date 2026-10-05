using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct EmptyManaRestoreSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRO<Mana> mana,
                         RefRW<ManaRestoreDelay> restoreDelay,
                         EnabledRefRW<ManaRestoreDelay> delayEnabled)
                     in SystemAPI.Query<
                             RefRO<Mana>,
                             RefRW<ManaRestoreDelay>,
                             EnabledRefRW<ManaRestoreDelay>>()
                         .WithDisabled<ManaRestoreDelay>())
            {
                if (mana.ValueRO.IsEmpty() == false)
                    continue;

                restoreDelay.ValueRW.Time = restoreDelay.ValueRO.Delay;
                delayEnabled.ValueRW = true;
            }
        }
    }
}