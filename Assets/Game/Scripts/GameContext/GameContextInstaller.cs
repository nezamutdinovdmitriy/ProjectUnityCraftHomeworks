using Atomic.Entities;
using Game.GameEntity;
using UnityEngine;

namespace Game
{
    public class GameContextInstaller : SceneEntityInstaller<IGameContext>
    {
        [SerializeField]
        private GameEntityPool _bulletPool;
        
        public override void Install(IGameContext context)
        {
            context.AddValue(GameContextAPI.BulletPool, _bulletPool);
        }
    }
}