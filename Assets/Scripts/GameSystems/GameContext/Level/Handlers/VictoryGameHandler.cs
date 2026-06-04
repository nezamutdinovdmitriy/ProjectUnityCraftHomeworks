using System;
using Modules;
using Zenject;

namespace GameSystems.GameContext.Level.LevelResultHandlers
{
    public class VictoryGameHandler : IInitializable, IDisposable
    {
        private readonly GameCycle _gameCycle;
        
        private readonly ISnake _snake;
        private readonly CoinManager _coinManager;

        public VictoryGameHandler(
            ISnake snake, 
            CoinManager coinManager, 
            GameCycle gameCycle)
        {
            _snake = snake;
            _coinManager = coinManager;
            _gameCycle = gameCycle;
        }
        
        public void Initialize() => _gameCycle.Victory += VictoryProcess;

        public void Dispose() => _gameCycle.Victory -= VictoryProcess;

        public void VictoryProcess()
        {
            _snake.SetActive(false);
            _coinManager.ClearActiveCoins();
        }
    }
}