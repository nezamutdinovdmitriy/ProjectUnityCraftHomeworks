using Game.Components;
using Game.UseCases;
using Unity.Entities;

namespace Game.Systems
{
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
                    && ammo.ValueRO.Value < maxAmmo.ValueRO.Value)
                {
                    ammo.ValueRW.Value += restoreAmount.ValueRO.Value;
                    restoreCooldown.ValueRW.ResetCooldown();
                }
            }
        }
    }
}