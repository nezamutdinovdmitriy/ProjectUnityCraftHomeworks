using System;
using System.Collections.Generic;

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Modules.FSM
{
    public class StateMachine<TKey> : State, ISerializationCallbackReceiver
    {
        protected static readonly EqualityComparer<TKey> s_comparer = EqualityComparer<TKey>.Default;

        [Serializable]
        protected struct StateInfo
        {
            public TKey key;
            public State state;
        }
        
        public int A { get; private set; }
        
        [Space]
#if ODIN_INSPECTOR
        [HideInPlayMode]
#endif
        [SerializeField]
        private TKey _initialState;

        [LabelText("States")]
        [HideInPlayMode]
        private StateInfo[] _initialStates;
        
        public event Action<TKey> OnStateChanged;
        public event Action<TKey> OnStateAdded;
        public event Action<TKey> OnStateRemoved;

        public bool HasState => _current.Value != null;

#if ODIN_INSPECTOR
        [HideInEditorMode]
        [ShowInInspector, ReadOnly]
#endif
        public TKey CurrentState => _current.Key;

        public int StateCount => _states.Count;

#if ODIN_INSPECTOR
        [HideInEditorMode]
        [ShowInInspector, ReadOnly]
#endif
        public IReadOnlyDictionary<TKey, State> States => _states;

        private readonly Dictionary<TKey, State> _states = new();
        private KeyValuePair<TKey, State> _current;

        [Button]
        public void ChangeState(TKey key, Action transition = null)
        {
            if (!_states.TryGetValue(key, out var next))
                throw new Exception($"State [{key}] not found");

            _current.Value?.OnExit();
            
            transition?.Invoke();

            Debug.Log("AAAA");
            _current = new KeyValuePair<TKey, State>(key, next);
            _current.Value?.OnEnter();
            OnStateChanged?.Invoke(key);
        }

        [Button]
        public bool TryChangeState(TKey key, Action transition = null)
        {
            if (s_comparer.Equals(_current.Key, key))
                return false;

            if (!_states.ContainsKey(key))
                return false;

            this.ChangeState(key, transition);
            return true;
        }

        public void AddState(TKey key, State state)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (state == null)
                throw new ArgumentNullException(nameof(state));
            
            _states.Add(key, state);
            OnStateAdded?.Invoke(key);
        }

        public bool RemoveState(TKey key)
        {
            if (!_states.Remove(key))
                return false;

            if (s_comparer.Equals(_current.Key, key))
                _current = default;

            OnStateRemoved?.Invoke(key);
            return true;
        }

        public bool ContainsState(TKey key) => _states.ContainsKey(key);

        public override void OnEnter()
        {
            State value = _current.Value;
            if (value)
                value.OnEnter();
        }

        public override void OnUpdate(float deltaTime)
        {
            State current = _current.Value;
            if (current)
                current.OnUpdate(deltaTime);
        }

        public override void OnExit()
        {
            
            State current = _current.Value;
            if (current) 
                current.OnExit();
        }
        
        public virtual void OnAfterDeserialize()
        {
            _states.Clear();

            if (_initialStates != null)
                foreach (StateInfo pair in _initialStates)
                    _states.Add(pair.key, pair.state);

            if (!_states.TryGetValue(_initialState, out var initialState))
                throw new Exception($"Initial state [{_initialState}] not found");

            _current = new KeyValuePair<TKey, State>(_initialState, initialState);
        }

        public virtual void OnBeforeSerialize()
        {
        }
    }
}