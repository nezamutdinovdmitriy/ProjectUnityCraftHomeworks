using UnityEngine;

namespace Game.GameEntity
{
    public struct RotateArgs
    {
        public Vector3 Direction;
        public float Speed;
        public float DeltaTime;

        public RotateArgs(Vector3 direction, float speed, float deltaTime)
        {
            Direction = direction;
            Speed = speed;
            DeltaTime = deltaTime;
        }
    }
}