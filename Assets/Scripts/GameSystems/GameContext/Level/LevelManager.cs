using System;
using Modules;
using Zenject;

namespace GameSystems.Level
{
    public class LevelManager : IInitializable
    {
        public event Action<int> LevelStarted;
        public event Action AllLevelsCompleted;
        
        private readonly IDifficulty _difficulty;

        public LevelManager(IDifficulty difficulty) => _difficulty = difficulty;
        
        public void Initialize() => StartLevel(_difficulty.Current);
        
        public void CompleteLevel()
        {
            if (_difficulty.Next(out int nextLevel))
                StartLevel(nextLevel);
            else
                AllLevelsCompleted?.Invoke();
        }

        private void StartLevel(int level)
        {
            int modifier = level == 0 ? 1 : level;
            
            LevelStarted?.Invoke(modifier);
        }
    }
}