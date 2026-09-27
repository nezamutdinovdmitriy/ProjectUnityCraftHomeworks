using Modules.AI;

namespace SampleGame.AI
{
    public readonly struct DefaultCommandData : ICommandData, IHasCommandPoint
    {
        private readonly CommandPoint _point;

        public DefaultCommandData(CommandPoint point) 
            => _point = point;

        public CommandType Type => CommandType.Default;

        public CommandPoint Point => _point;
                
        public void Unpack(Blackboard blackboard)
        {
            if (_point.Position.HasValue)
                blackboard.SetPrimitiveValue(BlackboardAPI.TargetPosition, _point.Position.Value);
        }

        public void Cleanup(Blackboard blackboard)
        {
            blackboard.DelValue(BlackboardAPI.TargetPosition);
        }
    }
}