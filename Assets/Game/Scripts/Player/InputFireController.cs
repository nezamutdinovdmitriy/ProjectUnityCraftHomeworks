using UnityEngine;

namespace Game
{
    public class InputFireController : MonoBehaviour
    {
        [SerializeField] private PlayerShip _playerShip;

        [SerializeField] private KeyCode _fireKey;
        
        private void Update()
        {
            if (Input.GetKeyDown(_fireKey))
                _playerShip.Fire();
        }
    }
}