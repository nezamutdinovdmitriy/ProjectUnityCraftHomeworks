using Game.Components;
using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class UpdateTimersSystemGroup : ComponentSystemGroup
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