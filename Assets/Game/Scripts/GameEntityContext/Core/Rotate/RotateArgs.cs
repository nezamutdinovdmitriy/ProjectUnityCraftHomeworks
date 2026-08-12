using UnityEngine;

namespace Game.GameEntities
{
    public struct RotateArgs
    {
        public Vector3 Direction;
        public readonly float Speed;
        public readonly float DeltaTime;

        public RotateArgs(Vector3 direction, float speed, float deltaTime)
        {
            Direction = direction;
            Speed = speed;
            DeltaTime = deltaTime;
        }
    }
}