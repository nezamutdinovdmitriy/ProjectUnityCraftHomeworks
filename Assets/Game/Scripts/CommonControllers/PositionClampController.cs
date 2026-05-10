using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class PositionClampController : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private TransformBounds _playerArea;

        private void LateUpdate()
        {
            if (_target == null)
                return;
            
            _target.position = _playerArea.ClampInBounds(_target.position);
        }
    }
}