using Modules.AI;
using SampleGame.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class FollowInputHandler : InputHandler
    {
        [SerializeField]
        private KeyCode _keyCode = KeyCode.F;

        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;

        [SerializeField]
        private CommandMarkerView _markersView;
        
        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
                Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
                
                FollowCommandData? followCommand = null;
                
                if (context.point != null)
                {
                    // ???
                    // TODO: Follow point
                }
                else if (context.target != null && context.target != _character)
                {
                    followCommand = new FollowCommandData(new CommandPoint(context.target));
                    _markersView.ShowFollowMarker(context.target.transform);
                    // TODO: Follow target
                }

                if (followCommand == null)
                    return;

                if (context.enqueueCommand)
                    blackboard.GetValue(BlackboardAPI.CommandQueue).Enqueue(followCommand);
                else
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, followCommand);
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}