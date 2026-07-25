using System;
using Atomic.Elements;
using Game.GameEntity;

namespace Atomic.Entities
{
    [Serializable]
    public class InteractableInstaller : IEntityInstaller<IGameEntity>
    {
        public void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.InteractableTag);
            entity.AddValue(GameEntityAPI.InteractCommand, new Command<IGameEntity>());
        }
    }
}