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

        [SerializeField]
        private CommandMarkerView _markersView;
        
        public override void Handle(ref InputContext context)
        {              
            Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
            
            if (Input.GetKeyDown(_keyCode))
            {
                _markersView.ShowHoldPositionMarker(_character.transform.position);
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