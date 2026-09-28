using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct Money : IComponentData
    {
        public int Value;
    }
}