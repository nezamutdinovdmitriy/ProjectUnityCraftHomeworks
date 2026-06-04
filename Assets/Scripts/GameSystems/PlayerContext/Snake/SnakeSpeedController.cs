using System;
using GameSystems.Level;
using Modules;
using Zenject;

namespace GameSystems
{
    public class SnakeSpeedController : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly LevelManager _levelManager;

        public SnakeSpeedController(ISnake snake, LevelManager levelManager)
        {
            _snake = snake;
            _levelManager = levelManager;
        }

        public void Initialize() => _levelManager.LevelStarted += SetSpeed;

        public void Dispose() => _levelManager.LevelStarted -= SetSpeed;

        private void SetSpeed(int modifier) => _snake.SetSpeed(modifier);
    }
}