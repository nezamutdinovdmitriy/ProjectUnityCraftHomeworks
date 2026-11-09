using Game.Components;
using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class UpdateTimersSystemGroup : ComponentSystemGroup
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