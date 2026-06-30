using UnityEngine;

namespace Game.Scripts.GameObjects.GameSystems.Attack
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField]
        private Entity _entity;

        [SerializeField]
        private InputService _input;
        
        private IPlayerAttacks _playerAttacks;

        private void Awake() => _playerAttacks = _entity.Get<IPlayerAttacks>();

        private void Update()
        {
            if(_input.IsMainAttacked)
                _playerAttacks.MainAttack();
            
            if(_input.IsAdditionalAttacked)
                _playerAttacks.AdditionalAttack();
        }
    }
}