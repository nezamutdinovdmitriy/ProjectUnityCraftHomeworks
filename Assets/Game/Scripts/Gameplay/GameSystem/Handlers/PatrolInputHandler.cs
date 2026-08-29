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

        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
                Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
                ICommandData commandData = blackboard.GetValue(BlackboardAPI.CurrentCommand);

                if (commandData is not PatrolCommandData patrolData)
                {
                    patrolData = new PatrolCommandData(_character.transform.position);
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, patrolData);
                }
                
                if (context.point != null)
                {
                    patrolData.Points.Add(new PatrolCommandData.Point(context.point));
                    
                    // TODO: Point destination
                }
                else if (context.target != null && context.target != _character)
                {
                    patrolData.Points.Add(new PatrolCommandData.Point(context.target));
                    
                    // TODO: Target destination
                }

                // if (context.enqueueCommand)
                // {
                //     // TODO: If current command is patrol the add waypoint else enqueue command
                // }
                // else
                // {
                //     // TODO: Switch to patrol
                // }
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}