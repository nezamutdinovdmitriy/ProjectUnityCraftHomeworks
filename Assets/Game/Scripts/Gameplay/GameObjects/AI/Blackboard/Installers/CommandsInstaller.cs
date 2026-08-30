using System.Collections.Generic;
using Modules.AI;
using Sirenix.OdinInspector;

namespace SampleGame.AI
{
    public class CommandsInstaller : IBlackboardInstaller
    {
        [ShowInInspector, HideInEditorMode]
        private Queue<ICommandData> commandQueue = new();
        
        public void Install(Blackboard blackboard)
        {
            blackboard.AddPrimitiveValue(BlackboardAPI.CurrentCommandType, CommandType.None);
            blackboard.AddReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
            blackboard.AddReferenceValue(BlackboardAPI.CommandQueue, commandQueue);
        }
    }
}