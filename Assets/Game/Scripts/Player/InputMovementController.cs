using UnityEngine;

namespace Game
{
    public class InputMovementController : MonoBehaviour
    {
        private const string HorizontalAxisKey = "Horizontal";
        private const string VerticalAxisKey = "Vertical";
        
        [SerializeField]
        private PlayerShip _playerShip;

        private void Update()
        {
            float dx = Input.GetAxisRaw(HorizontalAxisKey);
            float dy = Input.GetAxisRaw(VerticalAxisKey);

            Vector2 direction = new Vector2(dx, dy);
            
            _playerShip.SetMovementDirection(direction);
        }
    }
}