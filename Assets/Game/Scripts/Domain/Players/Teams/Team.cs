using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct Team : IComponentData
    {
        public TeamType Value;
    }
}