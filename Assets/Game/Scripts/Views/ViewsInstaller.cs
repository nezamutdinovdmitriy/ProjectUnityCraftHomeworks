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
        
        public override void InstallBindings()
        {
            Container.Bind<PlanetPopupView>().FromInstance(_planetPopupView).AsSingle();
            Container.Bind<PlanetView>().FromInstance(_planetView).AsSingle();
            //TODO:
        }
    }
}