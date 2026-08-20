using UnityEngine;

namespace Modules.FSM
{
    public abstract partial class State : MonoBehaviour
    {
        public abstract void OnEnter();

        public abstract void OnUpdate(float deltaTime);

        public abstract void OnExit();
    }
}