using GameObjects.Components;
using Zenject;

namespace GameContexts
{
    public class PlayerJumpController : ITickable
    {
        private readonly InputService _input;
        private readonly CharacterProvider _characterProvider;

        public PlayerJumpController(InputService input, CharacterProvider characterProvider)
        {
            _input = input;
            _characterProvider = characterProvider;
        }

        public void Tick()
        {
            if (_characterProvider.GetCharacter() == null)
                return;
            
            if (_input.IsJumped)
                if (_characterProvider.GetCharacter().TryGet(out JumpRequestComponent jumpRequestComponent))
                    jumpRequestComponent.RequestJump();
        }
    }
}