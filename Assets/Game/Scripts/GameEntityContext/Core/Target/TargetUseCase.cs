using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
{
    public static class TargetUseCase
    {
        public static void SetTarget(GameEntity[] entities, IGameEntity target)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.TryGetValue(
                        GameEntityAPI.Target,
                        out IVariable<IGameEntity> currentTarget))
                {
                    currentTarget.Value = target;
                }
            }
        }
    }
}