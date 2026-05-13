using UnityEngine;

namespace Game.Spawn
{
    public abstract class Factory<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private T _prefab;

        [SerializeField]
        private Transform _container; 

        public virtual T Create() => Instantiate(_prefab, _container);
    }
}