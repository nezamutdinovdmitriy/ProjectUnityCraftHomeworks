using UnityEngine;

namespace SampleGame
{
    public abstract class InputHandler : MonoBehaviour
    {
        public abstract void Handle(ref InputContext context);
    }
}