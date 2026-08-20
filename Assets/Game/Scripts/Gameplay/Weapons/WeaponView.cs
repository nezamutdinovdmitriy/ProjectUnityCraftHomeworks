using UnityEngine;

namespace SampleGame
{
    public sealed class WeaponView : MonoBehaviour
    {
        [SerializeField]
        private Weapon _weapon;
        
        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private AudioSource _audioSource;
        
        private void OnEnable()
        {
            _weapon.OnFire += this.OnFire;
        }

        private void OnDisable()
        {
            _weapon.OnFire -= this.OnFire;
        }

        private void OnFire()
        {
            _particleSystem.Play();
            _audioSource.Play();
        }
    }
}