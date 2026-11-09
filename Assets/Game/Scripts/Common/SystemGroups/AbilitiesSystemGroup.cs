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
            if (!SystemAPI.TryGetSingleton(out GameState state) ||
                state.Value != GameStateType.Playing)
                return;
            
            base.OnUpdate();
        }
    }
}