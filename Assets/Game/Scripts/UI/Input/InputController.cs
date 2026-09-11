using UnityEngine;

namespace SampleGame
{
    public sealed class InputController : MonoBehaviour
    {
        [SerializeField]
        private InputHandler _handler;

        private void Update()
        {
            InputContext context = new InputContext();
            _handler.Handle(ref context);
        }
    }
}