using UnityEngine;

// ReSharper disable ForCanBeConvertedToForeach

namespace Modules.FSM
{
    public sealed class CompositeState : State
    {
        [SerializeField]
        private State[] _states;

        public override void OnEnter()
        {
            for (int i = 0; i < _states.Length; i++)
                _states[i].OnEnter();
        }

        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _states.Length; i++)
                _states[i].OnUpdate(deltaTime);
        }

        public override void OnExit()
        {
            for (int i = 0; i < _states.Length; i++)
                _states[i].OnExit();
        }
    }
}