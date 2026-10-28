using Game.Components;
using Game.Predicates;
using Game.UseCases.Queries;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Game.UseCases
{
    public static class AIUseCase
    {
        public static Entity FindClosestTarget<TPredicate>(
            ref PhysicsWorldSingleton physicsWorld,
            float3 center,
            float radius,
            in TPredicate predicate)
            where TPredicate : struct, ITargetPredicate
        {
            if (radius > 0f)
            {
                var collector = new ClosestTargetCollector<TPredicate>(radius, in predicate);

                physicsWorld.OverlapSphereCustom(center, radius, ref collector, CollisionFilter.Default);

                return collector.ClosestEntity;
            }

            return Entity.Null;
        }
        
        public static bool IsExpired(this in TargetDetectionCooldown timer)
        {
            return timer.Time <= 0;
        }
    }
}