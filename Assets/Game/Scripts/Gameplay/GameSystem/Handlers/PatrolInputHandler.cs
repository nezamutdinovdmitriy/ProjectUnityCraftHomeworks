using Modules.AI;
using SampleGame.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class PatrolInputHandler : InputHandler
    {
        [SerializeField]
        private KeyCode _keyCode = KeyCode.P;

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
                ICommandData currentCommand = blackboard.GetValue(BlackboardAPI.CurrentCommand);

                CommandPoint? point = null;

                if (context.point != null)
                {
                    point = new CommandPoint(context.point);
                    _markersView.ShowPatrolMarker(context.point.Value);
                    // TODO: Point destination
                }
                else if (context.target != null && context.target != _character)
                {
                    point = new CommandPoint(context.target);
                    _markersView.ShowPatrolMarker(context.target.transform);
                    // TODO: Target destination
                }

                if (point == null)
                    return;
                
                if (context.enqueueCommand)
                {
                    if (currentCommand is PatrolCommandData currentPatrol)
                    {
                        currentPatrol.Points.Add(point.Value);
                        blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, currentPatrol);
                    }
                    else
                    {
                        PatrolCommandData newPatrolCommand = new PatrolCommandData(_character.transform.position);
                        newPatrolCommand.Points.Add(point.Value);
                        
                        blackboard.GetValue(BlackboardAPI.CommandQueue).Enqueue(newPatrolCommand);
                    }

                    // TODO: If current command is patrol the add waypoint else enqueue command
                }
                else
                {
                    PatrolCommandData newPatrolCommand = new PatrolCommandData(_character.transform.position);
                    newPatrolCommand.Points.Add(point.Value);
                    
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, newPatrolCommand);
                    // TODO: Switch to patrol
                }
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}