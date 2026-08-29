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

        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
                Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
                
                if (context.point != null)
                {
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new AttackCommandData(context.point));   
                    // TODO: Attack Position
                }
                else if (context.target != null && context.target != _character)
                {
                    blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new AttackCommandData(context.target));
                    // Тут возможно стоит сразу проставлять таргета в blackboard'е.
                    // TODO: Attack Target
                }
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}