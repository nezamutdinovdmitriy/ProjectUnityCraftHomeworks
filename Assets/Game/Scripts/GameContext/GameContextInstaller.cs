using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;
using UnityEngine;

namespace Game
{
    public class GameContextInstaller : SceneEntityInstaller<IGameContext>
    {
        [SerializeField]
        private GameEntityPool _bulletPool;

        [SerializeField]
        private GameEntity.GameEntity _character;
        
        public override void Install(IGameContext context)
        {
            context.AddValue(GameContextAPI.BulletPool, _bulletPool);
            context.AddValue(GameContextAPI.Character, new Variable<IGameEntity>(_character));
        }
    }
}