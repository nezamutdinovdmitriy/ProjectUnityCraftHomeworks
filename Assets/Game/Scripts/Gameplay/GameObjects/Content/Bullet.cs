using UnityEngine;

namespace SampleGame
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField]
        private int _damage = 1;

        [SerializeField]
        private float _lifetime = 5;

        [SerializeField]
        private MoveTransformComponent _moveComponent;

        [SerializeField]
        private TeamComponent _teamComponent;

        private void Start()
        {
            Destroy(this.gameObject, _lifetime);
        }

        private void FixedUpdate()
        {
            _moveComponent.MoveStep(this.transform.forward, Time.fixedDeltaTime);
        }

        private void OnTriggerEnter(Collider collider)
        {
            GameObject other = collider.gameObject;
            if (!other.TryGetComponent(out TakeDamageComponent component))
                return;

            if (!_teamComponent.IsEnemy(other))
                return;
            
            component.TakeDamage(new TakeDamageArgs
            {
                instigator = this.gameObject,
                damage = _damage
            });

            Destroy(this.gameObject);
        }
    }
}