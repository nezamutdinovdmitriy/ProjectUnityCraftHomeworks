using System.Collections.Generic;
using UnityEngine;

namespace Game.Spawn
{
    public abstract class Pool<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private int _prewarmCount = 10;

        private readonly Stack<T> _stack = new();

        [SerializeField]
        private Factory<T> _factory;

        private void Awake()
        {
            for (int i = 0; i < _prewarmCount; i++)
            {
                T instance = _factory.Create();
                
                OnCreate(instance);
                
                instance.gameObject.SetActive(false);
                
                _stack.Push(instance);
            }
        }

        public T Rent()
        {
            //T instance = _stack.TryPop(out T obj) ? obj : _factory.Create();

            T instance;
            
            if (_stack.TryPop(out T enemy))
            {
                instance = enemy;
            }
            else
            {
                instance = _factory.Create();
                OnCreate(instance);
            }
            
            instance.gameObject.SetActive(true);

            OnRent(instance);
            
            return instance;
        }

        public void Push(T instance)
        {
            OnPush(instance);
            
            instance.gameObject.SetActive(false);
            
            _stack.Push(instance);
        }

        protected virtual void OnCreate(T instance) {}
        protected virtual void OnRent(T instance) {}
        protected virtual void OnPush(T instance) {}
        
    }
}