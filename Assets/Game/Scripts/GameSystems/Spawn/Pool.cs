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
                instance.gameObject.SetActive(false);
                _stack.Push(instance);
            }
        }

        public T Rent()
        {
            T instance = _stack.TryPop(out T obj) ? obj : _factory.Create();
            
            instance.gameObject.SetActive(true);

            return instance;
        }

        public void Push(T instance)
        {
            instance.gameObject.SetActive(false);
            _stack.Push(instance);
        }
    }
}