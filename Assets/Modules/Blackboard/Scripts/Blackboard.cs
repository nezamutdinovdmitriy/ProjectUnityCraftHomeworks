using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [DefaultExecutionOrder(-1000)]
    [AddComponentMenu("Modules/AI/Blackboard")]
    public sealed partial class Blackboard : MonoBehaviour, ISerializationCallbackReceiver
    {
        private const int UNDEFINED_INDEX = -1;

        [SerializeField]
        private bool installOnAwake = true;

        [SerializeReference, HideInPlayMode]
        [PropertySpace(SpaceAfter = 12)]
        private IBlackboardInstaller[] installers = Array.Empty<IBlackboardInstaller>();

        [Header("Editor")]
        [SerializeField]
        private bool autoCompile;

        /// <summary>
        /// Initial tag capacity used to optimize tag allocation.
        /// </summary>
#if ODIN_INSPECTOR
        [FoldoutGroup("Optimization", 1)]
#else
        [Header("Optimization")]
#endif
        [Min(1)]
        [SerializeField]
        private int initialTagCapacity = 1;
        /// <summary>
        /// Initial value capacity used to optimize value allocation.
        /// </summary>
#if ODIN_INSPECTOR
        [FoldoutGroup("Optimization", 2)]
#endif
        [Min(1)]
        [SerializeField]
        private int initialValueCapacity = 1;

        public event Action OnStateChanged;

        public bool Installed => _installed;

        private bool _installed;

        private void Awake()
        {
            if (this.installOnAwake) 
                this.Install();
        }

        public void Install()
        {
            if (_installed)
                return;

            _installed = true;

            if (this.installers != null)
                for (int i = 0, length = this.installers.Length; i < length; i++) 
                    this.installers[i]?.Install(this);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.ConstructTags(this.initialTagCapacity);
            this.ConstructValues(this.initialValueCapacity);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        public void Clear()
        {
            this.ClearTags();
            this.ClearValues();
        }
    }
}