using Unity.Mathematics;

namespace Game.Components
{
    public static class HealthUseCase
    {
        public static void ReduceHealth(this ref Health health, float damage)
        {
            if (damage < 0)
                return;

            health.Value = math.max(0, health.Value - damage);
        }

        public static bool IsAlive(this in Health health) => health.Value > 0;
        public static bool IsDead(this in Health health) => health.Value < 0;
    }
}