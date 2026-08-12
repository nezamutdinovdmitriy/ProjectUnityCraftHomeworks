using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game
{
    public static class GameContextAPI
    {
        // Bullets
        public static ValueKey<IGameContext, BulletEntityPool> BulletPool = new(nameof(BulletPool));
        
        public static ValueKey<IGameContext, IVariable<IGameEntity>> Character = new(nameof(Character));
        
        public static ValueKey<IGameContext, IReactiveVariable<int>> Score = new(nameof(Score));
    }
}