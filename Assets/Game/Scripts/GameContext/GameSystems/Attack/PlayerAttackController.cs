using Zenject;

namespace GameContexts
{
    public class PlayerAttackController : ITickable
    {
        private readonly InputService _input;
        private readonly CharacterProvider _characterProvider;

        public PlayerAttackController(InputService input, CharacterProvider characterProvider)
        {
            _input = input;
            _characterProvider = characterProvider;
        }

        public void Tick()
        {
            if (_characterProvider.GetCharacter() == null)
                return;
            
            if(_input.IsMainAttacked)
                if(_characterProvider.GetCharacter().TryGet(out IPushComponent pushComponent))
                    pushComponent.Push();
            
            if(_input.IsAdditionalAttacked)
                if(_characterProvider.GetCharacter().TryGet(out ITossComponent tossComponent))
                    tossComponent.Toss();
        }
    }
}