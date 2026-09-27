using Modules.AI;

namespace SampleGame.AI
{
    public struct StopCommandData : ICommandData
    {
        public CommandType Type => CommandType.Stop;
        
        public void Unpack(Blackboard blackboard)
        {
        }

        public void Cleanup(Blackboard blackboard)
        {
        }
    }
}