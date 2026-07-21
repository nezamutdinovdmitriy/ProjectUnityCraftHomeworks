using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext.Content.Items.Ammo
{
    public class AmmoInstaller : SceneEntityInstaller<IEntityContext>
    {
        [SerializeField]
        private int _amount;
        
        public override void Install(IEntityContext entity)
        {
            entity.AddTag(EntityContextAPI.InteractableTag);
            
            entity.AddValue(EntityContextAPI.InteractCommand, new Command<IEntityContext>());
        }
    }
}