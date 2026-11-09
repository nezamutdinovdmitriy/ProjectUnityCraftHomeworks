using UnityEngine;

namespace Game.Presentation.ParticleEffects
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ParticleEffectView : MonoBehaviour
    {
        private ParticleSystem _root;

        public void Initialize()
        {
            if (_root != null)
                return;

            _root = GetComponent<ParticleSystem>();

            ParticleSystem[] subParticleSystems = GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem particles in subParticleSystems)
            {
                var main = particles.main;
                main.playOnAwake = false;
                main.stopAction = ParticleSystemStopAction.None;
            }
        }
        
        public void PlayAt(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            _root.Play(withChildren: true);
        }

        public void StopAndClear()
        {
            _root.Stop(
                withChildren: true,
                stopBehavior: ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        
        public bool IsAlive() 
            => _root.IsAlive(withChildren: true);
    }
}