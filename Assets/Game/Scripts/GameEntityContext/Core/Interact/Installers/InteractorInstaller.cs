using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    [Serializable]
    public class InteractorInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private TriggerEvents _triggerEvents;
        
        public void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.InteractorTag);
            entity.AddValue(GameEntityAPI.Trigger, _triggerEvents);
            
            entity.AddBehaviour(new InteractBehaviour());
        }
    }
}