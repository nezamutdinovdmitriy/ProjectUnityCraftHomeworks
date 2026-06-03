using Game.Scripts.Presenters;
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
        /*[SerializeField]
        private PlanetPresenter _planetPresenter;

        [SerializeField]
        private MoneyPresenter _moneyPresenter;

        [SerializeField]
        private PlanetPopupPresenter _planetPopupPresenter;*/

        public override void InstallBindings()
        {
            Container.Bind<PlanetPopupPresenter>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<MoneyPresenter>().FromComponentInHierarchy().AsSingle();

            Container.Bind<PlanetPresenter>().FromComponentInHierarchy().AsCached();
            
            /*Container.Bind<PlanetPopupPresenter>().FromInstance(_planetPopupPresenter).AsSingle();
            
            Container.Bind<MoneyPresenter>().FromInstance(_moneyPresenter).AsSingle();
            
            Container.Bind<PlanetPresenter>().FromInstance(_planetPresenter).AsSingle();*/
        }
    }
}