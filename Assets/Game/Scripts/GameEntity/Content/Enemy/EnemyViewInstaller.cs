using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;
using UnityEngine;

namespace Game.GameEntity.Content.Enemy
{
    public class EnemyViewInstaller : SceneEntityInstaller<IGameEntity>
    {
        private readonly int IsMovingKey = Animator.StringToHash("IsMoving");
        private readonly int IsAttackKey = Animator.StringToHash("Attack");
        private readonly int TakeDamageKey = Animator.StringToHash("TakeDamage");
        private readonly int DeathKey = Animator.StringToHash("Death");
        
        private readonly DisposableComposite _disposables = new();
        
        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        private AnimationEvents _animationEvents;
        
        [SerializeField]
        private AudioSource _audioSource;
        
        public override void Install(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.IsMoving).Subscribe(OnMoved).AddTo(_disposables);
            
            if (entity.TryGetValue(GameEntityAPI.Weapon, out IReactiveVariable<IWeaponEntity> weaponEntity))
            {
                IWeaponEntity weapon = weaponEntity.Value;
                weapon.GetValue(WeaponEntityAPI.FireStartEvent).Subscribe(OnFired).AddTo(_disposables);
            }
        }

        public override void Uninstall(IGameEntity entity)
        {
            _disposables?.Dispose();
        }

        #region Animation

        private void OnMoved(bool isMoving) => _animator.SetBool(IsMovingKey, isMoving);

        private void OnFired()
        {
            Debug.Log($"Trigger {Time.time}");
            _animator.SetTrigger(IsAttackKey);
        }

        #endregion
        
        #region SFX
        
        private void PlayRandomSound(AudioClip[] clips)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            _audioSource.pitch = 1;
        }

        #endregion

    }
}