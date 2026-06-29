using UnityEngine;

namespace Game
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _object;

        [SerializeField]
        private InputService _input;

        private MoveRequestComponent _moveRequest;

        private void Awake() => _moveRequest = _object.GetComponent<MoveRequestComponent>();

        public void Update()
        {
            if (_input.MoveDirection != Vector2.zero)
                _moveRequest.SetMoveDirection(_input.MoveDirection);
        }
    }
}