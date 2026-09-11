using UnityEngine;

namespace SampleGame.AI
{
    public readonly struct PositionCommandPoint : ICommandPoint
    {
        public readonly Vector3? Position;

        public PositionCommandPoint(Vector3? position) 
            => Position = position;


        public Vector3? GetPosition() => Position;
    }
}