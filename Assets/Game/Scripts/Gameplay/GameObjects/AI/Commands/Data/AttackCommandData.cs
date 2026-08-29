using UnityEngine;

namespace SampleGame.AI
{
    public struct AttackCommandData : ICommandData
    {
        public CommandType CommandType => CommandType.Attack;

        // public readonly CommandPoint _point;
        //
        // public AttackCommandData(CommandPoint point)
        // {
        //     _point = point;
        // }

        public readonly Vector3? Point;
        public readonly GameObject Target;
        
        public AttackCommandData(Vector3? point)
        {
            Point = point;
            Target = null;
        }
        
        public AttackCommandData(GameObject target)
        {
            Target = target;
            Point = null;
        }
    }
}