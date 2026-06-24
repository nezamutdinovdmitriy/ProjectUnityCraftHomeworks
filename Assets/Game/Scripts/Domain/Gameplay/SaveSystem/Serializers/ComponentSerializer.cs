using Modules.Entities;

namespace SampleGame.Gameplay
{
    public sealed partial class ComponentSerializer : IComponentSerializer
    {
        private readonly EntityCatalog _entityCatalog;
        private readonly EntityWorld _entityWorld;

        public ComponentSerializer(EntityCatalog entityCatalog, EntityWorld entityWorld)
        {
            _entityCatalog = entityCatalog;
            _entityWorld = entityWorld;
        }
    }
}