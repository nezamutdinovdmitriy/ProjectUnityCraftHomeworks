using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext
{
    public static class EntityContextAPI
    {
        // Common
        public static readonly ValueKey<IEntityContext, IVariable<Vector3>> Position = new(nameof(Position));
        
        // Movement
        public static readonly TagKey MovableTag = new(nameof(MovableTag));
        
        public static readonly ValueKey<IEntityContext, IRequest<Vector3>> MovementRequest = new(nameof(MovementRequest));
        public static readonly ValueKey<IEntityContext, ICommand<Vector3, float>> MovementCommand = new(nameof(MovementCommand));
        public static readonly ValueKey<IEntityContext, IValue<float>> MovementSpeed = new(nameof(MovementSpeed));
        
        // Rotation
        public static readonly ValueKey<IEntityContext, IVariable<Quaternion>> Rotation = new(nameof(Rotation));
        
        public static readonly ValueKey<IEntityContext, IRequest<Vector3>> RotationRequest = new(nameof(RotationRequest));
        public static readonly ValueKey<IEntityContext, ICommand<Vector3, float>> RotationCommand = new(nameof(RotationCommand));
        
        public static readonly ValueKey<IEntityContext, IValue<float>> RotationSpeed = new(nameof(RotationSpeed));
        
        // Health
        public static readonly ValueKey<IEntityContext, IValue<float>> MaxHealth = new(nameof(MaxHealth));
        public static readonly ValueKey<IEntityContext, IReactiveVariable<float>> CurrentHealth = new(nameof(CurrentHealth));
        
        // Fire
        public static readonly ValueKey<IEntityContext, IRequest> FireRequest = new(nameof(FireRequest));
        public static readonly ValueKey<IEntityContext, ICommand> FireCommand = new(nameof(FireCommand));
        public static readonly ValueKey<IEntityContext, ICooldown> FireCooldown = new(nameof(FireCooldown));
        
        // Interact
        public static readonly TagKey InteractableTag = new(nameof(InteractableTag));
        public static readonly TagKey InteractorTag = new(nameof(InteractorTag));
        public static readonly ValueKey<IEntityContext, ICommand<IEntityContext>> InteractCommand = new(nameof(InteractCommand));
        
        // Trigger
        public static readonly ValueKey<IEntityContext, TriggerEvents> Trigger = new(nameof(Trigger));
    }
}