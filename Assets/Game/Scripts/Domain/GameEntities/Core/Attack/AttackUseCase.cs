using Unity.Mathematics;

namespace Game.Components.UseCases
{
    public static class AttackUseCase
    {
        public static bool InAttackDistance(in float3 delta, float attackDistance)
        {
            float lengthSq = math.lengthsq(delta);
            float attackDistanceSq = attackDistance * attackDistance;

            return lengthSq < attackDistanceSq;
        }
        
        public static bool IsExpired(this in AttackCooldown cooldown)
        {
            return cooldown.Time <= 0;
        }
    }
}