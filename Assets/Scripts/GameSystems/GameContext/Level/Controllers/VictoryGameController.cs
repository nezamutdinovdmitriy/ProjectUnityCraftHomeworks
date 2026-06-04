using System;
using GameSystems.GameContext;
using Zenject;

namespace GameSystems.Level
{
    public class VictoryGameController : IInitializable, IDisposable
    {
        private readonly GameCycle _gameCycle;

        private readonly LevelManager _levelManger;

        public VictoryGameController(LevelManager levelManger, GameCycle gameCycle)
        {
            _levelManger = levelManger;
            _gameCycle = gameCycle;
        }

        public void Initialize() => _levelManger.AllLevelsCompleted += SetVictory;

        public void Dispose() => _levelManger.AllLevelsCompleted -= SetVictory;

        private void SetVictory() => _gameCycle.SetVictory();
    }
}