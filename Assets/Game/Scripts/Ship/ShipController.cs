using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    // +
    public abstract class ShipController : MonoBehaviour, IDamageable
    {
        public event Action<int> HealthChanged;
        public event Action Dead;

        public event Action<ShipController> Fired;

        public ShipConfig config;

        public TeamType Team => _team;
        
        [Header("Health")]
        public int currentHealth;
        

        [Header("Combat")]
        public Transform firePoint;
        public float bulletSpeed;
        public int bulletDamage;
        private float _fireTime;

        [SerializeField]
        private TeamType _team;

        [Header("Movement")]
        [SerializeField]
        protected RigidbodyMovementComponent rigidbodyMovementComponent;
        
        protected Vector3 moveDirection;

        [Header("Visual")]
        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private Transform _viewTransform;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private ShipViewConfig _viewConfig;

        [SerializeField]
        private ParticleSystem _fireVFX;

        [SerializeField]
        private AudioClip _fireSFX;

        [SerializeField]
        private AudioClip _damageSFX;

        private Material _material;
        private Tweener _damageAnimation;


        private void Awake()
        {
            currentHealth = config.Health;
            rigidbodyMovementComponent.SetSpeed(config.MoveSpeed);

            _material = new Material(_viewConfig.MaterialPrefab);
            _renderer.material = _material;
        }

        public void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, config.Health);
                NotifyAboutHealthChanged(currentHealth);
 
                if (currentHealth <= 0)
                {
                    NotifyAboutDead();
                    gameObject.SetActive(false);
                }
            }
        }
        
        public void OnFired()
        {
            float time = Time.time;
            if (time - _fireTime < config.FireCooldown || currentHealth <= 0)
                return;

            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX);

            if (_fireVFX)
                _fireVFX.Play();

            this.Fired?.Invoke(this);
            _fireTime = time;
        }
        
        public void NotifyAboutHealthChanged(int health)
        {
            if (health > 0)
                AnimateDamage();

            HealthChanged?.Invoke(health);
        }

        public void NotifyAboutDead()
        {
            // Instantiate particle vfx 
            ParticleSystem prefab = _viewConfig.DestroyEffectPrefab;
            Instantiate(prefab, _viewTransform.position, prefab.transform.rotation);

            Dead?.Invoke();
        }
        
        protected virtual void LateUpdate()
        {
            AnimateMovement(Time.deltaTime);
        }

        protected virtual void FixedUpdate() => rigidbodyMovementComponent.FixedUpdate();
        
        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _viewConfig.MoveRotationAngle * moveDirection.y;
            shipAngles.y = _viewConfig.MoveRotationAngle / 2 * moveDirection.x * -1f;
            
            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _viewConfig.MoveSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }

        private void AnimateDamage()
        {
            if (_damageAnimation.IsActive())
                _damageAnimation.Kill();

            _damageAnimation = DOVirtual.Float(
                0f,
                1f,
                _viewConfig.HitDuration,
                progress => _material?.SetFloat(_viewConfig.HitPropertyName,
                    _viewConfig.HitAnimationCurve.Evaluate(progress))
            ).SetLink(_renderer.gameObject);

            if (_damageSFX)
                _audioSource.PlayOneShot(_damageSFX);
        }
    }
}