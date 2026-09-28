using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct ProjectilePrefab : IComponentData
    {
        public Entity Value;
    }
}