using UnityEngine;

namespace Game.GameEntities
{
    public struct MovementArgs
    {
        public readonly float Speed;
        public readonly float DeltaTime;
        
        public Vector3 Direction;
        
        public MovementArgs(Vector3 direction, float speed, float deltaTime)
        {
            Direction = direction;
            Speed = speed;
            DeltaTime = deltaTime;
        }
    }
}