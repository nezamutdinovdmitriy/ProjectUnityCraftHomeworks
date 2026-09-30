using Unity.Burst;
using Unity.Mathematics;

namespace Game.Components
{
    [BurstCompile]
    public static class HealthUseCase
    {
        [BurstCompile]
        public static void ReduceHealth(this ref Health health, float damage)
        {
            if (damage < 0)
                return;

            health.Value = math.max(0, health.Value - damage);
        }

        [BurstCompile]
        public static bool IsAlive(this in Health health) => health.Value > 0;
        
        [BurstCompile]
        public static bool IsDead(this in Health health) => health.Value < 0;
    }
}