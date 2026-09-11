using UnityEngine;

namespace SampleGame
{
    public sealed class EnqueueCommandHandler : InputHandler
    {
        [SerializeField]
        private InputHandler _next;

        public override void Handle(ref InputContext context)
        {
            context.enqueueCommand = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);

            if (_next)
                _next.Handle(ref context);
        }
    }
}