using System.Collections.Generic;
using System.Linq;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;

namespace SampleGame.Gameplay
{
    public sealed partial class JsonComponentVisitor : IComponentVisitor
    {
        private readonly EntityCatalog _entityCatalog;
        private readonly EntityWorld _entityWorld;

        private VisitorMode _mode;

        public JsonComponentVisitor(EntityCatalog entityCatalog, EntityWorld entityWorld)
        {
            _entityCatalog = entityCatalog;
            _entityWorld = entityWorld;
        }

        public JToken SaveData { get; private set; }

        public void PrepareForSave()
        {
            _mode = VisitorMode.Save;
            ClearData();
        }

        public void PrepareForLoad(JToken componentData)
        {
            _mode = VisitorMode.Load;
            SaveData = componentData;
        }

        public void ClearData() => SaveData = null;
    }
}