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

        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
                if (context.point != null)
                {
                    // TODO: Follow point
                }
                else if (context.target != null && context.target != _character)
                {
                    // TODO: Follow target
                }
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}