using Modules.AI;

namespace SampleGame.AI
{
    public interface ICommandData
    {
        public CommandType Type { get; }
        public void Unpack(Blackboard blackboard);
        public void Cleanup(Blackboard blackboard);
    }
}