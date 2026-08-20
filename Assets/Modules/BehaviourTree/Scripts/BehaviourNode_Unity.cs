using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    public partial class BehaviourNode
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

        private protected virtual void Awake()
        {
            if (!_useUnityLifecycle)
                this.enabled = false;
        }

        private protected void Update()
        {
            if (_updateMode == UpdateMode.Update)
                this.Run(Time.deltaTime);
        }

        private protected void FixedUpdate()
        {
            if (_updateMode == UpdateMode.FixedUpdate)
                this.Run(Time.fixedDeltaTime);
        }

        private protected void OnDisable()
        {
            if (_useUnityLifecycle)
                this.Abort();
        }
    }
}