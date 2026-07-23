using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext.Content.Bullet
{
    public class BulletInstaller : SceneEntityInstaller<IEntityContext>
    {
        [SerializeField]
        private float _speed;
        
        public override void Install(IEntityContext entity)
        {
            entity.AddValue(EntityContextAPI.MovementSpeed, new Const<float>(_speed));
            
            entity.AddBehaviour(new MovementBehaviour());
        }
    }
}