using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public static class GameEntityAPI
    {
        // Common
        public static ValueKey<IGameEntity, TransformPositionVariable> Position = new(nameof(Position));
        public static ValueKey<IGameEntity, TransformRotationVariable> Rotation = new(nameof(Rotation));
        
        // Movement
        public static ValueKey<IGameEntity, IReactiveVariable<bool>> IsMoving = new(nameof(IsMoving));
        public static ValueKey<IGameEntity, IRequest<Vector3>> MovementRequest = new(nameof(MovementRequest));
        public static ValueKey<IGameEntity, ICommand<MovementArgs>> MovementCommand = new(nameof(MovementCommand));
        public static ValueKey<IGameEntity, IValue<float>> MovementSpeed = new(nameof(MovementSpeed));
        
        // Rotation
        public static ValueKey<IGameEntity, IValue<float>> RotateSpeed = new(nameof(RotateSpeed));
        public static ValueKey<IGameEntity, IRequest<Vector3>> RotateRequest = new(nameof(RotateRequest));
        public static ValueKey<IGameEntity, ICommand<RotateArgs>> RotateCommand = new(nameof(RotateCommand));
        
        // Health
        public static ValueKey<IGameEntity, IValue<float>> MaxHealth = new(nameof(MaxHealth));
        public static ValueKey<IGameEntity, IReactiveVariable<float>> CurrentHealth = new(nameof(CurrentHealth));
        
        // Fire
        public static ValueKey<IGameEntity, IRequest> FireRequest = new(nameof(FireRequest));
        public static ValueKey<IGameEntity, ICommand> FireCommand = new(nameof(FireCommand));
        
        // Aim
        public static ValueKey<IGameEntity, IRequest> AimRequest = new(nameof(AimRequest));
        public static ValueKey<IGameEntity, ICommand> AimCommand = new(nameof(AimCommand));
        public static ValueKey<IGameEntity, IVariable<bool>> HasAimingLastFrame = new(nameof(HasAimingLastFrame));
        public static ValueKey<IGameEntity, ICooldown> AimCooldown = new(nameof(AimCooldown));
    }
}