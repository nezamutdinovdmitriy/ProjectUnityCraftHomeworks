using Game.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class AmmoRestoreAuthoring : MonoBehaviour
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private int _amount;
        
        private sealed class Baker : Baker<AmmoRestoreAuthoring>
        {
            public override void Bake(AmmoRestoreAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new AmmoRestoreCooldown
                {
                    Cooldown = authoring._cooldown
                });
                
                AddComponent(entity, new AmmoRestoreAmount
                {
                    Value = authoring._amount
                });
            }
        }
    }
}