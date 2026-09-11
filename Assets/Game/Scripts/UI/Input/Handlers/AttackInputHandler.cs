using Modules.AI;
using SampleGame.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class AttackInputHandler : InputHandler
    {
        [SerializeField]
        private KeyCode _keyCode = KeyCode.A;

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

                AttackCommandData? attackCommand = null; 
                
                if (context.point != null)
                {
                    attackCommand = new AttackCommandData(new CommandPoint(context.point));
                    _markersView.ShowAttackMarker(context.point.Value);
                    // TODO: Attack Position
                }
                else if (context.target != null && context.target != _character)
                {
                    attackCommand = new AttackCommandData(new CommandPoint(context.target));
                    _markersView.ShowAttackMarker(context.target.transform);
                    // Тут возможно стоит сразу проставлять таргета в blackboard?.
                    // TODO: Attack Target
                }

                if (attackCommand == null)
                    return;

                if (context.enqueueCommand)
                    blackboard.GetValue(BlackboardAPI.CommandQueue).Enqueue(attackCommand);
                else
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, attackCommand);
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}