using Game.Scripts.GameObjects;
using UnityEngine;

namespace Game
{
    public class PlayerJumpController : MonoBehaviour
    {
        [SerializeField]
        private Entity _entity;

        [SerializeField]
        private InputService _input;
        
        private JumpRequestComponent _jumpRequestComponent;

        private void Awake()
        {
            _jumpRequestComponent = _entity.Get<JumpRequestComponent>();
        }

        private void Update()
        {
            if (_input.IsJumped)
                _jumpRequestComponent.RequestJump();
        }
    }
}