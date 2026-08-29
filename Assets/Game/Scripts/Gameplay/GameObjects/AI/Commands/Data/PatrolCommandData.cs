using System.Collections.Generic;
using UnityEngine;

namespace SampleGame.AI
{
    public struct PatrolCommandData : ICommandData
    {
        public struct Point
        {
            public readonly Vector3? Position;
            public readonly GameObject Target;
        
            public Point(Vector3? point)
            {
                Position = point;
                Target = null;
            }

            public Point(GameObject target)
            {
                Target = target;
                Position = null;
            }
        }
        
        public CommandType CommandType => CommandType.Patrol;

        public List<Point> Points;

        public PatrolCommandData(Vector3? basePoint)
        {
            Points = new List<Point>();
            Points.Add(new Point(basePoint));
        }
    }
}