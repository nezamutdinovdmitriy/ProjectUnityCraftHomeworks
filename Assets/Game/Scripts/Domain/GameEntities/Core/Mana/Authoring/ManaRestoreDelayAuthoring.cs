using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ManaRestoreDelayAuthoring : MonoBehaviour
    {
        [SerializeField] private float _restoreDelay;
        
        private sealed class Baker : Baker<ManaRestoreDelayAuthoring>
        {
            public override void Bake(ManaRestoreDelayAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ManaRestoreDelay
                {
                    Delay = authoring._restoreDelay
                });
                
                SetComponentEnabled<ManaRestoreDelay>(entity, false);
                
                AddComponent(entity, new ManaRestoreEvent());
                SetComponentEnabled<ManaRestoreEvent>(entity, false);
            }
        }
    }
}