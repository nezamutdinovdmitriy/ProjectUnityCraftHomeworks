using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    public partial struct PeriodicEnergyIncomeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (
                         energy,
                         cooldown)
                     in SystemAPI.Query<
                         RefRW<Energy>,
                         RefRW<EnergyIncomeCooldown>>()
                         .WithAll<Player>())
            {
                float deltaTime = SystemAPI.Time.DeltaTime;

                if (cooldown.ValueRO.Time > 0)
                {
                    cooldown.ValueRW.Time -= deltaTime;
                    continue;
                }

                energy.ValueRW.Value += cooldown.ValueRO.Income;
                cooldown.ValueRW.Time = cooldown.ValueRO.Cooldown;
            }
        }
    }
}