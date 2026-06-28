using UnityEngine;

namespace Game
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _object;

        private InputService _input;
        private JumpRequestComponent _jumpRequestComponent;

        private void Awake()
        {
            _input = GetComponent<InputService>();
            _jumpRequestComponent = GetComponent<JumpRequestComponent>();
        }

        private void Update()
        {
            if (_input.IsJumped)
                _jumpRequestComponent.RequestJump();
        }
    }
}