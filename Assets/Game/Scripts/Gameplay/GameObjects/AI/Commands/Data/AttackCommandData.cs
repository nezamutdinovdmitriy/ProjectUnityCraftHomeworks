using Modules.AI;

namespace SampleGame.AI
{
    public readonly struct AttackCommandData : ICommandData, IHasCommandPoint
    {
        private readonly CommandPoint _point;
        
        public AttackCommandData(CommandPoint point) => _point = point;
        public CommandType Type => CommandType.Attack;
        public CommandPoint Point => _point;
        
        public void Unpack(Blackboard blackboard)
        {
            if (_point.Target != null)
            {
                blackboard.SetReferenceValue(BlackboardAPI.Target, _point.Target);
                return;
            }

            if (_point.Position.HasValue)
                blackboard.SetPrimitiveValue(BlackboardAPI.TargetPosition, _point.Position.Value);
        }

        public void Cleanup(Blackboard blackboard)
        {
            blackboard.DelValue(BlackboardAPI.Target);
            blackboard.DelValue(BlackboardAPI.TargetPosition);
        }
    }
}