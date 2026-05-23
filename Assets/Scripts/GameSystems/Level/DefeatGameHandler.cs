using System;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace GameSystems
{
    public class DefeatGameHandler : IInitializable, IDisposable
    {
        public event Action Defeated;
        
        private readonly ISnake _snake;
        private readonly IWorldBounds _worldBounds;
        
        public DefeatGameHandler(ISnake snake, IWorldBounds worldBounds)
        {
            _snake = snake;
            _worldBounds = worldBounds;
        }
        
        public void Initialize()
        {
            _snake.OnSelfCollided += HandleSelfCollision;
            _snake.OnMoved += HandleOutOfBounds;
        }

        public void Dispose()
        {
            _snake.OnSelfCollided -= HandleSelfCollision;
            _snake.OnMoved -= HandleOutOfBounds;
        }

        private void HandleSelfCollision() => DefeatProcess();
        
        private void HandleOutOfBounds(Vector2Int position)
        {
            if (_worldBounds.IsInBounds(position) == false)
                DefeatProcess();
        }

        private void DefeatProcess()
        {
            _snake.SetActive(false);
            Defeated?.Invoke();
        }
    }
}