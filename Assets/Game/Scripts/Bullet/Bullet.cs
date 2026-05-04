using System;
using UnityEngine;

namespace Game
{
    // +
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collider2D> Hit;

        [SerializeField]
        private GameObject _blueVFX;

        [SerializeField]
        private GameObject _redVFX;

        private int _damage;
        private float _speed;
        private TeamType _team;
        private Vector2 _direction;

        public void Initialize(Vector2 position, Vector2 direction, int damage, float speed, TeamType team)
        {
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            _direction = direction;
            _damage = damage;
            _speed = speed;
            _team = team;

            SetVfx(team);

            Debug.Log($"TEAM: {team}");
            
            gameObject.layer = BulletLayerHelper.GetLayer(team);
        }

        public void MoveStep(float deltaTime)
        {
            Vector3 moveStep = _direction * _speed * deltaTime;
            transform.position += moveStep;
        }

        private void SetVfx(TeamType team)
        {
            _blueVFX.SetActive(team == TeamType.Player);
            _redVFX.SetActive(team == TeamType.Enemy);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable target) == false)
                return;

            if (target.Team != _team)
            {
                target.TakeDamage(_damage);

                Hit?.Invoke(this, other);
            }
        }
    }
}