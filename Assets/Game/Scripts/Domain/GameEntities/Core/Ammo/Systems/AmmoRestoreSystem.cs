using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    [BurstCompile]
    public partial struct AmmoRestoreSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRW<Ammo> ammo,
                         RefRO<MaxAmmo> maxAmmo,
                         RefRW<AmmoRestoreCooldown> restoreCooldown,
                         RefRO<AmmoRestoreAmount> restoreAmount)
                     in SystemAPI.Query<
                         RefRW<Ammo>,
                         RefRO<MaxAmmo>,
                         RefRW<AmmoRestoreCooldown>,
                         RefRO<AmmoRestoreAmount>>())
            {
                float deltaTime = SystemAPI.Time.DeltaTime;

                if(ammo.ValueRO.IsFull(in maxAmmo.ValueRO))
                    continue;
                
                if (restoreCooldown.ValueRO.IsExpired() == false)
                    restoreCooldown.ValueRW.Time -= deltaTime;

                if (restoreCooldown.ValueRO.IsExpired()
                    && ammo.ValueRO.IsFull(in maxAmmo.ValueRO) == false)
                {
                    ammo.ValueRW.Value = math.min(
                        maxAmmo.ValueRO.Value, 
                        ammo.ValueRO.Value + restoreAmount.ValueRO.Value);
                    
                    restoreCooldown.ValueRW.ResetCooldown();
                }
            }
        }
    }
}