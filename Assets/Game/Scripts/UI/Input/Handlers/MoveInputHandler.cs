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

        [SerializeField]
        private CommandMarkerView _markersView;
        
        public override void Handle(ref InputContext context)
        {
            if (context.rightClick)
            {
                Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
                CommandPoint? commandPoint = null;
                
                if (context.target != null && context.target != _character)
                {
                    commandPoint = new CommandPoint(context.target);
                    _markersView.ShowMoveMarker(context.target.transform);
                     // TODO: Move to target
                }
                else if (context.point != null)
                {
                    commandPoint = new CommandPoint(context.point);
                    _markersView.ShowMoveMarker(context.point.Value);
                    // TODO: Move to point
                }

                if (commandPoint == null)
                    return;

                if (context.enqueueCommand)
                    blackboard.GetValue(BlackboardAPI.CommandQueue)
                        .Enqueue(new MoveCommandData(commandPoint.Value));
                else
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand,
                        new MoveCommandData(commandPoint.Value));
                
                // blackboard.AddTag(BlackboardAPI.MoveCommandTag);
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}