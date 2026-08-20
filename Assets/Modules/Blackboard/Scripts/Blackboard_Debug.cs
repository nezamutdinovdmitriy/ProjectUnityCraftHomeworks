#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Modules.AI
{
    public partial class Blackboard
    {
        #region Tags

        private static readonly List<DebugTag> _debugTagsCache = new();

        [InlineProperty]
        private struct DebugTag : IComparable<DebugTag>
        {
            [ShowInInspector, ReadOnly]
            public string name;

            internal readonly int id;

            public DebugTag(string name, int id)
            {
                this.name = name;
                this.id = id;
            }

            public int CompareTo(DebugTag other) =>
                string.Compare(this.name, other.name, StringComparison.Ordinal);
        }

        [Searchable]
        [FoldoutGroup("Debug", order: 2)]
        [LabelText("Tags")]
        [ShowInInspector]
        [PropertyOrder(100)]
        [ListDrawerSettings(
            CustomRemoveElementFunction = nameof(DebugDelTag),
            CustomRemoveIndexFunction = nameof(DebugDelTagAt),
            HideAddButton = true
        )]
        private List<DebugTag> DebugTags
        {
            get
            {
                _debugTagsCache.Clear();

                TagEnumerator enumerator = this.GetTagEnumerator();
                while (enumerator.MoveNext())
                {
                    int id = enumerator.Current;
                    string name = BlackboardKeys.IdToName(id);
                    _debugTagsCache.Add(new DebugTag(name, id));
                }

                _debugTagsCache.Sort();
                return _debugTagsCache;
            }
            set { }
        }

        private void DebugDelTag(DebugTag tag) => this.DelTag(tag.id);

        private void DebugDelTagAt(int index) =>
            this.DelTag(this.DebugTags[index].id);

        #endregion

        #region Values

        private static readonly List<DebugValue> _debugValuesCache = new();

        [InlineProperty]
        private struct DebugValue : IComparable<DebugValue>
        {
            [HorizontalGroup(200), ShowInInspector, ReadOnly, HideLabel]
            public string name;

            [HorizontalGroup, ShowInInspector, HideLabel]
            public object value
            {
                get
                {
                    if (_blackboard == null)
                        return null;

                    return _blackboard.GetValue(id);
                }
                set
                {
                    if (_blackboard == null || value == null)
                        return;

                    var type = value.GetType();

                    if (type.IsValueType)
                        SetPrimitive(value, type);
                    else
                        _blackboard.SetReferenceValue(id, value);
                }
            }

            internal readonly int id;
            private readonly Blackboard _blackboard;

            public DebugValue(string name, int id, Blackboard blackboard)
            {
                this.name = name;
                this.id = id;
                this._blackboard = blackboard;
            }

            public int CompareTo(DebugValue other) =>
                string.Compare(this.name, other.name, StringComparison.Ordinal);

            private void SetPrimitive(object value, Type type)
            {
                var method = typeof(Blackboard)
                    .GetMethod(nameof(SetPrimitiveValue))
                    ?.MakeGenericMethod(type);

                method?.Invoke(_blackboard, new[] {id, value});
            }
        }

        [Searchable]
        [FoldoutGroup("Debug", order: 3)]
        [LabelText("Values")]
        [ShowInInspector]
        [PropertyOrder(100)]
        [PropertySpace(SpaceAfter = 8)]
        [ListDrawerSettings(
            CustomRemoveElementFunction = nameof(DebugDelValue),
            CustomRemoveIndexFunction = nameof(DebugDelValueAt),
            HideAddButton = true
        )]
        private List<DebugValue> DebugValues
        {
            get
            {
                _debugValuesCache.Clear();

                ValueEnumerator enumerator = this.GetValueEnumerator();
                while (enumerator.MoveNext())
                {
                    var (id, _) = enumerator.Current;
                    string name = BlackboardKeys.IdToName(id);

                    _debugValuesCache.Add(new DebugValue(name, id, this));
                }

                _debugValuesCache.Sort();
                return _debugValuesCache;
            }
            set { }
        }

        private void DebugDelValue(DebugValue value) =>
            this.DelValue(value.id);

        private void DebugDelValueAt(int index) =>
            this.DelValue(this.DebugValues[index].id);

        #endregion

        [GUIColor(0f, 0.83f, 1f)]
        [FoldoutGroup("Debug", order: 10)]
        [PropertyOrder(1000)]
        [HorizontalGroup("Debug/AddInstaller", Width = 120)]
        [HideInEditorMode]
        [Button("+ Add")]
        private void DebugAdd()
        {
            if (_debugInstaller == null)
                return;

            _debugInstaller.Install(this);
            _debugInstaller = null;
        }

        [FoldoutGroup("Debug", order: 10)]
        [PropertyOrder(1001)]
        [HorizontalGroup("Debug/AddInstaller")]
        [HideInEditorMode]
        [ShowInInspector]
        [HideLabel]
        private IBlackboardInstaller _debugInstaller;
    }
}
#endif