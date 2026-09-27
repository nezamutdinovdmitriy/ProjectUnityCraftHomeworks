using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public struct PatrolCommandData : ICommandData
    {
        public readonly List<CommandPoint> Points;

        public PatrolCommandData(Vector3? basePoint, CommandPoint endPoint)
        {
            Points = new List<CommandPoint>
            {
                new(basePoint),
                endPoint
            };
        }

        public CommandType Type => CommandType.Patrol;
        
        public void Unpack(Blackboard blackboard)
        {
            blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, 0);
            blackboard.SetReferenceValue(BlackboardAPI.PatrolPoints, Points);
        }

        public void Cleanup(Blackboard blackboard)
        {
            blackboard.DelValue(BlackboardAPI.PatrolPointIndex);
            blackboard.DelValue(BlackboardAPI.Target);
            blackboard.DelValue(BlackboardAPI.TargetPosition);
        }
    }
}