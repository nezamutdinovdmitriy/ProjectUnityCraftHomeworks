using Game.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(TransformSystemGroup), OrderFirst = true)]
    public partial struct VFXFollowTargetSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup =
                SystemAPI.GetComponentLookup<LocalTransform>(true);
        }
        
        public void OnUpdate(ref SystemState state)
        {
            _transformLookup.Update(ref state);
            
            foreach (var (
                         follow,
                         transform) 
                     in SystemAPI.Query<
                         RefRO<VFXFollowTarget>,
                         RefRW<LocalTransform>>())
            {
                Entity target = follow.ValueRO.Target;

                if (_transformLookup.TryGetRefRO(
                        follow.ValueRO.Target,
                        out RefRO<LocalTransform> targetTransform) == false)
                    continue;

                transform.ValueRW.Position = targetTransform.ValueRO.Position + follow.ValueRO.Offset;
            }
        }
    }
}