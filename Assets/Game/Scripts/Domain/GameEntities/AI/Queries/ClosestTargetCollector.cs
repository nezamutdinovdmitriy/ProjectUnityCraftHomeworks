using Game.Predicates;
using Unity.Entities;
using Unity.Physics;

namespace Game.UseCases.Queries
{
    public struct ClosestTargetCollector<TPredicate> : ICollector<DistanceHit>
        where TPredicate : struct, ITargetPredicate
    {
        private TPredicate _predicate;

        public bool EarlyOutOnFirstHit => false;
        
        public float MaxFraction { get; private set; }
        public int NumHits { get; private set; }
        public Entity ClosestEntity { get; private set; }

        public ClosestTargetCollector(float radius, in TPredicate predicate)
        {
            _predicate = predicate;
            MaxFraction = radius;
            NumHits = 0;
            ClosestEntity = Entity.Null;
        }
        
        public bool AddHit(DistanceHit hit)
        {
            if (_predicate.Invoke(hit.Entity) == false)
                return false;

            if (NumHits > 0 && hit.Distance >= MaxFraction)
                return false;

            MaxFraction = hit.Distance;
            ClosestEntity = hit.Entity;
            NumHits = 1;

            return true;
        }
    }
}