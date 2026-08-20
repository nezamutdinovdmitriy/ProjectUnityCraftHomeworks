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
                if (context.point != null)
                {
                    // TODO: Attack Position
                }
                else if (context.target != null && context.target != _character)
                {
                    // TODO: Attack Target
                }
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}