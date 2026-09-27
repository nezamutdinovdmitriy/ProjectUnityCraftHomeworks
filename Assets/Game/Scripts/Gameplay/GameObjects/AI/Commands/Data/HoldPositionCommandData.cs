using Modules.AI;

namespace SampleGame.AI
{
    public struct HoldPositionCommandData : ICommandData
    {
        public CommandType Type => CommandType.HoldPosition;
        
        public void Unpack(Blackboard blackboard)
        {
        }

        public void Cleanup(Blackboard blackboard)
        {
        }
    }
}