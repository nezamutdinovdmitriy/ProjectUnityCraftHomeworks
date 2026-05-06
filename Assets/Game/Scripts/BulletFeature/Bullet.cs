using System;
using UnityEngine;

namespace Game
{
    // +
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collider2D> Hit;
        public event Action<TeamType> Initialized;

        private int _damage;
        private float _speed;
        private Vector2 _direction;

        public TeamType Team { get; private set; }
        
        public void Initialize(
            Vector2 position, 
            Vector2 direction, 
            int damage, 
            float speed, 
            TeamType team)
        {
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            _direction = direction;
            _damage = damage;
            _speed = speed;
            Team = team;
            
            gameObject.layer = BulletLayerHelper.GetLayer(team);

            Initialized?.Invoke(team);
        }

        public void MoveStep(float deltaTime)
        {
            Vector3 moveStep = _direction * _speed * deltaTime;
            transform.position += moveStep;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable target)
                && target.Team != Team)
            {
                target.TakeDamage(_damage);
                
                Hit?.Invoke(this, other);
            }
        }
    }
}