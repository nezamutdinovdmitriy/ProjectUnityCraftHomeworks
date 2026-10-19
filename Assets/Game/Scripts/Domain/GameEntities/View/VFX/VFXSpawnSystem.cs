using Game.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct VFXSpawnSystem : ISystem
    {
        private ComponentLookup<LocalToWorld> _worldTransformLookup;
        private ComponentLookup<LocalTransform> _localTransformLookup;

        public void OnCreate(ref SystemState state)
        {
            _worldTransformLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
            _localTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);

            state.RequireForUpdate<VFXSpawnRequest>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _worldTransformLookup.Update(ref state);
            _localTransformLookup.Update(ref state);

            EntityCommandBuffer commands = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var requests
                     in SystemAPI.Query<DynamicBuffer<VFXSpawnRequest>>())
            {
                foreach (VFXSpawnRequest request in requests)
                {
                    if (_worldTransformLookup.TryGetRefRO(
                            request.Target,
                            out RefRO<LocalToWorld> targetTransform) == false
                        || _localTransformLookup.TryGetRefRO(
                            request.Prefab,
                            out RefRO<LocalTransform> prefabTransform) == false)
                        continue;

                    LocalTransform transform = prefabTransform.ValueRO;
                    transform.Position = targetTransform.ValueRO.Position + request.Offset;

                    Entity instance = commands.Instantiate(request.Prefab);
                    commands.SetComponent(instance, transform);
                    
                    if(request.FollowTarget)
                    {
                        commands.AddComponent(instance, new VFXFollowTarget
                        {
                            Target = request.Target,
                            Offset = request.Offset
                        });
                    }
                }
                
                requests.Clear();
            }
        }
    }
}