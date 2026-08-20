namespace Modules.AI
{
    public interface ICondition : IFunction<bool>
    {
    }
    
    public interface ICondition<in T> : IFunction<T, bool>
    {
    }
}