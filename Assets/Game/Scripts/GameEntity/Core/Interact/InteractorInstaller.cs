using System;
using Atomic.Elements;
using Game.GameEntity;
using UnityEngine;

namespace Atomic.Entities
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