using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class UnitPurchaseCatalogAuthoring : MonoBehaviour
    {
        [SerializeField] private UnitCardsCatalog _catalog;

        private sealed class Baker : Baker<UnitPurchaseCatalogAuthoring>
        {
            public override void Bake(UnitPurchaseCatalogAuthoring authoring)
            {
                DependsOn(authoring._catalog);
                
                if (authoring._catalog.Cards == null || authoring._catalog.Cards.Count == 0)
                {
                    Debug.Log("UnitPurchaseCatalog: the catalog is empty.", authoring);
                    return;
                }

                Entity entity = GetEntity(TransformUsageFlags.None);

                DynamicBuffer<UnitPurchaseItem> items = AddBuffer<UnitPurchaseItem>(entity);

                foreach (UnitCardConfig card in authoring._catalog.Cards)
                {
                    items.Add(new UnitPurchaseItem
                    {
                        Prefab = GetEntity(card.Prefab, TransformUsageFlags.None),
                        Price = card.Price
                    });
                }
            }
        }
    }
}