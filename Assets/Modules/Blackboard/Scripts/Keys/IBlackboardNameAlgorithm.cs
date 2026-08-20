namespace Modules.AI
{
    public interface IBlackboardNameAlgorithm
    {
        int NameToId(string name);

        void Reset();
    }
}