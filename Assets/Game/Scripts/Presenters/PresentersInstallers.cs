using Game.Scripts.Presenters;
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
        public override void InstallBindings()
        {
            Container.Bind<PlanetPopupPresenter>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MoneyPresenter>().FromComponentInHierarchy().AsSingle();

            Container.Bind<PlanetPresenter>().FromComponentsInHierarchy().AsCached();

            Container.Bind<PlanetIncomePresenter>().FromComponentsInHierarchy().AsCached();
        }
    }
}