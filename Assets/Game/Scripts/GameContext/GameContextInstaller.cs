using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;
using UnityEngine;

namespace Game
{
    public class GameContextInstaller : SceneEntityInstaller<IGameContext>
    {
        [SerializeField]
        private BulletEntityPool _bulletPool;

        [SerializeField]
        private GameEntity _character;
        
        public override void Install(IGameContext context)
        {
            context.AddValue(GameContextAPI.BulletPool, _bulletPool);
            context.AddValue(GameContextAPI.Character, new Variable<IGameEntity>(_character));
            context.AddValue(GameContextAPI.Score, new ReactiveVariable<int>());
        }
    }
}