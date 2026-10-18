using Game.Components;
using Unity.Entities;

namespace Game.UseCases
{
    public static class DamageUseCase
    {
        public static float Calculate(
            float baseDamage,
            Entity actor,
            ref ComponentLookup<Components.Damage> damageLookup)
        {
            return damageLookup.TryGetRefRO(actor, out RefRO<Damage> actorDamage)
                ? baseDamage + actorDamage.ValueRO.Value
                : baseDamage;
        }
    }
}