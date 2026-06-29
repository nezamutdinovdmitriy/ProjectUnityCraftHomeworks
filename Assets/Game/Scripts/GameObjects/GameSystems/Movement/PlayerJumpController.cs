using UnityEngine;

namespace Game
{
    public class PlayerJumpController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _object;

        private InputService _input;
        private JumpRequestComponent _jumpRequestComponent;

        private void Awake()
        {
            _input = GetComponent<InputService>();
            _jumpRequestComponent = _object.GetComponent<JumpRequestComponent>();
        }

        private void Update()
        {
            if (_input.IsJumped)
                _jumpRequestComponent.RequestJump();
        }
    }
}