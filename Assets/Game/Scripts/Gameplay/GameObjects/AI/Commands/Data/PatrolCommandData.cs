using System.Collections.Generic;
using UnityEngine;

namespace SampleGame.AI
{
    public struct PatrolCommandData : ICommandData
    {
        public readonly List<CommandPoint> Points;

        public PatrolCommandData(Vector3? basePoint, CommandPoint endPoint)
        {
            Points = new List<CommandPoint>()
            {
                new(basePoint),
                endPoint
            };
        }

        public CommandType Type => CommandType.Patrol;
    }
}