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
        public static ValueKey<IGameEntity, IRequest<Vector3>> MovementRequest = new(nameof(MovementRequest));
        public static ValueKey<IGameEntity, ICommand<MovementArgs>> MovementCommand = new(nameof(MovementCommand));
        public static ValueKey<IGameEntity, IValue<float>> MovementSpeed = new(nameof(MovementSpeed));
        
        // Rotation
        public static ValueKey<IGameEntity, IValue<float>> RotationSpeed = new(nameof(RotationSpeed));
        
        // Health
        public static ValueKey<IGameEntity, IValue<float>> MaxHealth = new(nameof(MaxHealth));
        public static ValueKey<IGameEntity, IReactiveVariable<float>> CurrentHealth = new(nameof(CurrentHealth));
    }
}