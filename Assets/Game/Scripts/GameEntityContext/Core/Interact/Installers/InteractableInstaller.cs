using System;
using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
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