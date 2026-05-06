using UnityEngine;

namespace Game
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;

        [SerializeField] private GameObject _blueVfx;
        [SerializeField] private GameObject _redVfx;
        [SerializeField] private BulletExplosionFactory _explosionFactory;

        private void OnEnable()
        {
            _bullet.Initialized += SetBulletView;
            _bullet.Hit += Explosion;
        }

        private void Explosion(Bullet arg1, Collider2D arg2)
        {
            _explosionFactory.Create(_bullet.transform.position);
        }

        private void OnDisable()
        {
            _bullet.Initialized -= SetBulletView;
            _bullet.Hit -= Explosion;
        }
        
        private void SetBulletView(TeamType team)
        {
            _blueVfx.SetActive(team == TeamType.Player);
            _redVfx.SetActive(team == TeamType.Enemy);
        }
    }
}