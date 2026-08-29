using UnityEngine;

namespace SampleGame.AI
{
    public readonly struct MoveCommandData : ICommandData
    {
        public CommandType CommandType => CommandType.Move;

        public readonly Vector3? Point;
        public readonly GameObject Target;
        
        public MoveCommandData(Vector3? point)
        {
            Point = point;
            Target = null;
        }

        public MoveCommandData(GameObject target)
        {
            Target = target;
            Point = null;
        }
    }
}