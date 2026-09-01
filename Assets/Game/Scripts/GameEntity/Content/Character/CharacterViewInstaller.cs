using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities.Weapon;
using UnityEngine;

namespace Game.GameEntities
{
    public class CharacterViewInstaller : SceneEntityInstaller<IGameEntity>
    {
        private const string MoveReceiveEventKey = "move_step_event";
        private const string BodyFallReceiveEventKey = "body_fall_event";

        private readonly int IsMovingKey = Animator.StringToHash("IsMoving");
        private readonly int IsAttackKey = Animator.StringToHash("Attack");
        private readonly int TakeDamageKey = Animator.StringToHash("TakeDamage");
        private readonly int DeathKey = Animator.StringToHash("Death");

        private readonly DisposableComposite _disposables = new();

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AnimationEvents _animationEvents;

        [Space] [Header("Take Damage")] [SerializeField]
        private AudioClip[] _painSounds;

        [SerializeField]
        private ParticleSystem _takeDamageParticle;

        [Space] [Header("Movement")] [SerializeField]
        private AudioClip[] _moveStepSounds;

        [Space] [Header("Death")] [SerializeField]
        private AudioClip _deathSounds;

        [SerializeField]
        private AudioClip[] _bodyFallSounds;

        [SerializeField]
        private ParticleSystem _bodyFallParticle;

        private float _previousHealthAmount;
        
        public override void Install(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.IsMoving).Subscribe(OnMoved).AddTo(_disposables);
            
            IReactiveVariable<float> currentHealth = entity.GetValue(GameEntityAPI.CurrentHealth);
            _previousHealthAmount = currentHealth.Value;
            currentHealth.Subscribe(OnHealthChanged).AddTo(_disposables);

            _animationEvents.Subscribe(MoveReceiveEventKey, OnMovedSFX);
            _animationEvents.Subscribe(BodyFallReceiveEventKey, OnBodyFall);

            if (entity.TryGetValue(GameEntityAPI.Weapon, out IReactiveVariable<IWeaponEntity> weaponEntity))
            {
                IWeaponEntity weapon = weaponEntity.Value;
                weapon.GetValue(WeaponEntityAPI.FireCommand).Subscribe(OnFired).AddTo(_disposables);
            }
        }

        public override void Uninstall(IGameEntity entity) => _disposables.Dispose();

        #region Animation

        private void OnMoved(bool isMoving) => _animator.SetBool(IsMovingKey, isMoving);

        private void OnFired() => _animator.SetTrigger(IsAttackKey);

        private void OnDeath()
        {
            _animator.SetTrigger(DeathKey);
            OnDeathSFX();
        }

        private void OnBodyFall()
        {
            _bodyFallParticle.Play();
            OnBodyFallSFX();
        }

        private void OnHealthChanged(float health)
        {
            if (health < _previousHealthAmount)
                OnTakeDamage();
            
            if (health <= 0)
                OnDeath();

            _previousHealthAmount = health;
        }

        private void OnTakeDamage()
        {
            _animator.SetTrigger(TakeDamageKey);
            _takeDamageParticle.Play();
            OnTakeDamageSFX();
        }

        #endregion
        
        #region SFX

        private void PlayRandomSound(AudioClip[] clips)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            _audioSource.pitch = 1;
        }

        private void OnMovedSFX() => PlayRandomSound(_moveStepSounds);

        private void OnTakeDamageSFX() => PlayRandomSound(_painSounds);

        private void OnDeathSFX() => _audioSource.PlayOneShot(_deathSounds);

        private void OnBodyFallSFX() => PlayRandomSound(_bodyFallSounds);

        #endregion
    }
}