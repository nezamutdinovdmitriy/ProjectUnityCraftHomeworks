using System;
using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntity.Core.Target
{
    [Serializable]
    public class TargetInstaller : IEntityInstaller<IGameEntity>
    {
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Target, new Variable<IGameEntity>());
            entity.AddValue(GameEntityAPI.TargetIsReached, new Variable<bool>());
        }
    }
}