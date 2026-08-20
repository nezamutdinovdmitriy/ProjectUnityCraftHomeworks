namespace Modules.AI
{
    public interface IAction
    {
        void Invoke();
    }

    public interface IAction<in T>
    {
        void Invoke(T arg);
    }
    
    public interface IAction<in T1, in T2>
    {
        void Invoke(T1 arg1, T2 arg2);
    }
}