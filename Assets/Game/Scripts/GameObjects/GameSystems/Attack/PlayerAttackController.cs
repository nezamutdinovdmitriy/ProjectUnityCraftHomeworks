using UnityEngine;

namespace Game.Scripts.GameObjects.GameSystems.Attack
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _object;

        private InputService _input;
        private IPlayerAttacks _playerAttacks;

        private void Awake()
        {
            _input = GetComponent<InputService>();
            _playerAttacks = GetComponent<IPlayerAttacks>();
        }

        private void Update()
        {
            if(_input.IsMainAttacked)
                _playerAttacks.MainAttack();
            
            if(_input.IsAdditionalAttacked)
                _playerAttacks.AdditionalAttack();
        }
    }
}