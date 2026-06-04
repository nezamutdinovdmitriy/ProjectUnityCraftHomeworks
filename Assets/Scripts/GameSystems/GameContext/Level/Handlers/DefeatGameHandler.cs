using System;
using Modules;
using Zenject;

namespace GameSystems.GameContext.Level.LevelResultHandlers
{
    public class DefeatGameHandler : IInitializable, IDisposable
    {
        private readonly GameCycle _gameCycle;
        private readonly ISnake _snake;

        public DefeatGameHandler(GameCycle gameCycle, ISnake snake)
        {
            _gameCycle = gameCycle;
            _snake = snake;
        }
        
        public void Initialize() => _gameCycle.Defeated += DefeatProcess;

        public void Dispose() => _gameCycle.Defeated -= DefeatProcess;

        private void DefeatProcess() => _snake.SetActive(false);
    }
}