using System;
using Atomic.Elements;
using Atomic.Entities;
using Game.EntityContext.Core.Health;
using UnityEngine;

namespace Game.EntityContext.Items.Medkit
{
    public class MedkitInstaller : SceneEntityInstaller<IEntityContext>
    {
        [SerializeField]
        private float _amount;
        
        public override void Install(IEntityContext entity)
        {
            entity.AddTag(EntityContextAPI.InteractableTag);
            
            entity.AddValue(EntityContextAPI.InteractCommand, new Command<IEntityContext>()
                .AddCondition(interactor => 
                    interactor.HasTag(EntityContextAPI.InteractorTag)
                    && interactor.TryGetValue(EntityContextAPI.MaxHealth, out IValue<float> maxHealth)
                    && interactor.GetValue(EntityContextAPI.CurrentHealth).Value < maxHealth.Value)
                .AddAction(interactor =>
                {
                    interactor.CollectMedkit(_amount);
                    interactor.ReduceHealth(101);
                    Destroy(gameObject);
                }));
        }
    }
}