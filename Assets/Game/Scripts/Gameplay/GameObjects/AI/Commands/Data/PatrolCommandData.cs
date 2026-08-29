using System.Collections.Generic;
using UnityEngine;

namespace SampleGame.AI
{
    public struct PatrolCommandData : ICommandData
    {
        public readonly List<CommandPoint> Points;

        public PatrolCommandData(Vector3? basePoint)
        {
            Points = new List<CommandPoint>();
            Points.Add(new CommandPoint(basePoint));
        }

        public CommandType Type => CommandType.Patrol;
    }
}