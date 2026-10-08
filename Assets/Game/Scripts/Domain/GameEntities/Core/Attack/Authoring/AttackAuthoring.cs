using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class AttackAuthoring : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _distance;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _hitDelay;

        private sealed class Baker : Baker<AttackAuthoring>
        {
            public override void Bake(AttackAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new AttackCooldown
                {
                    Duration = authoring._cooldown
                });
                SetComponentEnabled<AttackCooldown>(entity, false);
                
                AddComponent(entity, new AttackDistance
                {
                    Value = authoring._distance
                });
                
                AddComponent(entity, new Damage
                {
                    Value = authoring._damage
                });
                
                AddComponent(entity, new AttackHitDelay
                {
                    Delay = authoring._hitDelay
                });
                SetComponentEnabled<AttackHitDelay>(entity, false);
                
                AddComponent(entity, new AttackHitEvent());
                SetComponentEnabled<AttackHitEvent>(entity, false);
                
                AddComponent(entity, new AttackInProcess());
                SetComponentEnabled<AttackInProcess>(entity, false);
                
                AddComponent(entity, new AttackStartedEvent());
                SetComponentEnabled<AttackStartedEvent>(entity, false);
                
                AddComponent(entity, new AttackStartedRequest());
                SetComponentEnabled<AttackStartedRequest>(entity, false);
            }
        }
    }
}