using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «Aborter»")]
    public class BehaviourNodeAborter : BehaviourNode, IBehaviourNodeDecorator, ISerializationCallbackReceiver
    {
        public BehaviourNode Child => _origin;

        [Space]
        [SerializeField]
        private BehaviourNode _origin;

        [Space]
        [SerializeReference]
        private ICondition[] _conditions = Array.Empty<ICondition>();

        [Space]
        [SerializeReference]
        private IAction _abort;
        
        [Space]
        [SerializeField]
        private UnityEvent _abortEvent;
        
        [HideInEditorMode]
        [ShowInInspector, ReadOnly]
        private bool[] _states;

        protected override void OnStart()
        {
            for (int i = 0; i < _conditions.Length; i++) 
                _states[i] = _conditions[i].Invoke();
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            this.TryAbort();
            return _origin.Run(deltaTime);
        }

        private void TryAbort()
        {
            bool abortRequired = false;

            if (_conditions != null)
            {
                for (int i = 0; i < _conditions.Length; i++)
                {
                    bool currentState = _conditions[i].Invoke();
                    if (currentState != _states[i])
                    {
                        _states[i] = currentState;
                        abortRequired = true;
                    }
                }
            }


            if (abortRequired)
            {
                _origin.Abort();
                _abortEvent.Invoke();
                _abort?.Invoke();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_conditions != null)
                _states = new bool[_conditions.Length];
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }
    }
}