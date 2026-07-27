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
        
        public override void Install(IGameEntity entity)
        {
            _interactableInstaller.Install(entity);
            
            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(interactor =>
                    interactor.HasTag(GameEntityAPI.InteractorTag)
                    && interactor.TryGetValue(GameEntityAPI.MaxHealth, out IValue<float> maxHealth)
                    && interactor.GetValue(GameEntityAPI.CurrentHealth).Value < maxHealth.Value)
                .AddAction(interactor =>
                {
                    IReactiveVariable<float> currentHealth = interactor.GetValue(GameEntityAPI.CurrentHealth);
                    IValue<float> maxHealth = interactor.GetValue(GameEntityAPI.MaxHealth);

                    currentHealth.Value = Mathf.Min(currentHealth.Value + _healAmount, maxHealth.Value);
                    Destroy(gameObject);
                });
        }
    }
}