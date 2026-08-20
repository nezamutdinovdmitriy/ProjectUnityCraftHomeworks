using UnityEngine;

namespace SampleGame
{
    public sealed class ProjectileWeapon : Weapon
    {
        [SerializeField]
        private GameObject _owner;
        
        [SerializeField]
        private Transform _firePoint;
        
        [SerializeField]
        private GameObject _bulletPrefab;
        
        [SerializeField]
        private float _cooldown;
        
        private float _timestamp;

        private void Awake()
        {
            _timestamp = Time.time - _cooldown;
        }

        public override bool CanFire(GameObject target)
        {
            return _timestamp < Time.time;
        }

        protected override void ProcessFire(GameObject target)
        {
            this.SpawnBullet(target);
            this.ResetCooldown();
        }

        private void ResetCooldown()
        {
            _timestamp = Time.time + _cooldown;
        }

        private void SpawnBullet(GameObject target)
        {
            Vector3 firePosition = _firePoint.position;
            Vector3 delta = target.transform.position - firePosition;
            delta.y = 0;

            Quaternion fireDirection = Quaternion.LookRotation(delta.normalized, Vector3.up);
            GameObject bullet = Instantiate(_bulletPrefab, firePosition, fireDirection, WorldTransform.Instance);
            bullet.GetComponent<TeamComponent>().Team = _owner.GetComponent<TeamComponent>().Team;
        }
    }
}