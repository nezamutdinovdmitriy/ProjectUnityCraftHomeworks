using Zenject;

namespace Game
{
    public class PlayerJumpController : ITickable
    {
        private readonly InputService _input;
        private readonly JumpRequestComponent _jumpRequestComponent;

        public PlayerJumpController(InputService input, JumpRequestComponent jumpRequestComponent)
        {
            _input = input;
            _jumpRequestComponent = jumpRequestComponent;
        }

        public void Tick()
        {
            if (_input.IsJumped)
                _jumpRequestComponent.RequestJump();
        }
    }
}