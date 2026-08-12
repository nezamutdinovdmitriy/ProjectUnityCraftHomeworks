using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities.Weapon;
using UnityEngine;

namespace Game.GameEntities
{
    public static class GameEntityAPI
    {
        // Common
        public static TagKey CharacterTag = new(nameof(CharacterTag));
        public static ValueKey<IGameEntity, IVariable<IGameEntity>> Owner = new(nameof(Owner));
        
        public static ValueKey<IGameEntity, TransformPositionVariable> Position = new(nameof(Position));
        public static ValueKey<IGameEntity, TransformRotationVariable> Rotation = new(nameof(Rotation));
        public static ValueKey<IGameEntity, TriggerEvents> Trigger = new(nameof(Trigger));
        public static ValueKey<IGameEntity, IVariable<IGameEntity>> Target = new(nameof(Target));
        
        // Movement
        public static ValueKey<IGameEntity, IReactiveVariable<bool>> IsMoving = new(nameof(IsMoving));
        public static ValueKey<IGameEntity, IRequest<Vector3>> MovementRequest = new(nameof(MovementRequest));
        public static ValueKey<IGameEntity, ICommand<MovementArgs>> MovementCommand = new(nameof(MovementCommand));
        public static ValueKey<IGameEntity, IValue<float>> MovementSpeed = new(nameof(MovementSpeed));
        public static ValueKey<IGameEntity, IVariable<Vector3>> MovementDirection = new(nameof(MovementDirection));
        
        // Rotation
        public static ValueKey<IGameEntity, IValue<float>> RotateSpeed = new(nameof(RotateSpeed));
        public static ValueKey<IGameEntity, IRequest<Vector3>> RotateRequest = new(nameof(RotateRequest));
        public static ValueKey<IGameEntity, ICommand<RotateArgs>> RotateCommand = new(nameof(RotateCommand));
        
        // Health
        public static ValueKey<IGameEntity, IValue<float>> MaxHealth = new(nameof(MaxHealth));
        public static ValueKey<IGameEntity, IReactiveVariable<float>> CurrentHealth = new(nameof(CurrentHealth));
        
        // Death
        public static ValueKey<IGameEntity, ICooldown> DeathDelay = new(nameof(DeathDelay));
        public static ValueKey<IGameEntity, ICompositeAction> DeathAction = new(nameof(DeathAction));
        
        // Aim
        public static ValueKey<IGameEntity, IVariable<bool>> HasAimingLastFrame = new(nameof(HasAimingLastFrame));
        public static ValueKey<IGameEntity, ICooldown> AimCooldown = new(nameof(AimCooldown));
        public static ValueKey<IGameEntity, IVariable<Vector3>> AimDirection = new(nameof(AimDirection));
        
        // Attack
        public static ValueKey<IGameEntity, IReactiveVariable<IWeaponEntity>> Weapon = new(nameof(Weapon));
        public static ValueKey<IGameEntity, IRequest> FireRequest = new(nameof(FireRequest));
        public static ValueKey<IGameEntity, ICommand> FireCommand = new(nameof(FireCommand));
        
        // Interact
        public static TagKey InteractableTag = new(nameof(InteractableTag));
        public static TagKey InteractorTag = new(nameof(InteractorTag));
        public static ValueKey<IGameEntity, ICommand<IGameEntity>> InteractCommand = new(nameof(InteractCommand));
        public static ValueKey<IGameEntity, IVariable<bool>> WasUsed = new(nameof(WasUsed));
        
        // Damage
        public static TagKey DamageableTag = new(nameof(DamageableTag));
        public static ValueKey<IGameEntity, IValue<float>> Damage = new(nameof(Damage));
        public static ValueKey<IGameEntity, ICommand<float>> TakeDamageCommand = new(nameof(TakeDamageCommand));
        
        // LifeTime
        public static ValueKey<IGameEntity, ICooldown> Lifetime = new(nameof(Lifetime));
        public static ValueKey<IGameEntity, ICompositeAction> DestroyAction = new(nameof(DestroyAction));

        //public static ValueKey<IGameEntity, ICommand> LifetimeEndCommand = new(nameof(LifetimeEndCommand));

        // AI
        public static ValueKey<IGameEntity, IVariable<bool>> TargetIsReached = new(nameof(TargetIsReached));
    }
}