using Game.GameEntities;
using Game.Score;

namespace Game
{
    public static class DamageUseCase
    {
        public static bool TryInvokeTakeDamageCommand(
            this IGameContext gameContext, 
            IGameEntity targetEntity, 
            float damage)
        {
            if (targetEntity.TryInvokeTakeDamageCommand(damage) == false)
                return false;
            
            if(targetEntity.IsDead())
                gameContext.AddScore();
            
            return true;
        }
    }
}