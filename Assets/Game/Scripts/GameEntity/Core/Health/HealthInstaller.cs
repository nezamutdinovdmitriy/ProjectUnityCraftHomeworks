using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    [Serializable]
    public class HealthInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private float _maxHealth;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.MaxHealth, new Const<float>(_maxHealth));
            entity.AddValue(GameEntityAPI.CurrentHealth, new ReactiveVariable<float>(_maxHealth));
        }
    }
}