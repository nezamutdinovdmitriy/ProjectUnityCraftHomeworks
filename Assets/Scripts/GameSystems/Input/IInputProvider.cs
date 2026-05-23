using Modules;

namespace GameSystems
{
    public interface IInputProvider
    {
        public SnakeDirection GetDirection();
    }
}