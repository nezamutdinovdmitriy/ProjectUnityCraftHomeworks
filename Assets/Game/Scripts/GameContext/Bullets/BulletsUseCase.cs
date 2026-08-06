using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;
using UnityEngine;

namespace Game.Bullets
{
    public static class BulletsUseCase
    {
        public static IGameEntity SpawnBullet(this IGameContext gameContext, Vector3 position, Quaternion rotation, IGameEntity owner)
        {
            GameEntityPool pool = gameContext.GetValue(GameContextAPI.BulletPool);

            IGameEntity bullet = pool.Rent();
            
            bullet.GetValue(GameEntityAPI.Position).Value = position;
            
            bullet.GetValue(GameEntityAPI.Rotation).Value = rotation;
            bullet.GetValue(GameEntityAPI.Owner).Value = owner;
            
            //Debug.Log($"{bullet.GetValue(GameEntityAPI.MovementRequest).Required} {bullet.GetValue(GameEntityAPI.MovementRequest).Arg}");
            //bullet.GetValue(GameEntityAPI.MovementRequest).Consume(out _); // этот момент пришлось закостылить
            //((GameEntity.GameEntity)bullet).gameObject.SetActive(true);
            
            return bullet;
        }

        public static void DestroyBullet(this IGameContext gameContext, GameEntity.GameEntity bullet)
        {
            GameEntityPool pool = gameContext.GetValue(GameContextAPI.BulletPool);
            //bullet.GetValue(GameEntityAPI.MovementRequest).Consume(out _);
            pool.Return(bullet);
        }
    }
}