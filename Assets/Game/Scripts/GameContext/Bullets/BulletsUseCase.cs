using Atomic.Entities;
using Game.GameEntity;
using UnityEngine;

namespace Game.Bullets
{
    public static class BulletsUseCase
    {
        public static IGameEntity SpawnBullet(this IGameContext gameContext, Vector3 position, Quaternion rotation, IGameEntity owner, float spreadAngle = 0.25f)
        {
            GameEntity.GameEntity bullet = gameContext.GetValue(GameContextAPI.BulletPool).Rent();
            
            bullet.GetValue(GameEntityAPI.Owner).Value = owner;
            bullet.GetValue(GameEntityAPI.Position).Value = position;
            bullet.GetValue(GameEntityAPI.Rotation).Value = rotation.WithSpread(spreadAngle);
            
            bullet.gameObject.SetActive(true);
            
            return bullet;
        }

        public static void DestroyBullet(this IGameContext gameContext, GameEntity.GameEntity bullet)
        {
            BulletEntityPool pool = gameContext.GetValue(GameContextAPI.BulletPool);

            bullet.gameObject.SetActive(false);
            pool.Return(bullet);
        }

        public static Quaternion WithSpread(this Quaternion rotation, float maxAngle)
        {
            float spread = Random.Range(-maxAngle, maxAngle);
            return rotation * Quaternion.Euler(0f, spread, 0f);
        }
    }
}