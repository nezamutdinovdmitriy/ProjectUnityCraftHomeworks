using System;
using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntity
{
    [Serializable]
    public class FireInstaller : IEntityInstaller<IGameEntity>
    {
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.FireRequest, new Request());
            entity.AddValue(GameEntityAPI.FireCommand, new Command());
            
            entity.AddBehaviour(new FireBehaviour());
        }
    }
}