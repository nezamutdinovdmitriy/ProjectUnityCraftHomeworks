
namespace Modules.AI
{
    public interface IFunction<out R>
    {
        R Invoke();
    }

    public interface IFunction<in T, out R>
    {
        R Invoke(T arg);
    }

    public interface IFunction<in T1, in T2, out R>
    {
        R Invoke(T1 arg1, T2 arg2);
    }
}