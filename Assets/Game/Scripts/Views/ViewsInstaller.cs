using Modules.UI;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField]
        private ParticleAnimator _particleAnimator;
        
        public override void InstallBindings()
        {
            Container.Bind<ParticleAnimator>().FromInstance(_particleAnimator).AsSingle();
        }
    }
}