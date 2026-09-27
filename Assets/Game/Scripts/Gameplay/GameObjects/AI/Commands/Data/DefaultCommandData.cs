using Modules.AI;

namespace SampleGame.AI
{
    public readonly struct DefaultCommandData : ICommandData
    {
        private readonly CommandPoint _point;

        public DefaultCommandData(CommandPoint point) 
            => _point = point;

        public CommandType Type => CommandType.Default;
                
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