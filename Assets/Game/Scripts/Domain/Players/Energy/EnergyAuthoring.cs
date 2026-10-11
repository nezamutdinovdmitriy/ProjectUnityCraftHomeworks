using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class EnergyAuthoring : MonoBehaviour
    {
        [SerializeField] private int _energy;

        [SerializeField] private int _income;
        [SerializeField] private float _incomeCooldown;
        
        private sealed class Baker : Baker<EnergyAuthoring>
        {
            public override void Bake(EnergyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Energy {Value = authoring._energy});
                AddComponent(entity, new EnergyIncomeCooldown
                {
                    Income = authoring._income,
                    Time = authoring._incomeCooldown,
                    Cooldown = authoring._incomeCooldown
                });
            }
        }
    }
}