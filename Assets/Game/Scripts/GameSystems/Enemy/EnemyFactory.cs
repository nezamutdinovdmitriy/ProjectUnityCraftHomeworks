using Game.Spawn;
using UnityEngine;

namespace Game
{
    public class EnemyFactory : Factory<Enemy>
    {
        [Header("Target")] [SerializeField]
        private Ship _player;

        [SerializeField]
        private BulletManager _bulletManager;

        public override Enemy Create()
        {
            Enemy instance = base.Create();

            instance.Construct(_player, _bulletManager);

            return instance;
        }
    }
}