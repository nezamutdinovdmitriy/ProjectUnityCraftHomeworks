using UnityEngine;

namespace GameObjects.Components
{
    public static class MoveUseCase
    {
        public static Vector2 GetDirection(Vector3 from, Vector3 to) 
            => ((Vector2)to - (Vector2)from).normalized;

        public static float GetDistance(Vector3 from, Vector3 to) 
            => Vector2.Distance(from, to);

        public static bool IsReached(Vector3 from, Vector3 to, float stoppingDistance) 
            => GetDistance(from, to) <= stoppingDistance;
    }
}