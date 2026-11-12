using UnityEngine;

namespace Modules.ParticleEffects
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ParticleEffectView : MonoBehaviour
    {
        private ParticleSystem _root;
        private TransformHandle _transformHandle;

        public void Initialize()
        {
            if (_root != null)
                return;

            _root = GetComponent<ParticleSystem>();
            _transformHandle = gameObject.transformHandle;

            foreach (ParticleSystem particles in GetComponentsInChildren<ParticleSystem>(true))
            {
                ParticleSystem.MainModule main = particles.main;
                main.playOnAwake = false;
                main.stopAction = ParticleSystemStopAction.None;
            }
        }
        
        public void SetPosition(Vector3 position) => _transformHandle.position = position;
        
        public bool IsAlive() => _root.IsAlive(withChildren: true);
        
        public void PlayAt(Vector3 position)
        {
            SetPosition(position);
            gameObject.SetActive(true);
            _root.Play(withChildren: true);
        }

        public void StopAndClear()
        {
            _root.Stop(
                withChildren: true,
                stopBehavior: ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}