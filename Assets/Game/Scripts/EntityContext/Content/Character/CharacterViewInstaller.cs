using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext
{
    public class CharacterViewInstaller : SceneEntityInstaller<IEntityContext>
    {
        private const string MovingEvent = "move_step_event";
        private const string BodyFallEvent = "body_fall_event";
        private const string FireEvent = "fire_event";
        
        private readonly int IsMovingKey = Animator.StringToHash("IsMoving");
        private readonly int IsDeathKey = Animator.StringToHash("Death");
        private readonly int AttackKey = Animator.StringToHash("Attack");

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private AnimationEvents _animationEvents;

        [Header("SFXs")]
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _moveStepSFXs;

        [SerializeField]
        private AudioClip[] _takeDamageSFXs;

        [SerializeField]
        private AudioClip[] _bodyFallSFXs;
        
        [SerializeField]
        private AudioClip _deathSFX;

        [SerializeField]
        private AudioClip _firePistolSFX;
        
        private readonly DisposableComposite _disposables = new();

        public override void Install(IEntityContext entity)
        {
            entity.GetValue(EntityContextAPI.IsDead)
                .Subscribe(OnDeath)
                .AddTo(_disposables);
            
            entity.GetValue(EntityContextAPI.CurrentHealth)
                .Subscribe(_ => PlayTakeDamageSFX())
                .AddTo(_disposables);

            entity.GetValue(EntityContextAPI.FireCommand).OnEvent += OnFired;
            
            _animationEvents.Subscribe(MovingEvent, PlayStepMoveSFX);
            _animationEvents.Subscribe(BodyFallEvent, PlayBodyFallSFX);
            _animationEvents.Subscribe(FireEvent, PlayFirePistolSFX);
            
            entity.WhenFixedTick(_ =>
            {
                _animator.SetBool(IsMovingKey, entity.GetValue(EntityContextAPI.IsMoving).Value);
            });
        }

        public override void Uninstall(IEntityContext entity)
        {
            _disposables.Dispose();
            
            entity.GetValue(EntityContextAPI.FireCommand).OnEvent -= OnFired;
            
            _animationEvents.Unsubscribe(MovingEvent, PlayStepMoveSFX);
            _animationEvents.Unsubscribe(BodyFallEvent, PlayBodyFallSFX);
            _animationEvents.Unsubscribe(FireEvent, OnFired);
        }
        
        private void OnFired() => _animator.SetTrigger(AttackKey);

        private void OnDeath(bool isDeath)
        {
            if (isDeath)
            {
                _animator.SetTrigger(IsDeathKey);
                PlayDeathSFX();
            }
        }

        #region SFXs

        private void PlayFirePistolSFX()
        {
            float randomPitch = Random.Range(0.95f, 1.05f);
            _audioSource.pitch = randomPitch;
            _audioSource.PlayOneShot(_firePistolSFX);
            _audioSource.pitch = 1f;
        }
        
        private void PlayStepMoveSFX()
        {
            int index = Random.Range(0, _moveStepSFXs.Length);
            float randomPitch = Random.Range(0.95f, 1.05f);
            _audioSource.pitch = randomPitch;
            _audioSource.PlayOneShot(_moveStepSFXs[index]);
            _audioSource.pitch = 1f;
        }

        private void PlayTakeDamageSFX()
        {
            int index = Random.Range(0, _takeDamageSFXs.Length);
            _audioSource.PlayOneShot(_takeDamageSFXs[index]);
        }

        private void PlayDeathSFX()
        {
            float randomPitch = Random.Range(0.95f, 1.05f);
            _audioSource.pitch = randomPitch;
            _audioSource.PlayOneShot(_deathSFX);
            _audioSource.pitch = 1f;
        }

        private void PlayBodyFallSFX()
        {
            int index = Random.Range(0, _bodyFallSFXs.Length);
            _audioSource.PlayOneShot(_bodyFallSFXs[index]);
        }
        
        #endregion
    }
}