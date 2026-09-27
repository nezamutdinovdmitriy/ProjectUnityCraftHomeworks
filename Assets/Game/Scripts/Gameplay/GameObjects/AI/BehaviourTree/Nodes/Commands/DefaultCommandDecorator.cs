using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class DefaultCommandDecorator : BehaviourNode
    {
        [SerializeField] private Blackboard _blackboard;
        [SerializeField] private BehaviourNode _child;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            EnsureCurrentCommand();
            return _child.Run(deltaTime);
        }

        protected override void OnAbort()
        {
            if(_child.IsRunning)
                _child.Abort();
        }

        private void EnsureCurrentCommand()
        {
            if (_blackboard.HasValue(BlackboardAPI.CurrentCommand))
                return;
            
            ICommandData command = new DefaultCommandData(
                new CommandPoint(_blackboard.GetValue(BlackboardAPI.Character).transform.position));

            _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, command);
        }
    }
}