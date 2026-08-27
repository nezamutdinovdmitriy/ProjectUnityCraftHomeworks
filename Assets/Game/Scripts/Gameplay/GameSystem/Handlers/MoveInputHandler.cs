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
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, 
                        new MoveCommandData(context.target));
                    // TODO: Move to target
                }
                else if (context.point != null)
                {
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new MoveCommandData(context.point));
                    // TODO: Move to point
                }
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}