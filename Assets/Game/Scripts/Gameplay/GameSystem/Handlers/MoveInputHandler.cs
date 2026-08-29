using Modules.AI;
using SampleGame.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class MoveInputHandler : InputHandler
    {
        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;
        
        
        public override void Handle(ref InputContext context)
        {
            if (context.rightClick)
            {
                Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
                
                if (context.target != null && context.target != _character)
                {
                    if (context.enqueueCommand)
                    {
                        blackboard.GetValue(BlackboardAPI.CommandQueue)
                            .Enqueue(new MoveCommandData(new CommandPoint(context.target)));
                        return;
                    }
                    
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, 
                        new MoveCommandData(new CommandPoint(context.target)));
                    // TODO: Move to target
                }
                else if (context.point != null)
                {
                    if (context.enqueueCommand)
                    {
                        blackboard.GetValue(BlackboardAPI.CommandQueue)
                            .Enqueue(new MoveCommandData(new CommandPoint(context.point)));
                        return;
                    }
                    
                    blackboard.SetReferenceValue(
                        BlackboardAPI.CurrentCommand, 
                        new MoveCommandData(new CommandPoint(context.point)));
                    // TODO: Move to point
                }
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}