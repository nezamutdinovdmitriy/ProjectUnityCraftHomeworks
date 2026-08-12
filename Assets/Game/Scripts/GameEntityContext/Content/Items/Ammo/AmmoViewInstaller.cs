using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class AmmoViewInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private ParticleSystem _particle;

        [SerializeField]
        private AudioSource _audioSource;
        
        public override void Install(IGameEntity entity) 
            => entity.GetValue(GameEntityAPI.InteractCommand).OnEvent += OnInteracted;

        public override void Uninstall(IGameEntity entity) 
            => entity.GetValue(GameEntityAPI.InteractCommand).OnEvent -= OnInteracted;

        private void OnInteracted(IGameEntity obj)
        {
            gameObject.SetActive(false);
            _particle.Play();
            _audioSource.Play();
        }
    }
}