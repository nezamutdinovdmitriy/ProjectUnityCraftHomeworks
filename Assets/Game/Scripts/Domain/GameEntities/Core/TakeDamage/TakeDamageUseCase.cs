using Game.Components;

namespace Game.UseCases
{
    public static class TakeDamageUseCase
    {
        public static float Apply(ref Health health, float requestedDamage, float multiplier = 1f)
        {
            float resultDamage = requestedDamage * multiplier;
            
            health.ReduceHealth(resultDamage);

            return resultDamage;
        }
    }
    
}