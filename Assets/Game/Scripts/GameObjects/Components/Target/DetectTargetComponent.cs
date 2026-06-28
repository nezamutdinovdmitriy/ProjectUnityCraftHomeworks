using UnityEngine;

namespace Game.Target
{
    public class DetectTargetComponent : MonoBehaviour
    {
        [SerializeField]
        private float _detectRadius;

        [SerializeField]
        private LayerMask _targetMask;

        [SerializeField]
        private GameObject _currentTarget;

        private Collider2D[] _colliderBuffer;
        private ContactFilter2D _filter;

        private void Awake()
        {
            _colliderBuffer = new Collider2D[5];
            
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(_targetMask);
        }

        private void FixedUpdate() => DetectTarget();

        public bool TryGetTarget(out GameObject target)
        {
            if (_currentTarget == null)
            {
                target = null;
                return false;
            }

            target = _currentTarget;
            return true;
        }
        
        private void DetectTarget()
        {
            _currentTarget = null;
            
            int collidersCount = Physics2D.OverlapCircle(
                transform.position, 
                _detectRadius,
                _filter,
                _colliderBuffer);

            for (int i = 0; i < collidersCount; i++)
            {
                if (_colliderBuffer[i].TryGetComponent(out Character character))
                {
                    _currentTarget = character.gameObject;
                    break;
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, _detectRadius);
        }
    }
}