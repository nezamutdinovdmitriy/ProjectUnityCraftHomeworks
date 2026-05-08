using UnityEngine;

namespace Game
{
    public class PlayerInputController : MonoBehaviour
    {
        private const string HorizontalAxisKey = "Horizontal";
        private const string VerticalAxisKey = "Vertical";
        
        [SerializeField]
        private PlayerShip _playerShip;

        [SerializeField]
        private KeyCode _fireKey;

        private void Update()
        {
            MovementInputHandler();

            FireInputHandler();
        }

        private void FireInputHandler()
        {
            if (Input.GetKeyDown(_fireKey))
                _playerShip.Fire();
        }

        private void MovementInputHandler()
        {
            float dx = Input.GetAxisRaw(HorizontalAxisKey);
            float dy = Input.GetAxisRaw(VerticalAxisKey);

            Vector2 direction = new Vector2(dx, dy);

            _playerShip.SetMovementDirection(direction);
        }
    }
}