using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct HealVFXRequestSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<HealVFXSettings>();
            state.RequireForUpdate<VFXSpawnRequest>();
        }

        public void OnUpdate(ref SystemState state)
        {
            HealVFXSettings settings = SystemAPI
                .GetSingleton<HealVFXSettings>();

            if (settings.Prefab == Entity.Null)
                return;

            DynamicBuffer<VFXSpawnRequest> requests = SystemAPI
                .GetSingletonBuffer<VFXSpawnRequest>();

            foreach (var (events, target) 
                     in SystemAPI.Query<DynamicBuffer<HealEvent>>()
                         .WithEntityAccess())
            {
                foreach (HealEvent healEvent in events)
                {
                    if (healEvent.Value <= 0f)
                        continue;

                    requests.Add(new VFXSpawnRequest
                    {
                        Prefab = settings.Prefab,
                        Target = target,
                        Offset = settings.Offset,
                        FollowTarget = true
                    });
                }
            }
        }
    }
}