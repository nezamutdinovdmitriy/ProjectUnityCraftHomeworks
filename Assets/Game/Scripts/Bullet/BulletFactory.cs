using UnityEngine;

namespace Game
{
    public class BulletFactory : MonoBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        public Bullet Create(Transform parent)
        {
            Bullet instance = Instantiate(_bulletPrefab, parent);

            return instance;
        }
    }
}