namespace Atomic.Entities
{
    public class SceneEntityInstallerSpy : SceneEntityInstaller
    {
        public bool Installed { get; private set; }
        public bool Uninstalled { get; private set; }

        public override void Install(IEntity entity) => this.Installed = true;
        public override void Uninstall(IEntity entity) => this.Uninstalled = true;
    }
}