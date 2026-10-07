using Game.Predicates;
using Unity.Entities;

namespace Game.UseCases
{
    public static class TargetUseCase
    {
        public static Entity FindClosest<TCondition>(in TCondition condition) 
            where TCondition: struct, ITargetPredicate
        {
            return new Entity();
        }
    }
}