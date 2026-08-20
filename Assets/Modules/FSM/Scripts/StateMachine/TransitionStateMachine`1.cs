using System;
using System.Collections.Generic;
using Modules.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.FSM
{
    public class TransitionStateMachine<TKey> : StateMachine<TKey>
    {
        [HideInPlayMode]
        [Space]
        [SerializeField]
        private List<Transition> _transitions;

        public int TransitionCount => _transitions.Count;

        [PropertySpace]
        [HideInEditorMode]
        [ShowInInspector, ReadOnly]
        public IReadOnlyList<Transition> AllTransitions => _transitions;

        private void OnValidate()
        {
            if (_transitions != null)
                _transitions.Sort();
        }

        public event Action<Transition> OnTransitionAdded;
        public event Action<Transition> OnTransitionRemoved;

        public bool ContainsTransition(TKey from, TKey to) =>
            this.FindTransition(from, to, out _);

        public bool ContainsTransition(Transition transition) =>
            _transitions.Contains(transition);

        public bool AddTransition(Transition transition)
        {
            if (_transitions.Contains(transition))
                return false;

            _transitions.Add(transition);
            _transitions.Sort();

            this.OnTransitionAdded?.Invoke(transition);
            return true;
        }

        public bool RemoveTransition(Transition transition)
        {
            if (!_transitions.Remove(transition))
                return false;

            this.OnTransitionRemoved?.Invoke(transition);
            return true;
        }

        public bool RemoveTransition(TKey from, TKey to) =>
            this.FindTransition(from, to, out Transition transition) &&
            this.RemoveTransition(transition);

        private bool FindTransition(TKey from, TKey to, out Transition result)
        {
            for (int i = 0, count = _transitions.Count; i < count; i++)
            {
                var transition = _transitions[i];
                if (transition.Equals(from, to))
                {
                    result = transition;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public override void OnUpdate(float deltaTime)
        {
            this.UpdateTransitions();
            base.OnUpdate(deltaTime);
        }

        private void UpdateTransitions()
        {
            if (_transitions.Count <= 0)
                return;

            for (int i = 0, count = _transitions.Count; i < count; i++)
            {
                Transition transition = _transitions[i];
                if (!transition.From.Equals(this.CurrentState))
                    continue;

                if (!transition.CanPerform())
                    continue;

                this.ChangeState(transition.To, transition.Perform);
                break;
            }
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            if (_transitions != null)
                _transitions.Sort();
        }

        [Serializable]
        public sealed class Transition : IComparable<Transition>
        {
            [LabelText("From")]
            [HorizontalGroup]
            [SerializeField]
            private TKey _from;

            [HorizontalGroup]
            [LabelText("To")]
            [SerializeField]
            private TKey _to;

            [SerializeReference]
            private ICondition _condition;

            [SerializeReference]
            private IAction _action;

            [SerializeField]
            private int _priority;

            public TKey From => this._from;
            public TKey To => this._to;

            public int CompareTo(Transition other) => other._priority.CompareTo(_priority);

            public bool CanPerform() => _condition == null || _condition.Invoke();

            internal void Perform() => _action?.Invoke();

            public bool Equals(TKey from, TKey to) =>
                s_comparer.Equals(this._from, from) && s_comparer.Equals(this._to, to);

            public bool Equals(Transition other) => s_comparer.Equals(_from, other._from) &&
                                                    s_comparer.Equals(_to, other._to);

            public override bool Equals(object obj) => obj is Transition other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    return (s_comparer.GetHashCode(this._from) * 397) ^
                           s_comparer.GetHashCode(this._to);
                }
            }
        }
    }
}