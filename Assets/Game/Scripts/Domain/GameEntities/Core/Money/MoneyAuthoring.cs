using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class MoneyAuthoring : MonoBehaviour
    {
        [SerializeField] private int _money;

        private sealed class Baker : Baker<MoneyAuthoring>
        {
            public override void Bake(MoneyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Money {Value = authoring._money});
            }
        }
    }
}