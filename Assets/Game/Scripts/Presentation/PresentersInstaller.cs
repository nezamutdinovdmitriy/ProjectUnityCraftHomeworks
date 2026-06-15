using Zenject;

namespace Game.Gameplay
{
    public class PresentersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SaveLoadPresenter>().AsSingle();
        }
    }
}