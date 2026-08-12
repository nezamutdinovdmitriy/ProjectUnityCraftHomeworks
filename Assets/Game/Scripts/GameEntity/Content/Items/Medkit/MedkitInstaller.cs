using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public class MedkitInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private ConsumablePickupInstaller _consumablePickupInstaller;

        [SerializeField]
        private float _healAmount = 3;
        
        public override void Install(IGameEntity entity)
        {
            _consumablePickupInstaller.Install(entity);
            
            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor => interactor.IsHealthNotFull())
                .AddAction(interactor => interactor.HealthRestore(_healAmount));
        }
    }
}