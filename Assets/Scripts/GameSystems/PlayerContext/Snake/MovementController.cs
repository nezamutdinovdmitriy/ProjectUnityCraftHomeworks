using Modules;
using Zenject;

namespace GameSystems
{
    public class MovementController : ITickable
    {
        private readonly IInputProvider _inputProvider;
        private readonly ISnake _snake;

        public MovementController(IInputProvider input, ISnake snake)
        {
            _inputProvider = input;
            _snake = snake;
        }

        public void Tick() => _snake.Turn(_inputProvider.GetDirection());
    }
}