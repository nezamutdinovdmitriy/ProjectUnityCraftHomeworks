using Unity.Entities;

namespace Game.Predicates
{
    public interface ITargetPredicate
    {
        bool Invoke(Entity entity);
    }
}