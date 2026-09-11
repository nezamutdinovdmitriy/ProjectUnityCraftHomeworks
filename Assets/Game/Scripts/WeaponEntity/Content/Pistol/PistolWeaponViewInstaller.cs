using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Weapon
{
    public class PistolWeaponViewInstaller : SceneEntityInstaller<IWeaponEntity>
    {
        private readonly DisposableComposite _disposables = new();

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private ParticleSystem _particleSystem;
        
        public override void Install(IWeaponEntity entity)
        {
            entity.GetValue(WeaponEntityAPI.FireCommand)
                .Subscribe(OnFired)
                .AddTo(_disposables);
        }

        public override void Uninstall(IWeaponEntity entity) 
            => _disposables?.Dispose();

        private void OnFired()
        {
            PlaySFX();
            _particleSystem.Play();
        }

        private void PlaySFX()
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.Play();
            _audioSource.pitch = 1;
        }
    }
}