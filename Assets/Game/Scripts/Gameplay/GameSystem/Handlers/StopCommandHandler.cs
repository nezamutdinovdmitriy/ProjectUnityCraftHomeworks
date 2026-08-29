using Modules.AI;
using SampleGame.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class StopCommandHandler : InputHandler
    {
        [SerializeField]
        private KeyCode _keyCode = KeyCode.S;
        
        [SerializeField]
        private GameObject _character;
        
        [SerializeField]
        private InputHandler _next;
        
        public override void Handle(ref InputContext context)
        {
            if (Input.GetKeyDown(_keyCode))
            {
                Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
                blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new StopCommandData());
                // TODO: Stop
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}