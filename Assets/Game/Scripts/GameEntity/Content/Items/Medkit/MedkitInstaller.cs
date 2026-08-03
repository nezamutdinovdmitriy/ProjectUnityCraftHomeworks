using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public class MedkitInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private InteractableInstaller _interactableInstaller;

        [SerializeField]
        private float _healAmount = 3;
        
        [SerializeField]
        private Cooldown _destroyTimer;

        private bool _wasUsed;
        
        public override void Install(IGameEntity entity)
        {
            _interactableInstaller.Install(entity);
            
            entity.WhenFixedTick(deltaTime =>
            {
                if(_wasUsed)
                    _destroyTimer.Tick(deltaTime);
                
                if(_destroyTimer.IsCompleted())
                    Destroy(gameObject);
            });
            
            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor =>
                    interactor.HasTag(GameEntityAPI.InteractorTag)
                    && _wasUsed == false
                    && interactor.TryGetValue(GameEntityAPI.MaxHealth, out IValue<float> maxHealth)
                    && interactor.GetValue(GameEntityAPI.CurrentHealth).Value < maxHealth.Value)
                .AddAction(interactor =>
                {
                    IReactiveVariable<float> currentHealth = interactor.GetValue(GameEntityAPI.CurrentHealth);
                    IValue<float> maxHealth = interactor.GetValue(GameEntityAPI.MaxHealth);

                    currentHealth.Value = Mathf.Min(currentHealth.Value + _healAmount, maxHealth.Value);
                    _wasUsed = true;
                });
        }
    }
}