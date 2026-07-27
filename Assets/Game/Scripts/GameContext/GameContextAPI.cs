using Atomic.Entities;
using Game.GameEntity;

namespace Game
{
    public static class GameContextAPI
    {
        // Bullets
        public static ValueKey<IGameContext, GameEntityPool> BulletPool = new(nameof(BulletPool));
    }
}