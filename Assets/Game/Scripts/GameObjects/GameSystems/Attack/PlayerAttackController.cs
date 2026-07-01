using Zenject;

namespace Game.Scripts.GameObjects.GameSystems.Attack
{
    public class PlayerAttackController : ITickable
    {
        private readonly InputService _input;
        private readonly IPlayerAttacks _playerAttacks;

        public PlayerAttackController(InputService input, IPlayerAttacks playerAttacks)
        {
            _input = input;
            _playerAttacks = playerAttacks;
        }

        public void Tick()
        {
            if(_input.IsMainAttacked)
                _playerAttacks.MainAttack();
            
            if(_input.IsAdditionalAttacked)
                _playerAttacks.AdditionalAttack();
        }
    }
}