using Modules.AI;

namespace SampleGame.AI
{
    public readonly struct FollowCommandData : ICommandData, IHasCommandPoint
    {
        private readonly CommandPoint _point;

        public FollowCommandData(CommandPoint point) 
            => _point = point;

        public CommandType Type => CommandType.Follow;

        public CommandPoint Point => _point;
        
        public void Unpack(Blackboard blackboard)
        {
            if (_point.Target != null)
                blackboard.SetReferenceValue(BlackboardAPI.Target, _point.Target);
        }

        public void Cleanup(Blackboard blackboard)
        {
            blackboard.DelValue(BlackboardAPI.Target);
        }
    }
}