using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;
using Event = Atomic.Elements.Event;

namespace Game.GameEntities
{
    [Serializable]
    public class FireInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private Cooldown _fireDelay = 1.033f;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.FireRequest, new Request());
            entity.AddValue(GameEntityAPI.FireCommand, new Command());
            entity.AddValue(GameEntityAPI.FireStartEvent, new Event());
            
            entity.AddBehaviour(new FireBehaviour(_fireDelay));
        }
    }
}