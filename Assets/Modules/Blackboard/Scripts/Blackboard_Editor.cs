#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Modules.AI
{
    public partial class Blackboard
    {
        private void OnValidate()
        {
            if (this.autoCompile && !EditorApplication.isPlaying && !EditorApplication.isCompiling)
                this.Compile();
        }

#if ODIN_INSPECTOR
        [Button(nameof(Compile)), HideInPlayMode]
        [GUIColor(0f, 0.83f, 1f)]
        [PropertySpace(SpaceBefore = 4, SpaceAfter = 8)]
        [PropertyTooltip("Compiles the blackboard's state in the Unity Editor")]
#endif
        [ContextMenu(nameof(Compile))]
        private void Compile()
        {
            try
            {
                this.ConstructValues(this.initialValueCapacity);
                this.Clear();

                _installed = false;
                this.Install();
                this.PrecomputeCapacity();
            }
            catch (Exception e)
            {
                Debug.LogError($"<color=#FF3C3C>{this.name} Compilation failed: {e.Message}</color>\n{e.StackTrace}", this);
            }
        }

        private void PrecomputeCapacity()
        {
            this.initialTagCapacity = _tagCount;
            this.initialValueCapacity = _valueCount;
        }
    }
}

#endif
