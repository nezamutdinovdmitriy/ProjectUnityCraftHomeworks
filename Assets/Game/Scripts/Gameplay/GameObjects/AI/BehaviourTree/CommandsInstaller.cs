using Modules.AI;

namespace SampleGame.AI
{
    public class CommandsInstaller : IBlackboardInstaller
    {
        public void Install(Blackboard blackboard)
        {
            blackboard.AddPrimitiveValue(BlackboardAPI.CurrentCommandType, CommandType.None);
            blackboard.AddReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
        }
    }
}