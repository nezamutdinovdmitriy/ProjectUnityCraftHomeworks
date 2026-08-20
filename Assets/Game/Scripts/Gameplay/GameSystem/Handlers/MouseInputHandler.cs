using UnityEngine;

namespace SampleGame
{
    public sealed class MouseInputHandler : InputHandler
    {
        private const int LEFT_MOUSE = 0;
        private const int RIGHT_MOUSE = 1;

        [SerializeField]
        private InputHandler _next;

        public override void Handle(ref InputContext context)
        {
            context.mousePosition = Input.mousePosition;
            context.leftClick = Input.GetMouseButtonDown(LEFT_MOUSE);
            context.rightClick = Input.GetMouseButtonDown(RIGHT_MOUSE);

            if (_next)
                _next.Handle(ref context);
        }
    }
}