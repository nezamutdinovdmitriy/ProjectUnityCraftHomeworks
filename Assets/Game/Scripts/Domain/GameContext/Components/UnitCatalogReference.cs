using SampleGame;
using Unity.Entities;

namespace Game.Components
{
    public struct UnitCatalogReference : IComponentData
    {
        public UnityObjectRef<UnitCardsCatalog> Catalog;
    }
}