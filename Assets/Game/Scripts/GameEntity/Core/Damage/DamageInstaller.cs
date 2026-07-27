using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    [Serializable]
    public class DamageInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private float _damage;

        [SerializeField]
        private TriggerEvents _triggerEvents;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Damage, new Const<float>(_damage));
            entity.AddValue(GameEntityAPI.Trigger, _triggerEvents);
        }
    }
}