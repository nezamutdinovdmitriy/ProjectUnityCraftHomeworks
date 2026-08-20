using System;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable UnusedMemberInSuper.Global

namespace Modules.AI
{
    public abstract partial class BehaviourNode : MonoBehaviour, IBehaviourNode
    {
        internal event Action OnStarted;
        internal event Action OnStopped;
        internal event Action OnAborted;

        [Title("Debug")]
        [HideInEditorMode]
        [ShowInInspector]
        public bool IsRunning => _isRunning;

        [HideInEditorMode]
        [ShowInInspector]
        public BehaviourResult Result => result;

        private BehaviourResult result;
        private bool _isRunning;

        [HideInEditorMode]
        [Button]
        public BehaviourResult Run(float deltaTime)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                this.OnStart();
                this.OnStarted?.Invoke();
            }

            BehaviourResult result = this.OnUpdate(deltaTime);
            this.result = result;

            if (result != BehaviourResult.Running)
            {
                this.OnStop(result);
                _isRunning = false;
                this.OnStopped?.Invoke();
            }

            return result;
        }

        [HideInEditorMode]
        [Button]
        public void Abort()
        {
            if (!_isRunning)
                return;

            this.OnAbort();
            result = BehaviourResult.Aborted;
            this.OnAborted?.Invoke();

            this.OnStop(BehaviourResult.Aborted);
            _isRunning = false;
            this.OnStopped?.Invoke();
        }

        protected abstract BehaviourResult OnUpdate(float deltaTime);

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop(BehaviourResult result)
        {
        }

        protected virtual void OnAbort()
        {
        }
    }
}