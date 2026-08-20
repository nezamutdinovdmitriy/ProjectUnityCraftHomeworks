using UnityEngine;

namespace SampleGame
{
    public sealed class MoveInputHandler : InputHandler
    {
        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;

        public override void Handle(ref InputContext context)
        {
            if (context.rightClick)
            {
                if (context.target != null && context.target != _character)
                {
                    // TODO: Move to target
                }
                else if (context.point != null)
                {
                    // TODO: Move to point
                }
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}