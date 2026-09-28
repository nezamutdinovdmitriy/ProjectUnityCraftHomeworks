using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ManaAuthoring : MonoBehaviour
    {
        [SerializeField] private float _mana;

        private class Baker : Baker<ManaAuthoring>
        {
            public override void Bake(ManaAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Mana {Value = authoring._mana});
                AddComponent(entity, new MaxMana {Value = authoring._mana});
            }
        }
    }
}