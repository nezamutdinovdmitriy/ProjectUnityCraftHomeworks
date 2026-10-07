using Game.Components;
using Game.Predicates;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace Game.UseCases
{
    public static class AIUseCase
    {
        public static Entity FindClosestTarget<TPredicate>(NativeList<DistanceHit> hits, in TPredicate predicate)
            where TPredicate : struct, ITargetPredicate
        {
            TPredicate condition = predicate;
            
            Entity closest = Entity.Null;
            float closestDistance = float.MaxValue;

            foreach (DistanceHit hit in hits)
            {
                if (condition.Invoke(hit.Entity) == false)
                    continue;

                if (hit.Distance < closestDistance)
                {
                    closestDistance = hit.Distance;
                    closest = hit.Entity;
                }
            }

            return closest;
        }
        
        public static bool IsExpired(this in TargetDetectionCooldown timer)
        {
            return timer.Time <= 0;
        }
        
        public static bool IsEnemy(this in Team selfTeam, in Team candidateTeam)
        {
            return selfTeam.Value != candidateTeam.Value;
        }
    }
}