using System;
using Atomic.Elements;
using Game.GameEntity;
using Game.Weapon;
using UnityEngine;

namespace Atomic.Entities
{
    [Serializable]
    public class ConsumablePickupInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private InteractableInstaller _interactableInstaller;
        
        [SerializeField]
        private DestroyAfterUseInstaller _destroyAfterUseInstaller;
        
        public void Install(IGameEntity entity)
        {
            _interactableInstaller.Install(entity);
            _destroyAfterUseInstaller.Install(entity);
            
            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor =>
                    interactor.HasTag(GameEntityAPI.InteractorTag)
                    && entity.GetValue(GameEntityAPI.WasUsed).Value == false)
                .AddAction(interactor =>
                {
                    entity.GetValue(GameEntityAPI.WasUsed).Value = true;
                });
        }
    }
}