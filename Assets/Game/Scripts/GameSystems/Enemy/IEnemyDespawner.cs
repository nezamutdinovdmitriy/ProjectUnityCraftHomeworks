using System;

namespace Game
{
    // +
    public interface IEnemyDespawner
    {
        public event Action EnemyDespawned;
        void Despawn(Enemy enemy);
    }
}