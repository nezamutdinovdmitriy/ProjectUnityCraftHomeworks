using UnityEngine;

namespace Game
{
    public class BulletExplosionFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionPrefab;

        public GameObject Create(Vector3 position)
        {
            return Instantiate(_explosionPrefab, position, Quaternion.identity);
        }
    }
}