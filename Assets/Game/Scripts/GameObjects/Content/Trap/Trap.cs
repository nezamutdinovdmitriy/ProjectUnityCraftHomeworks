using UnityEngine;

namespace Game
{
    public class Trap : MonoBehaviour
    {
        [SerializeField]
        private int _damage = 1;
        
        private CollisionComponent _collisionComponent;
        
        private void Awake() => _collisionComponent = GetComponent<CollisionComponent>();

        private void OnEnable() => _collisionComponent.OnEntered += OnCollisionEntered;
        private void OnDisable() => _collisionComponent.OnEntered += OnCollisionEntered;

        private void OnCollisionEntered(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out HealthComponent health))
            {
                health.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}