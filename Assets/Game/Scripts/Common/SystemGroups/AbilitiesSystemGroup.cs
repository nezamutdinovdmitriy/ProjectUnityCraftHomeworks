using Game.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(GenerateRequestsSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class AbilitiesSystemGroup : ComponentSystemGroup
    {
        protected override void OnUpdate()
        {
            if (SystemAPI.TryGetSingleton(out GameTime time) == false
                || time.DeltaTime <= 0f)
                return;
            
            base.OnUpdate();
        }
    }
}