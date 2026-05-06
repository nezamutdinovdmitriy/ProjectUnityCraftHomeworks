using UnityEngine;

namespace Game
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField]
        private Enemy _prefab;
        [SerializeField]
        private Transform _container;

        public Enemy Create() => Instantiate(_prefab, _container);
    }
}