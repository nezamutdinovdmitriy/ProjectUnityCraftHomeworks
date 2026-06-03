using Modules.UI;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField]
        private PlanetPopupView _planetPopupView;
        
        [SerializeField]
        private PlanetView _planetView;

        [SerializeField]
        private ParticleAnimator _particleAnimator;
        
        public override void InstallBindings()
        {
            Container.Bind<PlanetPopupView>().FromInstance(_planetPopupView).AsSingle();
            
            Container.Bind<PlanetView>().FromInstance(_planetView).AsSingle();

            Container.Bind<ParticleAnimator>().FromInstance(_particleAnimator).AsSingle();
        }
    }
}