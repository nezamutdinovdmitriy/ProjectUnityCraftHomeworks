using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    [CreateAssetMenu(
        fileName = "PresentersInstallers",
        menuName = "Zenject/New PresentersInstallers"
    )]
    public sealed class PresentersInstallers : ScriptableObjectInstaller
    {
        [SerializeField]
        private PlanetPresenter _planetPresenter;

        [Inject]
        private PlanetCatalog _planetCatalog;

        public override void InstallBindings()
        {
            Container.Bind<PlanetPresenter>().FromInstance(_planetPresenter).AsSingle();
        }

        //TODO:
    }
}