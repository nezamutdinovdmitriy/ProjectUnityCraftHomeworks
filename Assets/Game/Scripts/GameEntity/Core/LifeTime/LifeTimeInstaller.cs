using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity.Core.LifeTime
{
    [Serializable]
    public class LifeTimeInstaller : IEntityInstaller<IGameEntity>
    {
        private DisposableComposite _disposables = new();
        
        [SerializeField]
        private Cooldown _lifetime;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Lifetime, _lifetime);
            entity.AddValue(GameEntityAPI.LifetimeEndCommand, new Command());

            entity.WhenFixedTick(_lifetime.Tick).AddTo(_disposables);
            entity.WhenFixedTick(_ =>
            {
                if (_lifetime.IsCompleted())
                {
                    entity.GetValue(GameEntityAPI.Lifetime).ResetTime();
                    entity.GetValue(GameEntityAPI.LifetimeEndCommand).Invoke();
                }
            }).AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}