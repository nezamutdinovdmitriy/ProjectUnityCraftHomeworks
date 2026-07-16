using Zenject;

namespace Game.Scripts.SceneContext
{
    public class SceneContextInstaller : MonoInstaller
    {
        private readonly CharacterSystemsInstaller _characterSystemInstaller = new();

        public override void InstallBindings()
        {
            Container.Bind<CharacterProvider>().AsSingle();
            
            Container.Install(_characterSystemInstaller);
        }
    }
}