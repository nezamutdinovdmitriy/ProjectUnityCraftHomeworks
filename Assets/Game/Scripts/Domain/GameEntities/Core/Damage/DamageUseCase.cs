using Game.Components;
using Unity.Entities;

namespace Game.UseCases
{
    public static class DamageUseCase
    {
        public static float Calculate(float baseDamage, float actorDamage)
        {
            return baseDamage + actorDamage;
        }
        
        public static float Calculate(
            float baseDamage,
            Entity actor,
            ref ComponentLookup<Damage> damageLookup)
        {
            float actorDamage = damageLookup.TryGetRefRO(actor, out RefRO<Damage> damage)
                ? damage.ValueRO.Value
                : 0f;

            return Calculate(baseDamage, actorDamage);
        }
    }
}