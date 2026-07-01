using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerMoveController : ITickable
    {
        private readonly InputService _input;

        private readonly MoveRequestComponent _moveRequest;

        public PlayerMoveController(InputService input, MoveRequestComponent moveRequest)
        {
            _input = input;
            _moveRequest = moveRequest;
        }
        
        public void Tick()
        {
            if (_input.MoveDirection != Vector2.zero)
                _moveRequest.SetMoveDirection(_input.MoveDirection);
        }
    }
}