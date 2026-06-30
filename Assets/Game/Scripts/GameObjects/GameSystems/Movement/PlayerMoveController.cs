using Game.Scripts.GameObjects;
using UnityEngine;

namespace Game
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField]
        private Entity _entity;

        [SerializeField]
        private InputService _input;

        private MoveRequestComponent _moveRequest;

        private void Start()
        {
            _moveRequest = _entity.Get<MoveRequestComponent>();
        }

        public void Update()
        {
            if (_input.MoveDirection != Vector2.zero)
                _moveRequest.SetMoveDirection(_input.MoveDirection);
        }
    }
}