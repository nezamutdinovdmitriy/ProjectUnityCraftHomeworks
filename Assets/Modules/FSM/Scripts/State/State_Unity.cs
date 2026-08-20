using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.FSM
{
    public abstract partial class State
    {
        private enum UpdateMode
        {
            Update = 0,
            FixedUpdate = 1
        }

        [Header("Lifecycle")]
        [DisableInPlayMode]
        [SerializeField]
        private bool _useUnityLifecycle;
      
        [DisableInPlayMode]
        [ShowIf(nameof(_useUnityLifecycle))]
        [SerializeField]
        private UpdateMode _updateMode;

        private void Awake()
        {
            if (!_useUnityLifecycle)
                this.enabled = false;
        }

        private void OnEnable() => this.OnEnter();
        
        private void OnDisable() => this.OnExit();

        private void Update()
        {
            if (_updateMode == UpdateMode.Update)
                this.OnUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_updateMode == UpdateMode.FixedUpdate)
                this.OnUpdate(Time.fixedDeltaTime);
        }
    }
}