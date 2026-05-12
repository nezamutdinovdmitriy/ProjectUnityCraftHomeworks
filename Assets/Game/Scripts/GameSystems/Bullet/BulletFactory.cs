using UnityEngine;

namespace Game
{
    public class BulletFactory : MonoBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        [SerializeField]
        private BulletConfig _config;

        public Bullet Create(Transform parent)
        {
            Bullet instance = Instantiate(_bulletPrefab, parent);

            instance.Construct(_config);
            
            return instance;
        }
    }
}