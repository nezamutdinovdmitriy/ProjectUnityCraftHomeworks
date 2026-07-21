using Atomic.Elements;
using Atomic.Entities;
using Game.EntityContext.Core.Fire;
using Game.EntityContext.Core.Health;
using Game.EntityContext.Core.Interact;
using Game.EntityContext.Core.Rotation;
using Game.UI;
using UnityEngine;

namespace Game.EntityContext
{
    
    public class CharacterInstaller : SceneEntityInstaller<IEntityContext>
    {
        [SerializeField]
        private float _movementSpeed;

        [SerializeField]
        private float _rotationSpeed;
        
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private Cooldown _fireCooldown;
        
        [SerializeField]
        private Cooldown _aimCooldown;

        [SerializeField]
        private float _maxHealth;

        [SerializeField]
        private TriggerEvents _trigger;
        
        public override void Install(IEntityContext entity)
        {
            entity.AddValue(EntityContextAPI.Position, new TransformPositionVariable(_transform));
            
            HealthInstall(entity);
            MovementInstall(entity);
            RotationInstall(entity);
            FireInstall(entity);
            InteractInstall(entity);
            
            entity.AddBehaviour(new CharacterInputController());
        }

        private void InteractInstall(IEntityContext entity)
        {
            entity.AddTag(EntityContextAPI.InteractorTag);
            entity.AddValue(EntityContextAPI.Trigger, _trigger);
            entity.AddBehaviour(new InteractBehaviour());
        }

        private void HealthInstall(IEntityContext entity)
        {
            entity.AddValue(EntityContextAPI.MaxHealth, new Const<float>(_maxHealth)); 
            entity.AddValue(EntityContextAPI.CurrentHealth, new ReactiveVariable<float>(_maxHealth));
        }

        private void FireInstall(IEntityContext entity)
        {
            bool wasAimingLastFrame = false;
            
            entity.AddValue(EntityContextAPI.FireCooldown, _fireCooldown);
            entity.WhenFixedTick(_fireCooldown.Tick);

            entity.WhenFixedTick(_aimCooldown.Tick);
            
            entity.AddValue(EntityContextAPI.FireRequest, new Request());
           
            entity.AddValue(EntityContextAPI.FireCommand, new Command()
                .AddCondition(() =>
                {
                    Joystick joystick = UIContext.Instance.GetValue(UIContextAPI.FireJoystick);
                    bool isAiming = joystick.Direction != Vector2.zero;
                    
                    if(isAiming && wasAimingLastFrame == false)
                        _aimCooldown.ResetTime();

                    wasAimingLastFrame = isAiming;

                    return isAiming 
                           && _fireCooldown.IsCompleted() 
                           && _aimCooldown.IsCompleted() 
                           && entity.IsDead() == false;
                })
                .AddAction(_fireCooldown.ResetTime)
                .AddAction(entity.Fire));
            
            entity.AddBehaviour(new FireBehaviour());
        }

        private void RotationInstall(IEntityContext entity)
        {
            entity.AddValue(EntityContextAPI.Rotation, new TransformRotationVariable(_transform));
            entity.AddValue(EntityContextAPI.RotationSpeed, new Const<float>(_rotationSpeed));
            entity.AddValue(EntityContextAPI.RotationRequest, new Request<Vector3>());
            entity.AddValue(EntityContextAPI.RotationCommand, new Command<Vector3, float>()
                .AddCondition((_,_) => entity.IsDead() == false)
                .AddAction(entity.RotationStep));
            
            entity.AddBehaviour(new RotationBehaviour());
        }

        private void MovementInstall(IEntityContext entity)
        {
            entity.AddTag(EntityContextAPI.MovableTag);
            entity.AddValue(EntityContextAPI.MovementRequest, new Request<Vector3>());
            entity.AddValue(EntityContextAPI.MovementCommand, new Command<Vector3, float>()
                .AddCondition((_,_) => entity.IsDead() == false)
                .AddAction(entity.MoveStep));
            
            entity.AddValue(EntityContextAPI.MovementSpeed, new Const<float>(_movementSpeed));
            
            entity.AddBehaviour(new MovementBehaviour());
        }
    }
}