using Atomic.Entities;

namespace Game.Score
{
    public static class ScoreUseCase
    {
        public static void AddScore(this IGameContext gameContext) 
            => gameContext.GetValue(GameContextAPI.Score).Value++;
    }
}