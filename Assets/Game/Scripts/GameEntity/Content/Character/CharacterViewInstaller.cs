using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;
using UnityEngine;

namespace Game.GameEntity.Content.Character
{
    public class CharacterViewInstaller : SceneEntityInstaller<IGameEntity>
    {
        private const string MoveReceiveEventKey = "move_step_event";

        private readonly int IsMovingKey = Animator.StringToHash("IsMoving");
        private readonly int IsAttackKey = Animator.StringToHash("Attack");

        private readonly DisposableComposite _disposables = new();

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AnimationEvents _animationEvents;

        [Space] [Header("SFX")] [SerializeField]
        private AudioClip[] _moveStepSounds;

        [SerializeField]
        private AudioClip[] _painSounds;

        [SerializeField]
        private AudioClip _deathSounds;

        public override void Install(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.IsMoving).Subscribe(OnMoved).AddTo(_disposables);
            _animationEvents.Subscribe(MoveReceiveEventKey, OnMovedSFX);

            if (entity.TryGetValue(GameEntityAPI.Weapon, out IReactiveVariable<IWeaponEntity> weaponEntity))
            {
                IWeaponEntity weapon = weaponEntity.Value;
                weapon.GetValue(WeaponEntityAPI.FireCommand).Subscribe(OnFired).AddTo(_disposables);
            }
        }

        public override void Uninstall(IGameEntity entity)
        {
            _disposables.Dispose();
        }

        #region Animation

        private void OnMoved(bool isMoving) => _animator.SetBool(IsMovingKey, isMoving);

        private void OnFired() => _animator.SetTrigger(IsAttackKey);

        #endregion


        #region SFX

        private void PlayRandomSound(AudioClip[] clips)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            _audioSource.pitch = 1;
        }

        private void OnMovedSFX() => PlayRandomSound(_moveStepSounds);

        #endregion
    }
}