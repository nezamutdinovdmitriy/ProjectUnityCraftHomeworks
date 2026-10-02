using Game.Components;
using Unity.Entities;

namespace Game.Scripts.Domain.GameEntities.Core.TakeDamage
{
    public static class TakeDamageUseCase
    {
        public static void ApplyDamage(
            DynamicBuffer<TakeDamageEvent> events,
            DynamicBuffer<TakeDamageRequest> requests,
            ref Health health,
            float multiplier = 1f)
        {
            for (int i = 0; i < requests.Length && health.IsAlive(); i++)
            {
                float damage = requests[i].Damage * multiplier;
                    
                health.ReduceHealth(damage);

                events.Add(new TakeDamageEvent
                {
                    Damage = damage
                });
            }
                
            requests.Clear();
        }
    }
}