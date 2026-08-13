using Zenject;

namespace GameContexts
{
    public class CharacterSystemsInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<InputService>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerAttackController>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerJumpController>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerMoveController>()
                .AsSingle();
        }
    }
}