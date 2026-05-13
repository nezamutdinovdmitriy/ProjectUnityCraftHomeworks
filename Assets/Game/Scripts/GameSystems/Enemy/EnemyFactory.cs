using UnityEngine;

namespace Game
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField]
        private Enemy _prefab;
        [SerializeField]
        private Transform _container;

        [Header("Target")] [SerializeField]
        private Ship _player;
        
        [Header("Bullets")] [SerializeField]
        private BulletManager _bulletManager;

        public Enemy Create()
        {
            Enemy instance = Instantiate(_prefab, _container);
            instance.SetTarget(_player);

            return instance;
        }
    }
}