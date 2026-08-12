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
        
        public void Install(IGameEntity item)
        {
            _interactableInstaller.Install(item);
            _destroyAfterUseInstaller.Install(item);
            
            item.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor =>
                    interactor.HasTag(GameEntityAPI.InteractorTag)
                    && item.GetValue(GameEntityAPI.WasUsed).Value == false)
                .AddAction(interactor =>
                {
                    item.GetValue(GameEntityAPI.WasUsed).Value = true;
                });
        }
    }
}