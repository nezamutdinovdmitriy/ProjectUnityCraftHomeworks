using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class HealAmountAuthoring : MonoBehaviour
    {
        [SerializeField] private float _healAmount;

        private sealed class Baker : Baker<HealAmountAuthoring>
        {
            public override void Bake(HealAmountAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new HealAmount {Value = authoring._healAmount});
            }
        }
    }
}