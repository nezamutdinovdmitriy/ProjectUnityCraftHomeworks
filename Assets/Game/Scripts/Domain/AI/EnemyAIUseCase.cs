using Game.Components;

namespace Game.UseCases
{
    public static class EnemyAIUseCase
    {
        public static bool IsExpired(this in TargetDetectionCooldown timer)
        {
            return timer.Time <= 0;
        }
        
        public static bool IsEnemy(this in Team selfTeam, in Team candidateTeam)
        {
            return selfTeam.Value != candidateTeam.Value;
        }
    }
}