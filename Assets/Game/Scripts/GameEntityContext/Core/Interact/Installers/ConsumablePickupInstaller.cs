using System;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
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