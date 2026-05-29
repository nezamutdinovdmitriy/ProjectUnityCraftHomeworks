using System;

namespace GameSystems.GameContext
{
    public class GameCycle
    {
        public event Action Victory;
        public event Action Defeated;

        public void SetVictory() => Victory?.Invoke();
        public void SetDefeat() => Defeated?.Invoke();
    }
}