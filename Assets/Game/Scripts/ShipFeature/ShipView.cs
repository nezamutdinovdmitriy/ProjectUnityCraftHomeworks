using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField]
        private Ship _ship;

        [SerializeField]
        private ShipViewConfig _viewConfig;
        [SerializeField]
        private Transform _viewTransform;

        [Header("Visual")] [SerializeField]
        private ParticleSystem _fireVFX;
        [SerializeField]
        private Renderer _renderer;
        private Material _material;

        [Header("Audio")] [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _fireSFX;
        [SerializeField]
        private AudioClip _damageSFX;

        private Tweener _damageAnimation;

        private void Awake()
        {
            _material = new Material(_viewConfig.MaterialPrefab);
            _renderer.material = _material;
        }

        private void OnEnable()
        {
            _ship.HealthComponent.Changed += OnHealthChanged;
            _ship.HealthComponent.Dead += OnDead;
            _ship.FireComponent.Fired += OnFired;
        }

        private void OnDisable()
        {
            _ship.HealthComponent.Changed -= OnHealthChanged;
            _ship.HealthComponent.Dead -= OnDead;
            _ship.FireComponent.Fired -= OnFired;
        }

        private void LateUpdate() => AnimateMovement(Time.deltaTime);

        private void OnHealthChanged(int health) => AnimateDamage(health);

        private void OnDead()
        {
            ParticleSystem prefab = _viewConfig.DestroyEffectPrefab;
            Instantiate(prefab, _viewTransform.position, prefab.transform.rotation);
        }

        private void OnFired(Ship obj)
        {
            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX);

            if (_fireVFX)
                _fireVFX.Play();
        }

        private void AnimateDamage(int health)
        {
            if (health <= 0)
                return;

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

        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _viewConfig.MoveRotationAngle * _ship.MoveDirection.y;
            shipAngles.y = _viewConfig.MoveRotationAngle / 2 * _ship.MoveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _viewConfig.MoveSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }
    }
}