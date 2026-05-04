using UnityEngine;

namespace Game
{
    public class BulletFactory : MonoBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        public Bullet Create()
        {
            Bullet instance = Instantiate(_bulletPrefab);

            return instance;
        }
    }
}