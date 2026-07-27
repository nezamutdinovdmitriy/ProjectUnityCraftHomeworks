using Atomic.Entities;
using Game.GameEntity;
using UnityEngine;

namespace Game.Bullets
{
    public static class BulletsUseCase
    {
        public static IGameEntity SpawnBullet(this IGameContext gameContext, Vector3 position, Quaternion rotation)
        {
            GameEntityPool pool = gameContext.GetValue(GameContextAPI.BulletPool);

            IGameEntity bullet = pool.Rent();
            bullet.GetValue(GameEntityAPI.Position).Value = position;
            bullet.GetValue(GameEntityAPI.Rotation).Value = rotation;
            
            return bullet;
        }
    }
}