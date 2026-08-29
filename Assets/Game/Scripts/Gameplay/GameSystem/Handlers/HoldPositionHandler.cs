using Modules.AI;
using SampleGame.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class HoldPositionHandler : InputHandler
    {
        [SerializeField]
        private KeyCode _keyCode = KeyCode.H;

        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;

        public override void Handle(ref InputContext context)
        {              
            Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
            
            if (Input.GetKeyDown(_keyCode))
            {
                if (context.enqueueCommand)
                {
                    blackboard.GetValue(BlackboardAPI.CommandQueue).Enqueue(new HoldPositionCommandData());
                    return;
                }
                
                blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new HoldPositionCommandData());
                // TODO: Hold Position
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}