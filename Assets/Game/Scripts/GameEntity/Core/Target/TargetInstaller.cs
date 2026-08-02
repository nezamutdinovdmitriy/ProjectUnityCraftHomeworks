using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity.Core.Target
{
    [Serializable]
    public class TargetInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private TriggerEvents _triggerEvents;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Target, new Variable<IGameEntity>());
            entity.AddValue(GameEntityAPI.Trigger, _triggerEvents);
            
            
            entity.AddBehaviour(new DetectTargetBehaviour());
        }
    }
}