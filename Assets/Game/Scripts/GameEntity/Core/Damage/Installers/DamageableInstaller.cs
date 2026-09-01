using System;
using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
{
    [Serializable]
    public class DamageableInstaller : IEntityInstaller<IGameEntity>
    {
        public void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.DamageableTag);
            entity.AddValue(GameEntityAPI.TakeDamageCommand, new Command<float>());
        }
    }
}