using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    public partial struct AmmoRestoreSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new AmmoRestoreJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        private partial struct AmmoRestoreJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                ref Ammo ammo,
                in MaxAmmo maxAmmo,
                ref AmmoRestoreCooldown cooldown,
                in AmmoRestoreAmount amount)
            {
                if (ammo.IsFull(maxAmmo))
                    return;

                if (cooldown.IsExpired() == false)
                    cooldown.Time -= DeltaTime;

                if (cooldown.IsExpired())
                {
                    ammo.Value = math.min(maxAmmo.Value, ammo.Value + amount.Value);
                    cooldown.ResetCooldown();
                }
            }
        }
    }
}