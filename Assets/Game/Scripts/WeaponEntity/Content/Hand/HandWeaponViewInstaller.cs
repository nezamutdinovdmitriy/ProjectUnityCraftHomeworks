using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Weapon
{
    public class HandWeaponViewInstaller : SceneEntityInstaller<IWeaponEntity>
    {
        private readonly DisposableComposite _disposables = new();
        
        [SerializeField]
        private AnimationEvents _animationEvents;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _attackSound;
        
        public override void Install(IWeaponEntity entity)
        {
            entity.GetValue(WeaponEntityAPI.FireCommand)
                .Subscribe(OnFired)
                .AddTo(_disposables);
        }

        public override void Uninstall(IWeaponEntity entity) 
            => _disposables?.Dispose();
        
        private void OnFired() 
            => PlayRandomSound(_attackSound);
        
        private void PlayRandomSound(AudioClip[] clips)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            _audioSource.pitch = 1;
        }
    }
}