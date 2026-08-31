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
            blackboard.AddReferenceValue(BlackboardAPI.CurrentCommand, 
                new DefaultCommandData(
                    new CommandPoint(blackboard.GetValue(BlackboardAPI.Character).transform.position)));
            blackboard.AddReferenceValue(BlackboardAPI.CommandQueue, commandQueue);
        }
    }
}