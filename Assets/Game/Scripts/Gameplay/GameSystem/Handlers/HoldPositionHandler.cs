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
            if (Input.GetKeyDown(_keyCode))
            {
                _character
                    .GetComponentInChildren<Blackboard>()
                    .SetReferenceValue(BlackboardAPI.CurrentCommand, new HoldPositionCommandData());
                // TODO: Hold Position
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}